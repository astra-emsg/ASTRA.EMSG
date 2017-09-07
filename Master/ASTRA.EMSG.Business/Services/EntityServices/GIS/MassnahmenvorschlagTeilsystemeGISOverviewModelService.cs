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
    public interface IMassnahmenvorschlagTeilsystemeGISOverviewModelService : IService
    {
        List<MassnahmenvorschlagTeilsystemeGISOverviewModel> GetCurrentModelsByProjektname(string projektnameFilter);
    }

    public class MassnahmenvorschlagTeilsystemeGISOverviewModelService : MandantDependentEntityServiceBase<MassnahmenvorschlagTeilsystemeGIS, MassnahmenvorschlagTeilsystemeGISOverviewModel>, IMassnahmenvorschlagTeilsystemeGISOverviewModelService
    {
        private readonly ILocalizationService localizationService;

        public MassnahmenvorschlagTeilsystemeGISOverviewModelService(
            ITransactionScopeProvider transactionScopeProvider, 
            IEntityServiceMappingEngine entityServiceMappingEngine, 
            ISecurityService securityService,
            ILocalizationService localizationService) 
            : base(transactionScopeProvider, entityServiceMappingEngine, securityService)
        {
            this.localizationService = localizationService;
        }

        protected override MassnahmenvorschlagTeilsystemeGISOverviewModel CreateModel(MassnahmenvorschlagTeilsystemeGIS entity)
        {
            var model = base.CreateModel(entity);
            
            model.TeilsystemBezeichnung = localizationService.GetLocalizedEnum(entity.Teilsystem);
            model.DringlichkeitBezeichnung = localizationService.GetLocalizedEnum(entity.Dringlichkeit);
            model.StatusBezeichnung = localizationService.GetLocalizedEnum(entity.Status);
            return model;
        }

        public List<MassnahmenvorschlagTeilsystemeGISOverviewModel> GetCurrentModelsByProjektname(string projektnameFilter)
        {
            var query = FilteredEntities;

            if (!string.IsNullOrWhiteSpace(projektnameFilter))
                query = query.Where(s => s.Projektname.ToLower().Contains(projektnameFilter.ToLower()));

            return query.Select(m => new MassnahmenvorschlagTeilsystemeGIS()
            {
                Id = m.Id,
                Projektname = m.Projektname,
                BezeichnungVon = m.BezeichnungVon,
                BezeichnungBis = m.BezeichnungBis,
                Teilsystem = m.Teilsystem,
                Dringlichkeit = m.Dringlichkeit,
                Status = m.Status
            }).Select(CreateModel).ToList();
        }

        protected override Expression<Func<MassnahmenvorschlagTeilsystemeGIS, Mandant>> MandantExpression { get { return ir => ir.Mandant; } }
    }
}