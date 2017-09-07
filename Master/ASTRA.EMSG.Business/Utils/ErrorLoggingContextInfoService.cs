using System;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Common.Master.Logging;

namespace ASTRA.EMSG.Business.Utils
{
    public static class ErrorLoggingContextInfoService
    {
        public static void SetEntityInfo<TEntity>(Guid? id = null)
        {
            try
            {
                ErrorLoggingContextInfoStore.CurrentErrorLoggingContextInfo.EntityType = typeof(TEntity);
                ErrorLoggingContextInfoStore.CurrentErrorLoggingContextInfo.EntityId = id;
            }
            catch (Exception)
            {
            }
        }

        public static void SetEntityInfo<TEntity>(TEntity entity) where TEntity : class, IEntity
        {
            SetContextInfo(entity);
        }

        public static void SetMandantInfo(Mandant mandant)
        {
            SetContextInfo<IEntity>(null, mandant);
        }

        public static void SetMandantInfo(IMandantDependentEntity mandantDependentEntity)
        {
            if(mandantDependentEntity != null)
                SetMandantInfo(mandantDependentEntity.Mandant);
        }

        public static void SetErfassungsPeriodInfo(IErfassungsPeriodDependentEntity erfassungsPeriodDependentEntity)
        {
            if(erfassungsPeriodDependentEntity != null)
                SetErfassungsPeriodInfo(erfassungsPeriodDependentEntity.ErfassungsPeriod);
        }

        public static void SetErfassungsPeriodInfo(ErfassungsPeriod erfassungsPeriod)
        {
            SetContextInfo<IEntity>(null, null, erfassungsPeriod);
        }

        public static void SetContextInfo<TEntity>(TEntity entity = null, Mandant mandant = null, ErfassungsPeriod erfassungsPeriod = null)
            where TEntity : class, IEntity
        {
            try
            {
                var contextInfo = ErrorLoggingContextInfoStore.CurrentErrorLoggingContextInfo;

                if (entity != null)
                    contextInfo.EntityId = entity.Id;

                contextInfo.EntityType = typeof(TEntity);

                if (mandant != null)
                {
                    contextInfo.MandantName = mandant.MandantDisplayName;
                    contextInfo.MandantId = mandant.Id;
                }

                if (erfassungsPeriod != null)
                {
                    contextInfo.ErfassungsPeriodName = erfassungsPeriod.Erfassungsjahr.Year.ToString();
                    contextInfo.MandantId = erfassungsPeriod.Id;
                }
            }
            catch (Exception)
            {
            }
        }
    }
}
