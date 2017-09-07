using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Entities.GIS;
using ASTRA.EMSG.Business.Infrastructure.Transactioning;
using ASTRA.EMSG.Business.Models.GIS;
using ASTRA.EMSG.Business.ReflectionMappingConfiguration;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Business.Services.EntityServices.Common;
using ASTRA.EMSG.Business.Services.Security;

namespace ASTRA.EMSG.Business.Services.EntityServices.GIS
{
    public interface IKoordinierteMassnahmeGISOverviewModelService : IService
    {
        List<KoordinierteMassnahmeGISOverviewModel> GetCurrentModelsByProjektname(string projektnameFilter);
    }

    public class KoordinierteMassnahmeGISOverviewModelService : MandantDependentEntityServiceBase<KoordinierteMassnahmeGIS, KoordinierteMassnahmeGISOverviewModel>, IKoordinierteMassnahmeGISOverviewModelService
    {
        private readonly ILocalizationService localizationService;

        public KoordinierteMassnahmeGISOverviewModelService(
            ITransactionScopeProvider transactionScopeProvider, 
            IEntityServiceMappingEngine entityServiceMappingEngine, 
            ISecurityService securityService, ILocalizationService localizationService) 
            : base(transactionScopeProvider, entityServiceMappingEngine, securityService)
        {
            this.localizationService = localizationService;
        }

        public List<KoordinierteMassnahmeGISOverviewModel> GetCurrentModelsByProjektname(string projektnameFilter)
        {
            var query = FilteredEntities;

            if (!string.IsNullOrWhiteSpace(projektnameFilter))
                query = query.Where(s => s.Projektname.ToLower().Contains(projektnameFilter.ToLower()));

            return query.Select(s => new KoordinierteMassnahmeGIS()
            {
                Id = s.Id,
                Projektname = s.Projektname,
                BezeichnungVon = s.BezeichnungVon,
                BezeichnungBis = s.BezeichnungBis,
                Status = s.Status,
                AusfuehrungsAnfang = s.AusfuehrungsAnfang,
                AusfuehrungsEnde = s.AusfuehrungsEnde,
                KostenGesamtprojekt = s.KostenGesamtprojekt,
                LeitendeOrganisation = s.LeitendeOrganisation
            }).Select(CreateModel).ToList();
        }

        protected override KoordinierteMassnahmeGISOverviewModel CreateModel(KoordinierteMassnahmeGIS entity)
        {
            var model = base.CreateModel(entity);

            model.StatusBezeichnung = localizationService.GetLocalizedEnum(entity.Status);

            return model;
        }

        protected override Expression<Func<KoordinierteMassnahmeGIS, Mandant>> MandantExpression { get { return ir => ir.Mandant; } }
    }
}