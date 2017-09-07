using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Entities.GIS;
using ASTRA.EMSG.Business.Infrastructure.Filtering;
using ASTRA.EMSG.Business.Infrastructure.Transactioning;
using ASTRA.EMSG.Business.Models.GIS;
using ASTRA.EMSG.Business.ReflectionMappingConfiguration;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Business.Services.EntityServices.Common;
using ASTRA.EMSG.Business.Services.FilterBuilders;
using ASTRA.EMSG.Business.Services.Historization;
using ASTRA.EMSG.Business.Services.Security;
using ASTRA.EMSG.Business.Utils;
using ASTRA.EMSG.Common.Master.ConfigurationHandling;

namespace ASTRA.EMSG.Business.Services.EntityServices.GIS
{
    public class RealisierteMassnahmeGISOverviewFilter : IProjektnameFilter
    {
        public string Projektname { get; set; }
    }

    public interface IRealisierteMassnahmeGISOverviewModelService : IService
    {
        List<RealisierteMassnahmeGISOverviewModel> GetCurrentModelsByProjektname(RealisierteMassnahmeGISOverviewFilter filter);
    }

    public class RealisierteMassnahmeGISOverviewModelService : MandantAndErfassungsPeriodDependentEntityServiceBase<RealisierteMassnahmeGIS, RealisierteMassnahmeGISOverviewModel>, IRealisierteMassnahmeGISOverviewModelService
    {
        private readonly IFiltererFactory filtererFactory;
        private readonly IServerConfigurationProvider serverConfigurationProvider;

        public RealisierteMassnahmeGISOverviewModelService(
            ITransactionScopeProvider transactionScopeProvider, 
            IEntityServiceMappingEngine entityServiceMappingEngine, 
            ISecurityService securityService, 
            IFiltererFactory filtererFactory,
            IHistorizationService historizationService,
            IServerConfigurationProvider serverConfigurationProvider)
            : base(transactionScopeProvider, entityServiceMappingEngine, securityService, historizationService)
        {
            this.filtererFactory = filtererFactory;
            this.serverConfigurationProvider = serverConfigurationProvider;
        }

        protected override void OnModelCreated(RealisierteMassnahmeGIS entity, RealisierteMassnahmeGISOverviewModel model)
        {
            base.OnModelCreated(entity, model);
            model.Beschreibung = entity.Beschreibung.TrimToMaxLength(serverConfigurationProvider.MehrzeiligenEingabefelderMaxLengthInPreview);
        }

        public List<RealisierteMassnahmeGISOverviewModel> GetCurrentModelsByProjektname(RealisierteMassnahmeGISOverviewFilter filter)
        {
            return filtererFactory.CreateFilterer<RealisierteMassnahmeGIS>(filter).Filter(FilteredEntities)
                .Select(rm => new RealisierteMassnahmeGIS()
            {
                Id = rm.Id,
                Projektname = rm.Projektname,
                BezeichnungVon = rm.BezeichnungVon,
                BezeichnungBis = rm.BezeichnungBis,
                Beschreibung = rm.Beschreibung,
                KostenFahrbahn = rm.KostenFahrbahn,
                KostenTrottoirLinks = rm.KostenTrottoirLinks,
                KostenTrottoirRechts = rm.KostenTrottoirRechts,
                Laenge = rm.Laenge,
                BreiteFahrbahn = rm.BreiteFahrbahn,
                BreiteTrottoirLinks = rm.BreiteTrottoirLinks,
                BreiteTrottoirRechts = rm.BreiteTrottoirRechts

            }).Select(CreateModel).ToList();
        }

        protected override Expression<Func<RealisierteMassnahmeGIS, Mandant>> MandantExpression { get { return ir => ir.Mandant; } }
        protected override Expression<Func<RealisierteMassnahmeGIS, ErfassungsPeriod>> ErfassungsPeriodExpression { get { return ir => ir.ErfassungsPeriod; } }
    }
}