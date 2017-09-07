using System.Collections.Generic;
using ASTRA.EMSG.Business.Entities.Katalogs;
using ASTRA.EMSG.Business.Infrastructure.Transactioning;
using ASTRA.EMSG.Business.Models.Katalogs;
using ASTRA.EMSG.Business.ReflectionMappingConfiguration;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Business.Services.EntityServices.Common;
using System.Linq;

namespace ASTRA.EMSG.Business.Services.EntityServices.Katalogs
{
    public interface IGemeindeKatalogService : IService
    {
        List<GemeindeKatalogModel> GetGemeindeKatalogModels();
    }

    public class GemeindeKatalogService : EntityServiceBase<GemeindeKatalog, GemeindeKatalogModel>, IGemeindeKatalogService
    {
        public GemeindeKatalogService(ITransactionScopeProvider transactionScopeProvider, IEntityServiceMappingEngine entityServiceMappingEngine)
            : base(transactionScopeProvider, entityServiceMappingEngine)
        {
        }

        public List<GemeindeKatalogModel> GetGemeindeKatalogModels()
        {
            return FilteredEntities.Select(CreateModel).ToList();
        }
    }
}