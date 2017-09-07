using System.Collections.Generic;
using System.Linq;
using ASTRA.EMSG.Business.Entities.GIS;
using ASTRA.EMSG.Business.Entities.Katalogs;
using ASTRA.EMSG.Business.Entities.Strassennamen;
using ASTRA.EMSG.Business.Infrastructure.Transactioning;
using ASTRA.EMSG.Business.Models.Katalogs;
using ASTRA.EMSG.Business.ReflectionMappingConfiguration;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Business.Services.EntityServices.Common;
using ASTRA.EMSG.Business.Services.Historization;
using ASTRA.EMSG.Business.Services.Security;

namespace ASTRA.EMSG.Business.Services.EntityServices.Katalogs
{
    public interface IMassnahmenvorschlagKatalogOverviewService : IService
    {
        List<MassnahmenvorschlagKatalogOverviewModel> GetCurrentGlobalModels();
        List<MassnahmenvorschlagKatalogOverviewModel> GetCurrentModels();
    }

    public class MassnahmenvorschlagKatalogOverviewService : EntityServiceBase<MassnahmenvorschlagKatalog, MassnahmenvorschlagKatalogOverviewModel>, IMassnahmenvorschlagKatalogOverviewService
    {
        private readonly ISecurityService securityService;
        private readonly IHistorizationService historizationService;
        private readonly ILocalizationService localizationService;
        private readonly IGlobalMassnahmenvorschlagKatalogEditService globalMassnahmenvorschlagKatalogEditService;


        public MassnahmenvorschlagKatalogOverviewService(
            ITransactionScopeProvider transactionScopeProvider, 
            ISecurityService securityService, 
            IHistorizationService historizationService,
            IEntityServiceMappingEngine entityServiceMappingEngine, 
            ILocalizationService localizationService,
            IGlobalMassnahmenvorschlagKatalogEditService globalMassnahmenvorschlagKatalogEditService)
            : base(transactionScopeProvider, entityServiceMappingEngine)
        {
            this.securityService = securityService;
            this.historizationService = historizationService;
            this.localizationService = localizationService;
            this.globalMassnahmenvorschlagKatalogEditService = globalMassnahmenvorschlagKatalogEditService;
        }


        public List<MassnahmenvorschlagKatalogOverviewModel> GetCurrentGlobalModels()
        {
            return CreateModels(Query<GlobalMassnahmenvorschlagKatalog>()).ToList();
        }

        public override List<MassnahmenvorschlagKatalogOverviewModel> GetCurrentModels()
        {
            return CreateModels(Query<MassnahmenvorschlagKatalog>().Where(
                m => m.ErfassungsPeriod == historizationService.GetCurrentErfassungsperiod() && 
                     m.Mandant == securityService.GetCurrentMandant() &&
                     m.IsCustomized
                                    ), calculateIsUsed: false).ToList();
        }

        private List<MassnahmenvorschlagKatalogOverviewModel> CreateModels<T>(IEnumerable<T> massnahmenvorschlagKatalogs, bool calculateIsUsed = true) where T : MassnahmenvorschlagKatalogBase
        {
            var result = massnahmenvorschlagKatalogs.GroupBy(m => m.Parent.Typ)
                                                    .Select(m => new MassnahmenvorschlagKatalogOverviewModel
                                                        {
                                                            Typ = m.Key,
                                                            IsUsed = calculateIsUsed && globalMassnahmenvorschlagKatalogEditService.IsMassnahmenvorschlagKatalogUsed(m.Key),
                                                            TypBezeichnung = localizationService.GetLocalizedMassnahmenvorschlagTyp(m.Key),
                                                            KatalogTyp = m.First().KatalogTyp,
                                                            KatalogTypBezeichnung = localizationService.GetLocalizedEnum(m.First().KatalogTyp),
                                                            KonstenModels = m.OrderBy(gm => gm.Belastungskategorie.Reihenfolge).Select(k => new MassnahmenvorschlagKatalogKonstenEditModel
                                                                {
                                                                    Id = k.Id,
                                                                    DefaultKosten = k.DefaultKosten,
                                                                    Belastungskategorie = k.Belastungskategorie.Id,
                                                                    BelastungskategorieBezeichnung = localizationService.GetLocalizedBelastungskategorieTyp(k.BelastungskategorieTyp)
                                                                }).ToList()
                                                        }
                )
                                                    .ToList();
            return result;
        }
    }
}