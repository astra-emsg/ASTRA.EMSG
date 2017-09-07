using System;
using System.Collections.Generic;
using System.Security;
using ASTRA.EMSG.Business.Entities;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Models;
using ASTRA.EMSG.Business.Models.Administration;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Business.Services.EntityServices.Common;
using ASTRA.EMSG.Common.Enums;

namespace ASTRA.EMSG.Business.Services.Security
{
    public interface IApplicationSupporterService : IService
    {
        void StartSupporting(AndereBenutzerrollenEinnehmenModel userName);
        void StopSupporting();
        bool IsUserNameValid(string userName);
        AndereBenutzerrollenEinnehmenModel GetCurrentSupportedUserInfo();
        bool IsInSupportMode();
    }
    
    public class ApplicationSupporterService : IApplicationSupporterService
    {
        private readonly ISessionService sessionService;
        private readonly ISecurityService securityService;
        private readonly IEreignisLogService ereignisLogService;

        public ApplicationSupporterService(ISessionService sessionService, ISecurityService securityService, IEreignisLogService ereignisLogService)
        {
            this.sessionService = sessionService;
            this.securityService = securityService;
            this.ereignisLogService = ereignisLogService;
        }

        public void StartSupporting(AndereBenutzerrollenEinnehmenModel andereBenutzerrollenEinnehmenModel)
        {
            var userName = andereBenutzerrollenEinnehmenModel.UserName;

            if(string.IsNullOrEmpty(userName))
            {
                StopSupporting();
                return;
            }

            if(!securityService.IsUserExists(userName))
                throw new InvalidOperationException(string.Format("Username {0} does not exists!", userName));

            if(!securityService.GetCurrentRollen().Contains(Rolle.Applikationssupporter))
                throw new SecurityException("Unathorized!");
            
            sessionService.SupportedUserInfo = new SupportedUserInfo { UserName = userName };
            securityService.SetCurrentApplicationMode(ApplicationMode.Mandant);

            ereignisLogService.LogEreignis(EreignisTyp.RolleEinnehmen, new Dictionary<string, object> {{"benutzer", userName}});
        }

        public void StopSupporting()
        {
            sessionService.SupportedUserInfo = null;
            securityService.SetCurrentApplicationMode(ApplicationMode.Application);
        }

        public bool IsUserNameValid(string userName)
        {
            return string.IsNullOrEmpty(userName) || securityService.IsUserExists(userName);
        }

        public bool IsInSupportMode()
        {
            return sessionService.SupportedUserInfo != null;
        }

        public AndereBenutzerrollenEinnehmenModel GetCurrentSupportedUserInfo()
        {
            return new AndereBenutzerrollenEinnehmenModel
                       {
                           UserName = sessionService.SupportedUserInfo != null
                                          ? sessionService.SupportedUserInfo.UserName
                                          : null
                       };
        }
    }
}