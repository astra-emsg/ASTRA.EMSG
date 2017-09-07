using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Business.Services.EntityServices.Common;
using ASTRA.EMSG.Business.Entities.GIS;
using ASTRA.EMSG.Business.Models.GIS;
using ASTRA.EMSG.Business.Infrastructure.Transactioning;
using ASTRA.EMSG.Business.ReflectionMappingConfiguration;
using ASTRA.EMSG.Business.Services.Security;
using ASTRA.EMSG.Business.Services.Historization;
using ASTRA.EMSG.Business.Entities.Common;
using System.Linq.Expressions;

namespace ASTRA.EMSG.Business.Services.EntityServices.GIS
{
    public interface IAchsenUpdateLogService:IService
    {
        List<AchsenImportModel> GetCurrentModels();
    }
    public class AchsenUpdateLogService : MandantDependentEntityServiceBase<AchsenUpdateLog, AchsenImportModel>, IAchsenUpdateLogService
    {
        public AchsenUpdateLogService(ITransactionScopeProvider transactionScopeProvider,
            IEntityServiceMappingEngine entityServiceMappingEngine,
            ISecurityService securityService)
            : base(transactionScopeProvider, entityServiceMappingEngine, securityService)
        { }

       
        protected override void OnModelCreated(AchsenUpdateLog entity, AchsenImportModel model)
        {
            base.OnModelCreated(entity, model);
            model.ErfassungsPeriodeDateTime = entity.ErfassungsPeriod.Erfassungsjahr;   
            model.ImportDateTime = entity.Timestamp;                                     
            model.ImportStatistic = entity.Statistics;                                         
            model.AchsenVersion = entity.ImpNr.ToString();         
        }
        protected override Expression<Func<AchsenUpdateLog, Mandant>> MandantExpression { get { return a => a.Mandant; } }

        public bool HasAtLeastOneUpdate()
        {
            return GetCurrentModels().Count>0;
        }
        
    }
}
