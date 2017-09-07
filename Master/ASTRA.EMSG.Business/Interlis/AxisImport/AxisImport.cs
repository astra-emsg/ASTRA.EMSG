using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASTRA.EMSG.Business.Entities.GIS;
using ASTRA.EMSG.Business.Infrastructure.Transactioning;
using ASTRA.EMSG.Business.Interlis.Parser;
using ASTRA.EMSG.Business.Entities;
using ASTRA.EMSG.Business.Interlis.AxisImport.TransactionHandling;
using NHibernate;

namespace ASTRA.EMSG.Business.Interlis.AxisImport
{
    public class AxisImport
    {
        protected readonly IAxisImportMonitor axisImportMonitor;
        protected readonly TransactionScopeFactory transactionScopeFactory;
        public AxisImportStatistics Statistics = new AxisImportStatistics();

        public AxisImport(TransactionScopeFactory transactionScopeFactory, IAxisImportMonitor axisImportMonitor)
        {
            this.transactionScopeFactory = transactionScopeFactory;
            this.axisImportMonitor = axisImportMonitor;
        }

        public AxisImport(TransactionScopeFactory transactionScopeFactory)
        {
            this.axisImportMonitor = new SimpleAxisImportMonitor();
            this.transactionScopeFactory = transactionScopeFactory;
        }

        protected virtual void CleanAchskopieTables(ITransactionCommitter transactionComitter)
        {
            var kopieSektorTypeName = typeof(KopieSektor).Name;
            transactionComitter.Session.Delete(string.Format("from {0}", kopieSektorTypeName));
            transactionComitter.ForceNext();

            var kopieAchsenSegmentTypeName = typeof(KopieAchsenSegment).Name;
            transactionComitter.Session.Delete(string.Format("from {0}", kopieAchsenSegmentTypeName));
            transactionComitter.ForceNext();

            var kopieAchseTypeName = typeof(KopieAchse).Name;
            transactionComitter.Session.Delete(string.Format("from {0}", kopieAchseTypeName));
            transactionComitter.ForceNext();

        }

        /// <summary>
        /// Full Import (just for testing), 
        /// doesn't write ImportLog
        /// </summary>
        /// <param name="filename"></param>
        public void TestRunInitialFullImport(string filename)
        {
            using (MultiTransactionCommitter transactionCommitter = new MultiTransactionCommitter(transactionScopeFactory))
            {
                RunInitialFullImport(transactionCommitter, filename);
                transactionCommitter.Finish();
            }
        }

        private void RunInitialFullImport(ITransactionCommitter transactionComitter, string filename)
        {
            runImport(transactionComitter, filename, 1, importType.Full);
        }

        /// <summary>
        /// Run Incremental Import (just for testing)
        /// doesn't write ImportLog entry
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="impNr"></param>
        public void TestRunIncrementalImport(string filename, int impNr)
        {
            using (var transactionScope = transactionScopeFactory.CreateReadWrite())
            {
                SingleTransactionCommitter transactionCommitter = new SingleTransactionCommitter(transactionScope);
                RunIncrementalImport(transactionCommitter, filename, impNr);
                transactionScope.Commit();
            }
        }

        private void RunIncrementalImport(ITransactionCommitter transactionComitter, string filename, int impNr)
        {
            runImport(transactionComitter, filename, impNr, importType.Incremental);            
        }

        private enum importType
        {
            Incremental=0,
            Full = 1
        }

        AxisImportDataHandler getImportDataHandler(ITransactionCommitter transactionComitter,
                    IAxisImportMonitor axisImportMonitor,int impNr, AxisImportPass importPass, importType type)
        {
            AxisImportDataHandler dataHandler;
            switch (type)
            {
                case importType.Incremental:
                    dataHandler = new AxisImportDataHandlerIncr(transactionComitter,
                axisImportMonitor, impNr, importPass);
                    break;
                case importType.Full:
                    dataHandler = new AxisImportDataHandlerFull(transactionComitter,
               axisImportMonitor, impNr, importPass);
                    break;
                default:
                    dataHandler = null;
                    break;
            }
            return dataHandler;
        }

        private void runImport(ITransactionCommitter transactionComitter, string filename, int impNr, importType type)
        {
            if (type == importType.Full)
            {
                CleanAchskopieTables(transactionComitter);
            }
            {
                AxisImportDataHandler dataHandler = getImportDataHandler(transactionComitter, axisImportMonitor, 
                    impNr, AxisImportPass.Achsen, type);
                AxisReader2 axisReader = new AxisReader2(filename, dataHandler);
                axisReader.Parse();

                Statistics += dataHandler.Statistics;
            }
            if (type == importType.Full)
            {
                transactionComitter.ForceNext(); 
            }
            {
                AxisImportDataHandler dataHandler = getImportDataHandler(transactionComitter, axisImportMonitor,
                    impNr, AxisImportPass.Segmente, type);

                AxisReader2 axisReader = new AxisReader2(filename, dataHandler);
                axisReader.Parse();

                Statistics += dataHandler.Statistics;
            }
            if (type == importType.Full)
            {
                transactionComitter.ForceNext();
            }
            {
                AxisImportDataHandler dataHandler = getImportDataHandler(transactionComitter, axisImportMonitor,
                   impNr, AxisImportPass.Sektoren, type);

                AxisReader2 axisReader = new AxisReader2(filename, dataHandler);
                axisReader.Parse();

                Statistics += dataHandler.Statistics;
            }
            if (type == importType.Full)
            {
                transactionComitter.ForceNext();
            }
        }

        /// <summary>
        /// import file and write import-log.
        /// throws AxisImportCancelledException.
        /// </summary>
        /// <param name="filename"></param>
        ///
        public void RunImport(string filename, DateTime SenderTimestamp)
        {
            int impNr;

            using (var transactionScope = transactionScopeFactory.CreateReadOnly())
            {
                impNr = GetNextImpNR(transactionScope.Session, SenderTimestamp);
            }

            if (impNr == 1)
            {
                try
                {
                    // initial full imports use multiple transactions (performance and memory)
                    using (MultiTransactionCommitter transactionCommitter = new MultiTransactionCommitter(transactionScopeFactory))
                    {
                        CreateImpLogRecord(transactionCommitter.Session, impNr, filename, SenderTimestamp);
                        transactionCommitter.ForceNext();

                        RunInitialFullImport(transactionCommitter, filename);
                        transactionCommitter.ForceNext();

                        UpdateImportLogRecord(transactionCommitter.Session, impNr, AchsenImportLog.SUCCESS, Statistics);
                        transactionCommitter.Finish();
                    }
                }
                catch (Exception)
                {
                    using (var transactionScope = transactionScopeFactory.CreateReadWrite())
                    {
                        try
                        {
                            UpdateImportLogRecord(transactionScope.Session, impNr, AchsenImportLog.FAILED, Statistics);
                            transactionScope.Commit();
                        }
                        catch (Exception)
                        {
                        }
                    }
                    throw;
                }
            }
            else
            {
                using (var transactionScope = transactionScopeFactory.CreateReadWrite())
                {
                    // incremental imports run in a single transaction
                    SingleTransactionCommitter transactionCommitter = new SingleTransactionCommitter(transactionScope);

                    CreateImpLogRecord(transactionCommitter.Session, impNr, filename, SenderTimestamp);
                    RunIncrementalImport(transactionCommitter, filename, impNr);
                    UpdateImportLogRecord(transactionCommitter.Session, impNr, AchsenImportLog.SUCCESS, Statistics);
                    transactionScope.Commit();
                }
            }
        }

        protected virtual int GetNextImpNR(ISession session, DateTime SenderTimestamp)
        {
            var crit = session.CreateCriteria<AchsenImportLog>();

            var logEntries = crit.List<AchsenImportLog>();

            int impNr;

            if (logEntries.Count == 0)
            {
                impNr = 1;
            }
            else
            {
                if (logEntries.Count(o => o.SenderTimestamp >= SenderTimestamp) > 0)
                    throw new AxisImportException("You cannot import an older file. There is at least one newer import than " + SenderTimestamp.ToLongDateString());
 
                impNr = logEntries.Max(o => o.ImpNr) + 1;
            }

            if (logEntries.Where(o => o.Progress == AchsenImportLog.INPROGRESS).Count() > 0)
            {
                throw new AxisImportException("Previous imports in progress!");
            }

            if (logEntries.Where(o => o.Progress != AchsenImportLog.SUCCESS).Count() > 0)
            {
                throw new AxisImportException("Previous imports have failed. can't continue!");
            }


            return impNr;
        }


        protected virtual void CreateImpLogRecord(ISession session, int impNr, string filename, DateTime SenderTimestamp)
        {
            AchsenImportLog impLog = new AchsenImportLog();
            impLog.ImpNr = impNr;
            impLog.Path = filename;
            impLog.Progress = AchsenImportLog.INPROGRESS;
            impLog.Timestamp = DateTime.Now;
            impLog.SenderTimestamp = SenderTimestamp;
            session.Persist(impLog);
        }

        protected virtual void UpdateImportLogRecord(ISession session, int impNr, int progress, AxisImportStatistics statistics)
        {
            var entry = session.QueryOver<AchsenImportLog>().Where(m => m.ImpNr == impNr).SingleOrDefault();

            entry.Progress = progress;

            entry.AchsInserts = statistics.NumAxis.Inserts;
            entry.AchsUpdates = statistics.NumAxis.Updates;
            entry.AchsDeletes = statistics.NumAxis.Deletes;

            entry.SegmInserts = statistics.NumSegment.Inserts;
            entry.SegmUpdates = statistics.NumSegment.Updates;
            entry.SegmDeletes = statistics.NumSegment.Deletes;

            entry.SektInserts = statistics.NumSector.Inserts;
            entry.SektUpdates = statistics.NumSector.Updates;
            entry.SektDeletes = statistics.NumSector.Deletes;
        }

        public virtual void ClearImportLog()
        {
            using (var transactionScope = transactionScopeFactory.CreateReadWrite())
            {

                var allEntries = transactionScope.Session.QueryOver<AchsenImportLog>();

                allEntries.List().ToList().ForEach(o => transactionScope.Session.Delete(o));

                transactionScope.Commit();
            }
        }
    }
}
