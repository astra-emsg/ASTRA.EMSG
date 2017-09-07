using System;
using System.Collections.Generic;
using System.Linq;
using ASTRA.EMSG.Business.Entities.GIS;
using ASTRA.EMSG.Business.Infrastructure.Transactioning;
using ASTRA.EMSG.Business.Models.GIS;
using ASTRA.EMSG.Business.ReflectionMappingConfiguration;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Business.Services.EntityServices.Common;

namespace ASTRA.EMSG.Business.Services.EntityServices.GIS
{
    
    public interface IInspektionsRtStrAbschnitteService : IService
    {
        void CreateOrUpdate(InspektionsRtStrAbschnitteModel inspektionsRtStrAbschnitteModel);
        void DeleteEntity(Guid id);
        InspektionsRtStrAbschnitteModel GetById(Guid id);
        InspektionsRtStrAbschnitteModel GetByStrassenabschnittId(Guid strassenabschnittId);
        List<InspektionsRtStrAbschnitteModel> GetCurrentModels();
        IQueryable<InspektionsRtStrAbschnitte> GetCurrentEntities();
        bool HasInspektionsRouteGISJustUniqueStraasenabschnitten(InspektionsRouteGISModel inspektionsRouteGISModel, IList<InspektionsRtStrAbschnitteModel> inspektionsRtStrAbschnitteModelList);
    }

    public class InspektionsRtStrAbschnitteService : EntityServiceBase<InspektionsRtStrAbschnitte, InspektionsRtStrAbschnitteModel>, IInspektionsRtStrAbschnitteService
    {
        private readonly IStrassenabschnittGISService strassenabschnittGISService;
        private readonly ITransactionScopeProvider transactionScopeProvider;

        public InspektionsRtStrAbschnitteService(IEntityServiceMappingEngine entityServiceMappingEngine, IStrassenabschnittGISService strassenabschnittGISService, ITransactionScopeProvider transactionScopeProvider) :
            base(transactionScopeProvider, entityServiceMappingEngine)
        {
            this.transactionScopeProvider = transactionScopeProvider;
            this.strassenabschnittGISService = strassenabschnittGISService;
        }
        
        protected override void UpdateEntityFieldsInternal(InspektionsRtStrAbschnitteModel model, InspektionsRtStrAbschnitte entity)
        {
            entity.StrassenabschnittGIS = transactionScopeProvider.GetById<StrassenabschnittGIS>(model.StrassenabschnittId);
            base.UpdateEntityFieldsInternal(model, entity);
        }
        
        protected override InspektionsRtStrAbschnitteModel CreateModel(InspektionsRtStrAbschnitte entity)
        {
            var model= base.CreateModel(entity);
            FillInModel(model, entity.StrassenabschnittGIS);
            return model;
        }

        private void FillInModel(InspektionsRtStrAbschnitteModel model, StrassenabschnittGIS strassenabschnittGIS)
        {
            model.StrassenabschnittId = strassenabschnittGIS.Id;
            model.Strassenname = strassenabschnittGIS.Strassenname;
            model.Strasseneigentuemer = strassenabschnittGIS.Strasseneigentuemer;
            model.BezeichnungVon = strassenabschnittGIS.BezeichnungVon;
            model.BezeichnungBis = strassenabschnittGIS.BezeichnungBis;
        }

        public InspektionsRtStrAbschnitteModel GetByStrassenabschnittId(Guid strassenabschnittId)
        {
            var strassenabschnittGIS = GetEntityById<StrassenabschnittGIS>(strassenabschnittId);
            var model = new InspektionsRtStrAbschnitteModel();
            FillInModel(model, strassenabschnittGIS);
            return model;
        }

        public bool HasInspektionsRouteGISJustUniqueStraasenabschnitten(InspektionsRouteGISModel inspektionsRouteGISModel, IList<InspektionsRtStrAbschnitteModel> inspektionsRtStrAbschnitteModelList)
        {
            var inspektionsRouteGIS = GetEntityById<InspektionsRouteGIS>(inspektionsRouteGISModel.Id);
            List<Guid> irsList = inspektionsRtStrAbschnitteModelList.Select(irsa => irsa.StrassenabschnittId).ToList();

            if(inspektionsRouteGIS != null)
            {
                return !transactionScopeProvider.Queryable<InspektionsRouteGIS>()
                    .Any(ir => ir.Id != inspektionsRouteGIS.Id && ir.InspektionsRtStrAbschnitteList.Any(irsa => irsList.Contains(irsa.StrassenabschnittGIS.Id)));
            }
            
            return !transactionScopeProvider.Queryable<InspektionsRouteGIS>()
                .Any(ir => ir.InspektionsRtStrAbschnitteList.Any(irsa => irsList.Contains(irsa.StrassenabschnittGIS.Id)));
        }
    }
}
