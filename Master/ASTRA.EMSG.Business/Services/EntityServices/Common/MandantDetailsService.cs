using System;
using System.Linq.Expressions;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Infrastructure.Transactioning;
using ASTRA.EMSG.Business.Models.Administration;
using ASTRA.EMSG.Business.ReflectionMappingConfiguration;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Business.Services.Historization;
using ASTRA.EMSG.Business.Services.Security;
using System.Linq;

namespace ASTRA.EMSG.Business.Services.EntityServices.Common
{
    public interface IMandantDetailsService : IService
    {
        MandantDetailsModel GetCurrentMandantDetailsModel();
        MandantDetails GetCurrentMandantDetails();
        MandantDetailsModel UpdateEntity(MandantDetailsModel mandantDetailsModel);
        MandantDetails GetEntityByMandant(Guid mandantId);
        IQueryable<MandantDetails> GetEntitiesBy(Mandant mandant);
        IQueryable<MandantDetails> GetEntitiesBy(ErfassungsPeriod erfassungsPeriod);
        MandantDetails CreateCopy(ErfassungsPeriod closedPeriod);
        MandantDetails GetMandantDetailsByJahr(DateTime erfassungsJahr);
        void EnableAchsenEdit(Guid mandantDetailsId);
    }

    public class MandantDetailsService : MandantAndErfassungsPeriodDependentEntityServiceBase<MandantDetails, MandantDetailsModel>, IMandantDetailsService
    {
        public MandantDetailsService(
            ITransactionScopeProvider transactionScopeProvider, 
            IEntityServiceMappingEngine entityServiceMappingEngine, 
            ISecurityService securityService, 
            IHistorizationService historizationService
            )
            : base(transactionScopeProvider, entityServiceMappingEngine, securityService, historizationService)
        {
        }

        protected override void OnEntityUpdating(MandantDetails entity)
        {
            base.OnEntityUpdating(entity);

            entity.IsCompleted =
                entity.DifferenzHoehenlageSiedlungsgebiete.HasValue &&
                entity.Einwohner.HasValue &&
                entity.Gemeindeflaeche.HasValue &&
                entity.Gemeindetyp != null &&
                entity.MittlereHoehenlageSiedlungsgebiete.HasValue &&
                entity.OeffentlicheVerkehrsmittel != null &&
                entity.Siedlungsflaeche.HasValue &&
                entity.Steuerertrag.HasValue;
        }

        public MandantDetails GetCurrentMandantDetails()
        {
            return FilteredEntities.Single();
        }

        public MandantDetails GetEntityByMandant(Guid mandantId)
        {
            return Queryable.Where(md => !md.ErfassungsPeriod.IsClosed).SingleOrDefault(md => md.Mandant.Id == mandantId);
        }

        public MandantDetailsModel GetCurrentMandantDetailsModel()
        {
            return CreateModel(GetCurrentMandantDetails());
        }

        public MandantDetails CreateCopy(ErfassungsPeriod closedPeriod)
        {
            var mandantDetailsToCopy = GetEntitiesBy(closedPeriod).Single();
            var copiedMandantDetails = entityServiceMappingEngine.Translate<MandantDetails, MandantDetails>(mandantDetailsToCopy);
            return CreateEntity(copiedMandantDetails);
        }

        public MandantDetails GetMandantDetailsByJahr(DateTime erfassungsJahr)
        {
            var mandantDetailsByJahr = Queryable.SingleOrDefault(m => m.ErfassungsPeriod.Erfassungsjahr == erfassungsJahr && m.Mandant == securityService.GetCurrentMandant());
            if (mandantDetailsByJahr == null) //the selected year is a KenngroessenFruehererJahre so use the current MandantDetails
                mandantDetailsByJahr = GetCurrentMandantDetails();
            return mandantDetailsByJahr;
        }

        public void EnableAchsenEdit(Guid mandantDetailsId)
        {
            var entity = CurrentSession.Get<MandantDetails>(mandantDetailsId);
            entity.IsAchsenEditEnabled = true;
            Update(entity);
        }

        protected override Expression<Func<MandantDetails, Mandant>> MandantExpression { get { return md => md.Mandant; } }
        protected override Expression<Func<MandantDetails, ErfassungsPeriod>> ErfassungsPeriodExpression { get { return md => md.ErfassungsPeriod; } }
    }
}