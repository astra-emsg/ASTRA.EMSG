using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Entities.Strassennamen;
using ASTRA.EMSG.Business.Infrastructure.Transactioning;
using ASTRA.EMSG.Business.Models.Strassennamen;
using ASTRA.EMSG.Business.ReflectionMappingConfiguration;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Business.Services.EntityServices.Common;
using ASTRA.EMSG.Business.Services.Historization;
using ASTRA.EMSG.Business.Services.Security;
using ASTRA.EMSG.Business.Utils;
using ASTRA.EMSG.Common.Master.ConfigurationHandling;
using NHibernate.Linq;

namespace ASTRA.EMSG.Business.Services.EntityServices.Strassennamen
{
    public interface IRealisierteMassnahmeOverviewService : IService
    {
        List<RealisierteMassnahmeOverviewModel> GetCurrentModelsByProjektname(string projektnameFilter);
    }
    
    public class RealisierteMassnahmeOverviewService : MandantAndErfassungsPeriodDependentEntityServiceBase<RealisierteMassnahme, RealisierteMassnahmeOverviewModel>, IRealisierteMassnahmeOverviewService
    {
        private readonly IServerConfigurationProvider serverConfigurationProvider;

        public RealisierteMassnahmeOverviewService(
            ITransactionScopeProvider transactionScopeProvider,
            IEntityServiceMappingEngine entityServiceMappingEngine,
            IHistorizationService historizationService,
            ISecurityService securityService,
            IServerConfigurationProvider serverConfigurationProvider)
            : base(transactionScopeProvider, entityServiceMappingEngine, securityService, historizationService)
        {
            this.serverConfigurationProvider = serverConfigurationProvider;
        }

        protected override void OnModelCreated(RealisierteMassnahme entity, RealisierteMassnahmeOverviewModel model)
        {
            base.OnModelCreated(entity, model);
            model.Beschreibung = entity.Beschreibung.TrimToMaxLength(serverConfigurationProvider.MehrzeiligenEingabefelderMaxLengthInPreview);
        }

        public List<RealisierteMassnahmeOverviewModel> GetCurrentModelsByProjektname(string projektnameFilter)
        {
            var query = FilteredEntities;

            if (!string.IsNullOrWhiteSpace(projektnameFilter))
                query = query.Where(s => s.Projektname.ToLower().Contains(projektnameFilter.ToLower()));

            return query
                .Fetch(r => r.MassnahmenvorschlagFahrbahn)
                .Select(CreateModel).ToList();
        }

        protected override Expression<Func<RealisierteMassnahme, ErfassungsPeriod>> ErfassungsPeriodExpression { get { return rms => rms.ErfassungsPeriod; } }
        protected override Expression<Func<RealisierteMassnahme, Mandant>> MandantExpression { get { return rms => rms.Mandant; } }
    }
}