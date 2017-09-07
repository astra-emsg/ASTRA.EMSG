using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASTRA.EMSG.Business.Entities;
using ASTRA.EMSG.Business.Entities.Katalogs;
using ASTRA.EMSG.Business.Infrastructure.Transactioning;
using ASTRA.EMSG.Common.DataTransferObjects;
using ASTRA.EMSG.Business.ReflectionMappingConfiguration;

namespace ASTRA.EMSG.Business.Services.DTOServices
{
    public interface IBelastungskategorieDTOService : IDTOServiceBase<Belastungskategorie, BelastungskategorieDTO>
    { }
    public class BelastungskategorieDTOService : DTOServiceBase<Belastungskategorie, BelastungskategorieDTO>, IBelastungskategorieDTOService
    {
        public BelastungskategorieDTOService(ITransactionScopeProvider transactionScopeProvider, IDataTransferObjectServiceMappingEngine dataTransferObjectServiceMappingEngine)
            : base(transactionScopeProvider, dataTransferObjectServiceMappingEngine)
        { }
    }
}
