using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASTRA.EMSG.Business.Entities;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Infrastructure.Transactioning;
using ASTRA.EMSG.Common.DataTransferObjects;
using ASTRA.EMSG.Business.ReflectionMappingConfiguration;

namespace ASTRA.EMSG.Business.Services.DTOServices
{
    public interface ISchadendetailDTOService : IDTOServiceBase<Schadendetail, SchadendetailDTO>
    { }
    public class SchadendetailDTOService : DTOServiceBase<Schadendetail, SchadendetailDTO>, ISchadendetailDTOService
    {
        public SchadendetailDTOService(ITransactionScopeProvider transactionScopeProvider, IDataTransferObjectServiceMappingEngine dataTransferObjectServiceMappingEngine)
            : base(transactionScopeProvider, dataTransferObjectServiceMappingEngine)
        { }
    }
}
