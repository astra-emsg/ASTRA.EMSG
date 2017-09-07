using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASTRA.EMSG.Business.Infrastructure.Transactioning;
using ASTRA.EMSG.Business.Interlis.AxisImport;
using System.Text.RegularExpressions;
using System.Threading;
using ASTRA.EMSG.Common.Master.ConfigurationHandling;
using System.IO;
using ASTRA.EMSG.Common.Master.Logging;
using Common.Logging;
using NLog.Config;
using NLog.Targets;
using NLog.LayoutRenderers;
using NLog.Layouts;
using NLog.Filters;
using NLog;

namespace ASTRA.EMSG.Business.Interlis.AxisImportScanner
{
    public class AxisImportScanner : IAxisImportMonitor
    {

        private readonly ImportFolders importFolders;

        private int sleepMs = 60000;

        private bool cancel = false;
        private bool singleShot = false;

        private System.Threading.Thread worker = null;

        private System.IO.StreamWriter logFileWriter = null;

        private System.IO.StreamWriter debugLogWriter = null;

        public AxisImportScanner(string baseDir, int sleepMs = 60000)
        {
            importFolders = new ImportFolders(baseDir);
            this.sleepMs = sleepMs;
        }

        public void StartSyncronous()
        {
            singleShot = true;
            ScanForNewImports();
        }

        public void Start()
        {
            importFolders.CheckFolderStructure();
            Loggers.TechLogger.Debug("AxisImportScanner started at " + DateTime.Now.ToLongTimeString());
            worker = new System.Threading.Thread(new System.Threading.ThreadStart(this.ScanForNewImports));
            worker.Start();
        }

        public void Cancel()
        {
            this.cancel = true;

            if (worker != null)
            {
                worker.Interrupt();
                worker.Join();
                worker = null;
            }
        }


        private void ScanForNewImports()
        {
            try
            {
                importFolders.CheckFolderStructure();

                bool foundImport = false;
                bool firstRun = true;
                while (!cancel)
                {

                    DateTime dtNow = DateTime.Now;
                    if (firstRun || dtNow.Minute == 30 || dtNow.Minute == 0 || foundImport)
                    {
                        firstRun = false;
                        // Time for Check-Run.
                        Loggers.TechLogger.Debug("Starting Import Check");
                        NextImport nextImport = importFolders.GetNextImport();
                        if (nextImport == null)
                        {
                            foundImport = false;
                            if (singleShot) return;  // used with synchronous mode
                            try
                            {
                                System.Threading.Thread.Sleep(sleepMs);
                            }
                            catch (ThreadInterruptedException)
                            {
                            }
                            continue;
                        }
                        foundImport = true;
                        if (!StartImport(nextImport))
                        {
                            Loggers.TechLogger.Error("Errors: Scanner has been stopped!");
                            break;  // exit loop!
                        }

                    }
                    else
                    {
                        System.Threading.Thread.Sleep(sleepMs);
                    }
                }
            }
            catch (Exception ex)
            {
                Loggers.TechLogger.Error("Import Scanner stopped due to fatal error:");
                Loggers.TechLogger.Error(ex.ToString() + "\r\n" + ex.StackTrace);
                cancel = true;
            }
        }

        public bool IsCancelled()
        {
            return cancel;
        }

        public void WriteLog(string text)
        {
            logFileWriter.WriteLine(DateTime.Now + " " + text);
            Loggers.TechLogger.Debug(text);
        }


        private bool StartImport(NextImport nextImport)
        {
            if (logFileWriter != null)
            {
                throw new Exception("logFileWriter != null: are you already running an import?");
            }

            Console.WriteLine("logging to: " + nextImport.FullLogFilename);

            using (logFileWriter = new StreamWriter(nextImport.FullLogFilename))
            {

                try
                {
                    WriteLog("Starting Axis-Import: " + nextImport.FullFilename);

                    TransactionScopeFactory transactionScopeFactory = new TransactionScopeFactory(new MsSQLNHibernateConfigurationProvider(new ServerConfigurationProvider()));
                    {
                        AxisImport.AxisImport axisImport = this.GetImportInstance(transactionScopeFactory);

                        axisImport.RunImport(nextImport.FullFilename, nextImport.SenderTimestamp);

                        logFileWriter.WriteLine("Finished: " + axisImport.Statistics);

                    }

                    System.IO.File.Move(nextImport.FullFilename, nextImport.FullSaveFilename);

                    WriteLog("Moved File to Save-Folder");
                }
                catch (AxisImportCancelledException)
                {
                    WriteLog("Import cancelled");
                    return false;
                }
                catch (Exception ex)
                {
                    WriteLog("Import failed");
                    WriteLog(ex + "\n" + ex.StackTrace);
                    return false;
                }
                finally
                {
                    logFileWriter.Close();
                    logFileWriter = null;
                }
            }
            return true;  // success -> continue
        }

        protected virtual AxisImport.AxisImport GetImportInstance(TransactionScopeFactory transactionScopeFactory)
        {
            return new AxisImport.AxisImport(transactionScopeFactory, this);
        }




    }
}
