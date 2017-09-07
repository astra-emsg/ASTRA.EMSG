using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Entities.Summarisch;
using ASTRA.EMSG.Business.Infrastructure.Transactioning;
using ASTRA.EMSG.Business.Models.Summarisch;
using ASTRA.EMSG.Business.ReflectionMappingConfiguration;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Business.Services.EntityServices.Common;
using ASTRA.EMSG.Business.Services.Historization;
using ASTRA.EMSG.Business.Services.Security;
using ASTRA.EMSG.Business.Utils;
using ASTRA.EMSG.Common.Master.ConfigurationHandling;
using NHibernate.Linq;

namespace ASTRA.EMSG.Business.Services.EntityServices.Summarisch
{
    public interface IRealisierteMassnahmeSummarsichOverviewService : IService
    {
        List<RealisierteMassnahmeSummarsichOverviewModel> GetCurrentModelsByProjektname(string projektnameFilter);
    }

    public class RealisierteMassnahmeSummarsichOverviewService : MandantAndErfassungsPeriodDependentEntityServiceBase<RealisierteMassnahmeSummarsich, RealisierteMassnahmeSummarsichOverviewModel>, IRealisierteMassnahmeSummarsichOverviewService
    {
        private readonly ILocalizationService localizationService;
        private readonly IServerConfigurationProvider serverConfigurationProvider;

        public RealisierteMassnahmeSummarsichOverviewService(
            ITransactionScopeProvider transactionScopeProvider,
            IEntityServiceMappingEngine entityServiceMappingEngine,
            IHistorizationService historizationService,
            ILocalizationService localizationService,
            ISecurityService securityService,
            IServerConfigurationProvider serverConfigurationProvider)
            : base(transactionScopeProvider, entityServiceMappingEngine, securityService, historizationService)
        {
            this.localizationService = localizationService;
            this.serverConfigurationProvider = serverConfigurationProvider;
        }

        public override List<RealisierteMassnahmeSummarsichOverviewModel> GetCurrentModels()
        {
            return FilteredEntities.Fetch(rms => rms.Belastungskategorie).Select(CreateModel).ToList();
        }

        protected override void OnModelCreated(RealisierteMassnahmeSummarsich entity, RealisierteMassnahmeSummarsichOverviewModel model)
        {
            model.BelastungskategorieBezeichnung = entity.Belastungskategorie == null ? null : localizationService.GetLocalizedBelastungskategorieTyp(entity.BelastungskategorieTyp);
            model.Beschreibung = entity.Beschreibung.TrimToMaxLength(serverConfigurationProvider.MehrzeiligenEingabefelderMaxLengthInPreview);
        }

        public List<RealisierteMassnahmeSummarsichOverviewModel> GetCurrentModelsByProjektname(string projektnameFilter)
        {
            var query = FilteredEntities;

            if (!string.IsNullOrWhiteSpace(projektnameFilter))
                query = query.Where(s => s.Projektname.ToLower().Contains(projektnameFilter.ToLower()));

            return query.Fetch(rms => rms.Belastungskategorie).Select(CreateModel).ToList();
        }

        protected override Expression<Func<RealisierteMassnahmeSummarsich, ErfassungsPeriod>> ErfassungsPeriodExpression { get { return rms => rms.ErfassungsPeriod; } }
        protected override Expression<Func<RealisierteMassnahmeSummarsich, Mandant>> MandantExpression { get { return rms => rms.Mandant; } }
    }
}