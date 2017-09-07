using System;
using System.Collections.Generic;
using System.Linq;
using ASTRA.EMSG.Business.Entities.Katalogs;
using ASTRA.EMSG.Business.Infrastructure.Transactioning;
using ASTRA.EMSG.Business.Models.Katalogs;
using ASTRA.EMSG.Business.ReflectionMappingConfiguration;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Business.Services.EntityServices.Common;

namespace ASTRA.EMSG.Business.Services.EntityServices.Katalogs
{
    public interface IBelastungskategorieService : IService
    {
        List<BelastungskategorieModel> AllBelastungskategorieModel { get; }
        List<Belastungskategorie> AlleBelastungskategorie { get; }
        BelastungskategorieModel GetBelastungskategorie(string belastungskategorieTyp);
        BelastungskategorieModel GetBelastungskategorie(Guid? belastungskategorieId);
    }

    public class BelastungskategorieService : EntityServiceBase<Belastungskategorie, BelastungskategorieModel>, IBelastungskategorieService
    {
        public BelastungskategorieService(ITransactionScopeProvider transactionScopeProvider, IEntityServiceMappingEngine entityServiceMappingEngine)
            : base(transactionScopeProvider, entityServiceMappingEngine)
        {
        }

        public List<BelastungskategorieModel> AllBelastungskategorieModel { get { return AlleBelastungskategorie.Select(CreateModel).ToList(); } }

        private List<Belastungskategorie> alleBelastungskategorie;
        public List<Belastungskategorie> AlleBelastungskategorie
        {
            get
            {
                return alleBelastungskategorie ?? (alleBelastungskategorie = Queryable.OrderBy(bk => bk.Reihenfolge).ToList());
            }
        }

        public BelastungskategorieModel GetBelastungskategorie(string belastungskategorieTyp)
        {
            Belastungskategorie belastungskategorie = AlleBelastungskategorie.SingleOrDefault(bk => bk.Typ == belastungskategorieTyp);
            return belastungskategorie == null ? null : CreateModel(belastungskategorie);
        }

        public BelastungskategorieModel GetBelastungskategorie(Guid? belastungskategorieId)
        {
            Belastungskategorie belastungskategorie = AlleBelastungskategorie.SingleOrDefault(bk => bk.Id == belastungskategorieId);
            return belastungskategorie == null ? null : CreateModel(belastungskategorie);
        }
    }
}