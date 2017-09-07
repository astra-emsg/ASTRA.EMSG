using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASTRA.EMSG.Business.Entities.GIS;
using ASTRA.EMSG.Business.Infrastructure.Transactioning;
using ASTRA.EMSG.Common.DataTransferObjects;
using ASTRA.EMSG.Business.Entities;
using ASTRA.EMSG.Business.ReflectionMappingConfiguration;

namespace ASTRA.EMSG.Business.Services.DTOServices
{
    public interface IAchsenReferenzDTOService : IDTOServiceBase<AchsenReferenz, AchsenReferenzDTO>
    { }
    public class AchsenReferenzDTOService : DTOServiceBase<AchsenReferenz, AchsenReferenzDTO>, IAchsenReferenzDTOService
    {
        public AchsenReferenzDTOService(ITransactionScopeProvider transactionScopeProvider, IDataTransferObjectServiceMappingEngine dataTransferObjectServiceMappingEngine)
            : base(transactionScopeProvider, dataTransferObjectServiceMappingEngine)
        { }
    }
}
