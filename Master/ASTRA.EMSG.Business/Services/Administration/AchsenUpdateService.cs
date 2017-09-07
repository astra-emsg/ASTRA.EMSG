using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASTRA.EMSG.Business.Entities;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Infrastructure.Transactioning;
using ASTRA.EMSG.Business.Models;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Business.Services.EntityServices;
using ASTRA.EMSG.Business.Services.EntityServices.Common;
using NHibernate;
using ASTRA.EMSG.Business.Services.Security;
using ASTRA.EMSG.Business.AchsenUpdate.UpdateReferences;
using ASTRA.EMSG.Common.Master.Logging;
using ASTRA.EMSG.Common.Exceptions;
using ASTRA.EMSG.Common.Enums;
using ASTRA.EMSG.Business.Entities.GIS;
using NHibernate.Criterion;


namespace ASTRA.EMSG.Business.Services.Administration
{

    public interface IAchsenUpdateService : IService
    {
        void StartAchsenUpdate();
    }

    public class AchsenUpdateService: IAchsenUpdateService
    {
        private readonly ITransactionScopeProvider transactionScopeProvider;
        private readonly IErfassungsPeriodService erfassungsPeriodService;
        private readonly ISecurityService securityService;
        private readonly IEreignisLogService ereignisLogService;
        private readonly INHibernateConfigurationProvider nHibernateConfigurationProvider;
        private readonly ILocalizationService localizationService;
        private readonly ITimeService timeService;


        public AchsenUpdateService(ITransactionScopeProvider transactionScopeProvider, IErfassungsPeriodService erfassungsPeriodService, ISecurityService securityService, IEreignisLogService ereignisLogService, INHibernateConfigurationProvider nHibernateConfigurationProvider, ILocalizationService localizationService, ITimeService timeService)
        {
            this.nHibernateConfigurationProvider = nHibernateConfigurationProvider;
            this.transactionScopeProvider = transactionScopeProvider;
            this.erfassungsPeriodService = erfassungsPeriodService;
            this.securityService = securityService;
            this.ereignisLogService = ereignisLogService;
            this.localizationService = localizationService;
            this.timeService = timeService;
        }



        /// <summary>
        /// Starts the AchsenUpdate Service
        /// </summary>
        public void StartAchsenUpdate()
        {
            ITransactionScope transactionScope = new TransactionScopeFactory(nHibernateConfigurationProvider).CreateReadWrite();
            Mandant mandant = securityService.GetCurrentMandant();

            var crit = transactionScope.Session.QueryOver<AchsenLock>();
            crit.Where(al => al.Mandant == mandant && al.LockType == LockingType.AchsenUpdate && al.IsLocked == true);
            var achsLocklist = crit.List();

            if (achsLocklist.Count > 0)
            {
                throw new Exception(String.Format(localizationService.GetLocalizedError(ValidationError.UpdateRunning), mandant.MandantDisplayName, achsLocklist.Single().LockStart));
            }

            AchsenLock achsLock = new AchsenLock();
            achsLock.Mandant = mandant;
            achsLock.LockStart = timeService.Now;
            achsLock.IsLocked = true;
            achsLock.LockType = LockingType.AchsenUpdate;
            achsLock.Id = Guid.NewGuid();

            transactionScope.Session.Save(achsLock);
            transactionScope.Commit();
            transactionScope.Dispose();

            try
            {

                AchsenUpdate.AchsenAutoUpdate updater = new AchsenUpdate.AchsenAutoUpdate
                (
                    transactionScopeProvider.CurrentTransactionScope.Session,
                    mandant,
                    erfassungsPeriodService.GetCurrentErfassungsPeriod(mandant),
                    mandant.OwnerId
                );



                updater.ReferenceUpdater = new ReferenceUpdater(transactionScopeProvider.CurrentTransactionScope.Session,
                    erfassungsPeriodService.GetCurrentErfassungsPeriod(mandant));
                updater.Start();

                if (updater.Statistics.SegmentCount() == 0 && updater.Statistics.AxisCount() == 0 && updater.Statistics.SectorCount() == 0)
                {
                    throw new EmsgException(EmsgExceptionType.NoAxisToUpdate);
                }

                ereignisLogService.LogEreignis(EreignisTyp.Achsenupdate, new Dictionary<string, object> { { "Mandant", mandant.MandantDisplayName }, { "OwnerId", mandant.OwnerId } });
            }
            catch (EmsgException e)
            {
                Loggers.ApplicationLogger.Error(String.Format("Exception '{0}' for {1} with MandantId {2}", e.EmsgExceptionType, mandant.MandantDisplayName, mandant.Id));
                
                throw new Exception(localizationService.GetLocalizedEnum<EmsgExceptionType>(e.EmsgExceptionType));
            }
            catch (Exception e)
            {
                Loggers.ApplicationLogger.Error(String.Format("Error while running Achsenupdate for Mandant {0} with MandantId {1}", mandant.MandantDisplayName, mandant.Id), e);
                throw new Exception(localizationService.GetLocalizedError(ValidationError.UnexpectedAchsenUpdateError));
            }
            finally
            {
                transactionScope = new TransactionScopeFactory(nHibernateConfigurationProvider).CreateReadWrite();
                achsLock.IsLocked = false;
                achsLock.LockEnd = timeService.Now;
                transactionScope.Session.Update(achsLock);
                transactionScope.Commit();
                transactionScope.Dispose();

            }
        }




    }
}
