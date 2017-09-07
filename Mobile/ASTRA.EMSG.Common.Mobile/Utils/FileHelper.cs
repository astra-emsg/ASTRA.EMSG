using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using ASTRA.EMSG.Common.Mobile.Logging;

namespace ASTRA.EMSG.Common.Mobile.Utils
{
    public static class FileHelper
    {
        public static void TryDeleteFiles(FileSystemInfo fsi)
        {
            if (fsi.Exists)
            {
                fsi.Attributes = FileAttributes.Normal;
                var di = fsi as DirectoryInfo;

                if (di != null)
                {
                    foreach (var dirInfo in di.GetFileSystemInfos())
                    {
                        TryDeleteFiles(dirInfo);
                    }
                }
                try
                {
                    fsi.Delete();
                }
                catch { }
            }
            System.Environment.CurrentDirectory = System.AppDomain.CurrentDomain.BaseDirectory;
        }
        public static void CopyFile(FileInfo file, DirectoryInfo target)
        {
            if (Directory.Exists(target.FullName) == false)
            {
                Directory.CreateDirectory(target.FullName);
            }
            file.CopyTo(Path.Combine(target.FullName, file.Name), true);
        }
        public static void CopyAll(DirectoryInfo source, DirectoryInfo target, Action<int, string> progressCall = null)
        {
            Loggers.PerformanceLogger.Debug("CopyAll from {0} to {1} started", source.FullName, target.FullName);
            int filecount =0;
            if (progressCall != null)
            {
                filecount = source.GetFiles("*", SearchOption.AllDirectories).Count();
            }
            CopyAll(source, target, filecount, 0, progressCall);
            Loggers.PerformanceLogger.Debug("CopyAll from {0} to {1} finished", source.FullName, target.FullName);
        }
       
        private static int CopyAll(DirectoryInfo source, DirectoryInfo target, int totalFiles, int copiedFiles, Action<int, string> progressCall = null)
        {
            if (source.FullName.ToLower() == target.FullName.ToLower())
            {
                return copiedFiles;
            }

            // Check if the target directory exists, if not, create it.
            if (Directory.Exists(target.FullName) == false)
            {
                Directory.CreateDirectory(target.FullName);
            }

            // Copy each file into it's new directory.
            foreach (FileInfo fi in source.EnumerateFiles())
            {
                if (fi.Attributes != FileAttributes.Normal)
                {
                    fi.Attributes = FileAttributes.Normal;
                }
                fi.CopyTo(Path.Combine(target.ToString(), fi.Name), true);
                if (progressCall != null)
                {
                    copiedFiles++;
                    if (copiedFiles % 10 == 0 || copiedFiles == totalFiles)
                    {
                        progressCall((copiedFiles * 100) / totalFiles, "");
                    }
                }
            }

            // Copy each subdirectory using recursion.
            foreach (DirectoryInfo diSourceSubDir in source.EnumerateDirectories())
            {
                DirectoryInfo nextTargetSubDir =
                    target.CreateSubdirectory(diSourceSubDir.Name);
                copiedFiles = CopyAll(diSourceSubDir, nextTargetSubDir, totalFiles, copiedFiles, progressCall);
            }
            return copiedFiles;
        }
    }
}
