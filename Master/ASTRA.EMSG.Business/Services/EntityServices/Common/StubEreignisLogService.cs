using System.Collections.Generic;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Infrastructure.Transactioning;
using ASTRA.EMSG.Business.ReflectionMappingConfiguration;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Business.Services.Security;

namespace ASTRA.EMSG.Business.Services.EntityServices.Common
{
    public class StubEreignisLogService : EreignisLogService
    {
        public StubEreignisLogService(ISecurityService securityService, ITimeService timeService, ILocalizationService localizationService, ITransactionScopeProvider transactionScopeProvider, IEntityServiceMappingEngine entityServiceMappingEngine) : base(securityService, timeService, localizationService, transactionScopeProvider, entityServiceMappingEngine)
        {
        }

        public override void LogEreignis(EreignisTyp typ, IDictionary<string, object> data)
        {
        }
    }
}