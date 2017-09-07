using System;
using System.Collections.Generic;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Models.Administration;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Business.Services.EntityServices.Common;
using ASTRA.EMSG.Common.Enums;
using ASTRA.EMSG.Common.Exceptions;
using ASTRA.EMSG.Common.Master.Logging;
using System.Linq;
using ASTRA.EMSG.Business.Services.Identity;
using ASTRA.EMSG.Common.Utils;

namespace ASTRA.EMSG.Business.Services.Security
{
    public interface ISecurityService : IService
    {
        Mandant GetCurrentMandant();
        List<Mandant> GetCurrentUserMandanten();
        UserInfoModel GetCurrentUserInfo();
        void SetCurrentMandant(Guid mandantId);
        bool IsAuthenticated { get; }
        List<Rolle> GetCurrentRollen();
        string GetCurrentUserName();
        bool IsUserExists(string username);
        void SetCurrentApplicationMode(ApplicationMode applicationMode);
        List<ApplicationMode> GetSupportedApplicationModes();
        ApplicationMode GetCurrentApplicationMode();
        List<string> GetNotInitializedMandanten();
    }

    public class SecurityService : ISecurityService
    {
        private readonly Rolle[] applicationLevelRollen = new[]{Rolle.Applikationsadministrator, Rolle.Applikationssupporter};
        private readonly Rolle[] mandantLevelRollen = new[]{Rolle.Benchmarkteilnehmer, Rolle.Benutzeradministrator, Rolle.DataManager, Rolle.DataReader};

        private readonly IMandantenService mandantenService;
        private readonly ICookieService cookieService;
        private readonly IUserIdentityProvider userIdentityProvider;
        private readonly ISessionService sessionService;
        private readonly IIdentityCacheService identityCacheService;

        public SecurityService(IUserIdentityProvider userIdentityProvider, 
            ISessionService sessionService, 
            IMandantenService mandantenService, 
            ICookieService cookieService,
            IIdentityCacheService identityCacheService)
        {
            this.userIdentityProvider = userIdentityProvider;
            this.sessionService = sessionService;
            this.mandantenService = mandantenService;
            this.cookieService = cookieService;
            this.identityCacheService = identityCacheService;
        }

        public string CurrentUserName { get { return userIdentityProvider.UserName; } }
        public string ActiveUserName
        {
            get
            {
                //Note: AppSupport
                if (sessionService.SupportedUserInfo != null)
                    return sessionService.SupportedUserInfo.UserName;
                return CurrentUserName;
            }
        }

        public Mandant GetCurrentMandant()
        {
            var currentApplicationMode = GetCurrentApplicationModeInternal();
            if(currentApplicationMode != ApplicationMode.Mandant)
                throw new InvalidOperationException("ApplicationMode ist nich Mandant!");

            if (SelectedMandantId.HasValue)
            {
                var mandant = mandantenService.GetMandantById(SelectedMandantId.Value);
                if(mandant != null)
                {
                    var mandantRoles = identityCacheService.GetRoleMandator(mandant.MandantName, ActiveUserName);
                    if(mandantRoles.Any())
                        return mandant;
                }
            }

            var currentMandant = GetCurrentUserMandanten().First();
            SetCurrentMandant(currentMandant.Id);

            return currentMandant;
        }

        public ApplicationMode GetCurrentApplicationMode()
        {
            var currentApplicationMode = GetCurrentApplicationModeInternal();
            if (currentApplicationMode == null)
                throw new InvalidOperationException("CurrentApplicationMode is NULL!");

            return currentApplicationMode.Value;
        }

        private ApplicationMode? GetCurrentApplicationModeInternal()
        {
            var applicationMode = SelectedApplicationMode;
            var supportedApplicationModes = GetSupportedApplicationModes();

            if (supportedApplicationModes.IsEmpty())
                return null;

            if (applicationMode.HasValue && supportedApplicationModes.Contains(applicationMode.Value))
                return applicationMode;

            SetCurrentApplicationMode(supportedApplicationModes.First());
            return supportedApplicationModes.First();
        }

        private Guid? SelectedMandantId { get { return cookieService.SelectedMandantId; } }

        public void SetCurrentMandant(Guid mandantId)
        {
            cookieService.SelectedMandantId = mandantId;
        }

        private ApplicationMode? SelectedApplicationMode { get { return cookieService.SelectedApplicationMode; } }

        public void SetCurrentApplicationMode(ApplicationMode applicationMode)
        {
            cookieService.SelectedApplicationMode = applicationMode;
        }

        public List<Mandant> GetCurrentUserMandanten()
        {
            var mandantenNames = identityCacheService.GetMandatorShort(ActiveUserName)
                .Select(mandatorShort => mandatorShort.Mandatorname)
                .OrderByDescending(lms => lms)
                .ToList();

            var mandanten = mandantenService.GetCurrentMandanten();
            return mandanten.Where(m => mandantenNames.Contains(m.MandantName)).OrderBy(m => m.MandantName).ToList();
        }

        public List<ApplicationMode> GetSupportedApplicationModes()
        {
            var applicationModes = new List<ApplicationMode>();

            var userRoles = identityCacheService.GetRole(ActiveUserName);
            
            var hasApplicationLevelRolle = userRoles.Any(lr => applicationLevelRollen.Contains(lr.Rolle));
            if (hasApplicationLevelRolle)
                applicationModes.Add(ApplicationMode.Application);

            var mandantNames = mandantenService.GetCurrentMandanten().Select(mm => mm.MandantName);
            var hasMandantLevelRolle = userRoles.Any(lr => mandantLevelRollen.Contains(lr.Rolle) && mandantNames.Contains(lr.MandatorName));
            if (hasMandantLevelRolle)
                applicationModes.Add(ApplicationMode.Mandant);
            
            if (applicationModes.IsEmpty())
            {
                applicationModes.Add(ApplicationMode.NoMandants);
                return applicationModes;
            }

            return applicationModes;
        }

        public List<string> GetNotInitializedMandanten()
        {
            var mandantenNames = identityCacheService.GetRole(ActiveUserName).Select(lr => lr.MandatorName).Distinct();
            var emsgMandantNames = mandantenService.GetCurrentMandanten().Select(mm => mm.MandantName);

            return mandantenNames.Where(lm => lm != null && !emsgMandantNames.Contains(lm)).ToList();
        }

        public UserInfoModel GetCurrentUserInfo()
        {
            return new UserInfoModel
                       {
                           CurrentUserName = CurrentUserName,
                           CurrentMandantName = GetCurrentApplicationMode() == ApplicationMode.Application ? "-" : GetCurrentMandant().MandantName,
                           CurrentRollen = string.Join(", ", GetCurrentRollen().Select(r => r.ToString()))
                       };
        }

        public bool IsAuthenticated
        {
            get
            {
                if(!userIdentityProvider.IsAuthenticated)
                    return false;

                if (!GetCurrentApplicationModeInternal().HasValue)
                    throw new EmsgException(EmsgExceptionType.NoPermissions);

                return true;
            }
        }

        public List<Rolle> GetCurrentRollen()
        {
            var userRoles = identityCacheService.GetRole(ActiveUserName);

            var currentApplicationMode = GetCurrentApplicationModeInternal();
            if (!currentApplicationMode.HasValue)
                throw new InvalidOperationException("CurrentApplicationMode ist NULL!");
            
            if(currentApplicationMode.Value == ApplicationMode.Application)
            {
                return AdjustRollenList(userRoles.Where(lr => applicationLevelRollen.Contains(lr.Rolle))
                    .Select(lr => lr.Rolle)
                    .Distinct());
            }

            if(currentApplicationMode.Value == ApplicationMode.Mandant)
            {
                var astraMandatorName = GetCurrentMandant().MandantName;
                return AdjustRollenList(userRoles.Where(lr => lr.MandatorName == astraMandatorName && mandantLevelRollen.Contains(lr.Rolle))
                    .Select(lr => lr.Rolle)
                    .Distinct());
            }

            if (currentApplicationMode.Value == ApplicationMode.NoMandants)
            {
                return new List<Rolle>();
            }

            throw new NotSupportedException(string.Format("ApplicationMode {0} ist nicht supportiert!", currentApplicationMode));
        }

        private List<Rolle> AdjustRollenList(IEnumerable<Rolle> rollen)
        {
            //Note: AppSupport
            var rolleList = rollen.ToList();
            if (sessionService.SupportedUserInfo != null)
            {
                rolleList.Remove(Rolle.Applikationsadministrator);

                if (!rolleList.Contains(Rolle.Applikationssupporter))
                    rolleList.Add(Rolle.Applikationssupporter);
            }
            return rolleList;
        }

        public string GetCurrentUserName()
        {
            return CurrentUserName;
        }

        public bool IsUserExists(string username)
        {
            var mandatorShorts = identityCacheService.GetMandatorShort(username);
            return identityCacheService.IsUserExists(username) && mandatorShorts != null && mandatorShorts.Any();
        }
    }
}