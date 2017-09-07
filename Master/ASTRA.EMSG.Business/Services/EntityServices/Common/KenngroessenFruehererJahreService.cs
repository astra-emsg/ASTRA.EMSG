using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Infrastructure.Transactioning;
using ASTRA.EMSG.Business.Models.Common;
using ASTRA.EMSG.Business.ReflectionMappingConfiguration;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Business.Services.Security;
using System.Linq;
using NHibernate.Linq;

namespace ASTRA.EMSG.Business.Services.EntityServices.Common
{

    public interface IKenngroessenFruehererJahreService : IService
    {
        void DeleteEntity(Guid id);
        KenngroessenFruehererJahreModel GetById(Guid id);
        KenngroessenFruehererJahre GetEntityById(Guid id);
        KenngroessenFruehererJahreModel UpdateEntity(KenngroessenFruehererJahreModel strassenabschnittModel);
        KenngroessenFruehererJahreModel CreateEntity(KenngroessenFruehererJahreModel strassenabschnittModel);
        IQueryable<KenngroessenFruehererJahre> GetEntitiesBy(Mandant mandant);
        bool IsJahrUnique(KenngroessenFruehererJahreModel model);
        List<KenngroessenFruehererJahre> GetKenngroessenFruehererJahren(int jahrVon, int jahrBis);
        void DeleteAllKenngroessenFruehererJahre(IQueryable<KenngroessenFruehererJahre> kenngroessenFruehererJahres);
    }

    public class KenngroessenFruehererJahreService : MandantDependentEntityServiceBase<KenngroessenFruehererJahre, KenngroessenFruehererJahreModel>, IKenngroessenFruehererJahreService
    {
        public KenngroessenFruehererJahreService(ITransactionScopeProvider transactionScopeProvider, IEntityServiceMappingEngine entityServiceMappingEngine, ISecurityService securityService) : base(transactionScopeProvider, entityServiceMappingEngine, securityService)
        {
        }

        protected override Expression<Func<KenngroessenFruehererJahre, Mandant>> MandantExpression { get { return k => k.Mandant; } }

        public bool IsJahrUnique(KenngroessenFruehererJahreModel model)
        {
            return !FilteredEntities.Any(e => e.Jahr == model.Jahr && e.Id != model.Id);
        }

        protected override void UpdateEntityFieldsInternal(KenngroessenFruehererJahreModel model, KenngroessenFruehererJahre entity)
        {
            base.UpdateEntityFieldsInternal(model, entity);
            foreach (var detailModel in model.KenngroesseFruehereJahrDetailModels)
            {
                var detailEntity = entity.KenngroesseFruehereJahrDetails.SingleOrDefault(d => d.Id == detailModel.Id);
                bool isNew = false;
                if (detailEntity == null)
                {
                    isNew = true;
                    detailEntity = new KenngroessenFruehererJahreDetail();
                }
                detailModel.Id = detailEntity.Id;
                entityServiceMappingEngine.Translate(detailModel, detailEntity);
                if (isNew)
                {
                    detailEntity.KenngroessenFruehererJahre = entity;
                    entity.KenngroesseFruehereJahrDetails.Add(detailEntity);
                }
            }
        }

        protected override void OnModelCreated(KenngroessenFruehererJahre entity, KenngroessenFruehererJahreModel model)
        {
            base.OnModelCreated(entity, model);
            model.KenngroesseFruehereJahrDetailModels = entity.KenngroesseFruehereJahrDetails
                .Select(CreateModelFromEntity<KenngroessenFruehererJahreDetailModel, KenngroessenFruehererJahreDetail>).OrderBy(m => m.BelastungskategorieTyp).ToList();
        }

        KenngroessenFruehererJahre IKenngroessenFruehererJahreService.GetEntityById(Guid id)
        {
            return GetEntityById(id);
        }

        public List<KenngroessenFruehererJahre> GetKenngroessenFruehererJahren(int jahrVon, int jahrBis)
        {
            return FilteredEntities
                .Where(kfj => jahrVon <= kfj.Jahr && kfj.Jahr <= jahrBis)
                .OrderBy(kfj => kfj.Jahr)
                .FetchMany(kfj => kfj.KenngroesseFruehereJahrDetails)
                .ThenFetch(kfjd => kfjd.Belastungskategorie)
                .ToList();
        }

        public void DeleteAllKenngroessenFruehererJahre(IQueryable<KenngroessenFruehererJahre> kenngroessenFruehererJahres)
        {
            foreach (var kenngroessenFruehererJahre in kenngroessenFruehererJahres)
            {
                this.DeleteEntity(kenngroessenFruehererJahre.Id);
            }
        }
    }
}
