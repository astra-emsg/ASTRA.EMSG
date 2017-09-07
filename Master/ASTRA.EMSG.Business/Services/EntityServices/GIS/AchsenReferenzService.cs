using System.Collections.Generic;
using System.Linq;
using ASTRA.EMSG.Business.Entities.GIS;
using ASTRA.EMSG.Business.Infrastructure.Transactioning;
using System;
using ASTRA.EMSG.Business.Models.GIS;
using ASTRA.EMSG.Business.ReflectionMappingConfiguration;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Business.Services.EntityServices.Common;

namespace ASTRA.EMSG.Business.Services.EntityServices.GIS
{
    public interface IAchsenReferenzService : IService
    {
        void CreateOrUpdateList(IList<AchsenReferenz> entityList);
        List<AchsenReferenz> GetAchsenReferenzGruppe(Guid groupId);
        void DeleteEntity(Guid id);
        IQueryable<AchsenReferenz> GetCurrentEntities();
        AchsenReferenzModel GetById(Guid id);
    }

    public class AchsenReferenzService : EntityServiceBase<AchsenReferenz, AchsenReferenzModel>, IAchsenReferenzService
    {
        public AchsenReferenzService(ITransactionScopeProvider transactionScopeProvider, IEntityServiceMappingEngine entityServiceMappingEngine)
            : base(transactionScopeProvider, entityServiceMappingEngine)
        {
        }

        public void CreateOrUpdateList(IList<AchsenReferenz> entityList)
        {
            foreach (AchsenReferenz achsenreferenz in entityList)
                CreateOrUpdateEntity(CreateEntity(achsenreferenz));
        }

        public List<AchsenReferenz> GetAchsenReferenzGruppe(Guid groupId) 
        {
            return Queryable.Where(ar => ar.ReferenzGruppe.Id == groupId).ToList();
        }
    }
}