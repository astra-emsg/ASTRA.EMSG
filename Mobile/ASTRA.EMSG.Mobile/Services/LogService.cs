using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASTRA.EMSG.Common.Mobile.Logging;
using NLog.Config;
using NLog.Targets;
using NLog.LayoutRenderers;
using NLog.Layouts;
using NLog.Filters;
using System.IO;
using Ionic.Zip;

namespace ASTRA.EMSG.Mobile.Services
{
    public interface ILogService
    {
        void ExportLog(string path);
    }
    public class LogService : ILogService
    {
        public void ExportLog(string savepath)
        {

            LoggingConfiguration config = Loggers.TechLogger.Factory.Configuration;
            FileTarget standardTarget = config.FindTargetByName("TechLoggerFile") as FileTarget;

            var filename = standardTarget.FileName as SimpleLayout;

            var logfilepath = SimpleLayout.Evaluate(filename.Text);
            string logdir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Path.GetDirectoryName(logfilepath));
            //File.Copy(sourcefilepath, filepath, true);
            using (FileStream sfs = new FileStream(savepath, FileMode.Create))
            {
                using (ZipFile zipFile = new ZipFile())
                {
                    foreach (string logFile in Directory.GetFiles(logdir, "Application*.log", SearchOption.AllDirectories))
                    {
                        using (var fs = new FileStream(logFile, FileMode.Open))
                        {
                            zipFile.AddEntry(Path.GetFileName(logFile), ReadFully(fs));
                        }
                    }

                    zipFile.Save(sfs);
                }
            }

        }
        private byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (var ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

    }
}
