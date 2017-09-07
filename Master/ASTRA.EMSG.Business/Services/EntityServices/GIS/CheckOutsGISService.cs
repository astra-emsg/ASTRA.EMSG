using System;
using ASTRA.EMSG.Business.Entities.GIS;
using ASTRA.EMSG.Business.Infrastructure.Transactioning;
using System.Linq;
using ASTRA.EMSG.Business.Models.GIS;
using ASTRA.EMSG.Business.ReflectionMappingConfiguration;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Business.Services.EntityServices.Common;

namespace ASTRA.EMSG.Business.Services.EntityServices.GIS
{
    public interface ICheckOutsGISService : IService
    {
        CheckOutsGISModel GetById(Guid id);
        IQueryable<CheckOutsGIS> GetCurrentEntities();
        CheckOutsGISModel CreateEntity(CheckOutsGISModel model);
        CheckOutsGISModel UpdateEntity(CheckOutsGISModel model);
    }

    public class CheckOutsGISService : EntityServiceBase<CheckOutsGIS, CheckOutsGISModel>, ICheckOutsGISService
    {
        public CheckOutsGISService(ITransactionScopeProvider transactionScopeProvider, IEntityServiceMappingEngine entityServiceMappingEngine)
            : base(transactionScopeProvider, entityServiceMappingEngine)
        {
        }

    }
}
