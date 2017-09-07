using System;
using System.Collections.Generic;
using System.Linq;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Infrastructure.Transactioning;
using ASTRA.EMSG.Business.Models.Administration;
using ASTRA.EMSG.Business.ReflectionMappingConfiguration;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Business.Services.Security;
using ASTRA.EMSG.Common.Utils;

namespace ASTRA.EMSG.Business.Services.EntityServices.Common
{
    public interface IEreignisLogService : IService
    {
        void LogEreignis(EreignisTyp typ, IDictionary<string, object> data);
        List<EreignisLogOverviewModel> GetModelsByFilter(EreignisLogOverviewParameter filterParameter);
        void ClearLog();
    }

    public class EreignisLogService : EntityServiceBase<EreignisLog, EreignisLogOverviewModel>, IEreignisLogService
    {
        private readonly ISecurityService securityService;
        private readonly ITimeService timeService;
        private readonly ILocalizationService localizationService;

        public EreignisLogService(
            ISecurityService securityService,
            ITimeService timeService,
            ILocalizationService localizationService,
            ITransactionScopeProvider transactionScopeProvider, 
            IEntityServiceMappingEngine entityServiceMappingEngine
            ) : base(transactionScopeProvider, entityServiceMappingEngine)
        {
            this.securityService = securityService;
            this.timeService = timeService;
            this.localizationService = localizationService;
        }

        public virtual void LogEreignis(EreignisTyp typ, IDictionary<string, object> data)
        {
            string ereignisData = string.Join(", ", data.Select(p => string.Format("{0}: {1}", p.Key, p.Value)));
            if (ereignisData.Length > 5000)
                ereignisData = ereignisData.Substring(0, 5000);
            
            CreateEntity(new EreignisLog
            {
                Benutzer = securityService.GetCurrentUserName(),
                MandantName = GetMandantNameForCurrentUser(),
                EreignisTyp = typ,
                Zeit = timeService.Now,
                EreignisData = ereignisData
            });
        }

        private string GetMandantNameForCurrentUser()
        {
            switch (securityService.GetCurrentApplicationMode())
            {
                case ApplicationMode.Mandant:
                    return securityService.GetCurrentMandant().MandantName;
                case ApplicationMode.Application:
                    return "-";
                case ApplicationMode.NoMandants:
                    return localizationService.GetLocalizedText("NoMandantsEreignisLog"); //TODO: localization
            }
            throw new InvalidOperationException("Application mode not supported!");
        }

        public List<EreignisLogOverviewModel> GetModelsByFilter(EreignisLogOverviewParameter filterParameter)
        {
            var queryable = FilteredEntities;

            if (filterParameter.Benutzer.HasText())
                queryable = queryable.Where(el => el.Benutzer.ToLower().Contains(filterParameter.Benutzer.ToLower()));

            if (filterParameter.Mandant.HasText())
                queryable = queryable.Where(el => el.MandantName.ToLower().Contains(filterParameter.Mandant.ToLower()));

            if (filterParameter.EreignisTyp.HasValue)
                queryable = queryable.Where(el => el.EreignisTyp == filterParameter.EreignisTyp.Value);

            if (filterParameter.ZeitVon.HasValue)
                queryable = queryable.Where(el => el.Zeit >= filterParameter.ZeitVon.Value);

            if (filterParameter.ZeitBis.HasValue)
                queryable = queryable.Where(el => el.Zeit <= filterParameter.ZeitBis.Value);

            return queryable.Select(CreateModel).ToList();
        }

        public void ClearLog()
        {
            foreach (var logEntry in FilteredEntities.ToList())
                Delete(logEntry);
        }

        protected override void OnModelCreated(EreignisLog entity, EreignisLogOverviewModel model)
        {
            base.OnModelCreated(entity, model);
            model.EreignisTypBezeichnung = localizationService.GetLocalizedEnum(entity.EreignisTyp);
        }
    }
}