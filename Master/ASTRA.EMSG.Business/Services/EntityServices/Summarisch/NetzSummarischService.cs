using System;
using System.Linq;
using System.Linq.Expressions;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Entities.Katalogs;
using ASTRA.EMSG.Business.Entities.Summarisch;
using ASTRA.EMSG.Business.Infrastructure.Transactioning;
using ASTRA.EMSG.Business.Models.Summarisch;
using ASTRA.EMSG.Business.ReflectionMappingConfiguration;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Business.Services.EntityServices.Common;
using ASTRA.EMSG.Business.Services.Historization;
using ASTRA.EMSG.Business.Services.Security;
using NHibernate.Linq;

namespace ASTRA.EMSG.Business.Services.EntityServices.Summarisch
{
    public interface INetzSummarischService : IService
    {
        NetzSummarischModel GetCurrentNetzSummarischModel();
        NetzSummarisch GetCurrentNetzSummarisch();
        void CreateNetzSummarischFor(ErfassungsPeriod erfassungsPeriod);
        NetzSummarisch GetNetzSummarisch(ErfassungsPeriod erfassungsPeriod);
        IQueryable<NetzSummarisch> GetEntitiesBy(ErfassungsPeriod erfassungsPeriod);
        void DeleteEntity(Guid id);
        NetzSummarischModel GetById(Guid id);
        NetzSummarischModel UpdateEntity(NetzSummarischModel netzSummarischModel);
    }

    public class NetzSummarischService : MandantAndErfassungsPeriodDependentEntityServiceBase<NetzSummarisch, NetzSummarischModel>, INetzSummarischService
    {
        public NetzSummarischService(ITransactionScopeProvider transactionScopeProvider, IEntityServiceMappingEngine entityServiceMappingEngine,
            ISecurityService securityService, IHistorizationService historizationService)
            : base(transactionScopeProvider, entityServiceMappingEngine, securityService, historizationService)
        {
        }

        public NetzSummarischModel GetCurrentNetzSummarischModel() { return CreateModel(FilteredEntities.Single()); }
        public NetzSummarisch GetCurrentNetzSummarisch() { return FilteredEntities.Single(); }

        public NetzSummarisch GetNetzSummarisch(ErfassungsPeriod erfassungsPeriod) { return GetEntitiesBy(erfassungsPeriod).FetchMany(ns => ns.NetzSummarischDetails).Single(); }

        public void CreateNetzSummarischFor(ErfassungsPeriod erfassungsPeriod)
        {
            var netzSummarisch = new NetzSummarisch
                                     {
                                         ErfassungsPeriod = erfassungsPeriod,
                                         MittleresErhebungsJahr = null,
                                         Mandant = erfassungsPeriod.Mandant
                                     };

            Create(netzSummarisch);

            foreach (Belastungskategorie belastungskategorie in CurrentSession.Query<Belastungskategorie>())
            {
                Create(new NetzSummarischDetail
                           {
                               NetzSummarisch = netzSummarisch,
                               Belastungskategorie = belastungskategorie
                           });
            }
        }

        protected override Expression<Func<NetzSummarisch, Mandant>> MandantExpression { get { return ns => ns.Mandant; } }
        protected override Expression<Func<NetzSummarisch, ErfassungsPeriod>> ErfassungsPeriodExpression { get { return ns => ns.ErfassungsPeriod; } }
    }
}
