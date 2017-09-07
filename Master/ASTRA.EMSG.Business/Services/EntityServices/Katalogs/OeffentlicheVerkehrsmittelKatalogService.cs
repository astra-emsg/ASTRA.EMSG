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
    public interface IOeffentlicheVerkehrsmittelKatalogService : IService
    {
        List<OeffentlicheVerkehrsmittelKatalogModel> GetOeffentlicheVerkehrsmittelKatalogModels();
    }

    public class OeffentlicheVerkehrsmittelKatalogService : EntityServiceBase<OeffentlicheVerkehrsmittelKatalog, OeffentlicheVerkehrsmittelKatalogModel>, IOeffentlicheVerkehrsmittelKatalogService
    {
        public OeffentlicheVerkehrsmittelKatalogService(ITransactionScopeProvider transactionScopeProvider, IEntityServiceMappingEngine entityServiceMappingEngine)
            : base(transactionScopeProvider, entityServiceMappingEngine)
        {
        }

        public List<OeffentlicheVerkehrsmittelKatalogModel> GetOeffentlicheVerkehrsmittelKatalogModels()
        {
            return FilteredEntities.Select(CreateModel).ToList();
        }
    }
}