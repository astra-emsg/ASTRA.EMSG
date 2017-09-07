using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Entities.Katalogs;
using ASTRA.EMSG.Business.Entities.Strassennamen;
using ASTRA.EMSG.Business.Infrastructure.Transactioning;
using ASTRA.EMSG.Business.Models.Strassennamen;
using ASTRA.EMSG.Business.ReflectionMappingConfiguration;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Business.Services.EntityServices.Common;
using ASTRA.EMSG.Business.Services.Historization;
using ASTRA.EMSG.Business.Services.Security;
using NHibernate.Linq;

namespace ASTRA.EMSG.Business.Services.EntityServices.Strassennamen
{
    public interface IRealisierteMassnahmeService : IService
    {
        void DeleteEntity(Guid id);
        RealisierteMassnahmeModel GetById(Guid id);
        RealisierteMassnahmeModel CreateEntity(RealisierteMassnahmeModel realisierteMassnahmeSummarsichModel);
        RealisierteMassnahmeModel UpdateEntity(RealisierteMassnahmeModel realisierteMassnahmeSummarsichModel);
        IQueryable<RealisierteMassnahme> GetEntitiesBy(ErfassungsPeriod getErfassungsPeriod);
        IQueryable<RealisierteMassnahme> GetEntitiesBy(ErfassungsPeriod getErfassungsPeriod, Mandant mandant);
    }

    public class RealisierteMassnahmeService : MandantAndErfassungsPeriodDependentEntityServiceBase<RealisierteMassnahme, RealisierteMassnahmeModel>, IRealisierteMassnahmeService
    {
        private readonly ILocalizationService localizationService;

        public RealisierteMassnahmeService(
            ITransactionScopeProvider transactionScopeProvider,
            IEntityServiceMappingEngine entityServiceMappingEngine,
            IHistorizationService historizationService,
            ILocalizationService localizationService,
            ISecurityService securityService)
            : base(transactionScopeProvider, entityServiceMappingEngine, securityService, historizationService)
        {
            this.localizationService = localizationService;
        }

        protected override void OnModelCreated(RealisierteMassnahme entity, RealisierteMassnahmeModel model)
        {
            if (entity.BreiteFahrbahn == 0)
                model.BreiteFahrbahn = null;
        }

        protected override Expression<Func<RealisierteMassnahme, ErfassungsPeriod>> ErfassungsPeriodExpression { get { return rms => rms.ErfassungsPeriod; } }
        protected override Expression<Func<RealisierteMassnahme, Mandant>> MandantExpression { get { return rms => rms.Mandant; } }
    }
}