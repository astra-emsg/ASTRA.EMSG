using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Common.Enums;
using ASTRA.EMSG.Common.Master.ConfigurationHandling;
using ASTRA.EMSG.Mobile.Services;
using ASTRA.EMSG.Web.Infrastructure;
using Ionic.Zip;

namespace ASTRA.EMSG.Business.Services.Administration
{
    public interface IHelpSystemService : IService
    {
        Stream DownloadMasterHelpSystem();
        List<string> UploadMasterHelpSystem(Stream stream);

        Stream DownloadMobileHelpSystem();
        List<string> UploadMobileHelpSystem(Stream stream);
    }

    public class HelpSystemService : IHelpSystemService
    {
        public static readonly string HelpFileMappingFileName = "HelpFileMapping.xml";

        private readonly ILocalizationService localizationService;

        private readonly string masterHelpSystem;
        private readonly string mobilHelpSystem;
        private string[] acceptableMobileNames;

        public HelpSystemService(
            IServerConfigurationProvider serverConfigurationProvider, 
            ILocalizationService localizationService, 
            IServerPathProvider serverPathProvider)
        {
            this.localizationService = localizationService;

            mobilHelpSystem = Path.Combine(serverConfigurationProvider.ClientFilesFolderPath, "Help");
            masterHelpSystem = serverPathProvider.MapPath("~/Help");
            LoadNames();
        }

        public Stream DownloadMasterHelpSystem()
        {
            return GetHelpSystemZip(masterHelpSystem);
        }

        private void LoadNames()
        {
            var langPrefix = GetLangPrefixes();
            
            var helpFileNamesObj = new MobileHelpFileNames();
            var names = helpFileNamesObj.GetType().GetFields().Select(p => p.GetValue(helpFileNamesObj));

            acceptableMobileNames = langPrefix.SelectMany(pre => names.Select(name => pre + name)).ToArray();
        }

        public List<string> UploadMasterHelpSystem(Stream stream)
        {
            var result = new List<string>();
            try
            {
                using (ZipFile zipFile = ZipFile.Read(stream))
                {
                    if (IsValidHelpFile(zipFile))
                    {
                        if (!zipFile.Entries.Any(f => f.FileName == HelpFileMappingFileName))
                        {
                            result.Add(
                                string.Format(
                                    localizationService.GetLocalizedError(ValidationError.HelpFileMappingMissing),
                                    HelpFileMappingFileName));
                            return result;
                        }
                        ClearTargetDirectory(masterHelpSystem);
                        zipFile.ExtractAll(masterHelpSystem, ExtractExistingFileAction.OverwriteSilently);
                    }
                    else
                    {
                        result.Add(localizationService.GetLocalizedError(ValidationError.InvalidFileStructure));
                        return result;
                    }
                }
            }
            catch (Exception)
            {
                result.Add(localizationService.GetLocalizedError(ValidationError.WrongFileFormat));
            }

            return result;
        }

        private bool IsValidHelpFile(ZipFile zipFile)
        {
            var folderNames = GetLangPrefixes();
            var valid = folderNames.Select(name => new Regex("^" + name + "[^/]*.htm(l)?$")).ToArray();
            return valid.All(rule => zipFile.EntryFileNames.Any(rule.IsMatch));
        }

        public Stream DownloadMobileHelpSystem()
        {
            return GetHelpSystemZip(mobilHelpSystem);
        }

        public List<string> UploadMobileHelpSystem(Stream stream)
        {
            var result = new List<string>();
            try
            {
                using (ZipFile zipFile = ZipFile.Read(stream))
                {
                    bool allAvailable = true;
                    foreach (var acceptableName in acceptableMobileNames.Where(acceptableName => !zipFile.EntryFileNames.Contains(acceptableName)))
                    {
                        result.Add(string.Format(localizationService.GetLocalizedError(ValidationError.HelpFileMissing), acceptableName));
                        allAvailable = false;
                    }

                    if (allAvailable)
                    {
                        ClearTargetDirectory(mobilHelpSystem);
                        zipFile.ExtractAll(mobilHelpSystem,ExtractExistingFileAction.OverwriteSilently);
                    }
                }
            }
            catch (Exception)
            {
                result.Add(localizationService.GetLocalizedError(ValidationError.WrongFileFormat));
            }

            return result;
        }

        private void ClearTargetDirectory(string taretDirectory)
        {
            RemoveReadOnlyRecoursive(new DirectoryInfo(taretDirectory));

            foreach (var file in Directory.GetFiles(taretDirectory, "*", SearchOption.AllDirectories))
                File.Delete(file);

            foreach (var directory in Directory.GetDirectories(taretDirectory, "*", SearchOption.AllDirectories))
                if (Directory.Exists(directory))
                    Directory.Delete(directory, true);
        }

        private Stream GetHelpSystemZip(string helpSystemPath)
        {
            var zipStream = new MemoryStream();

            using (ZipFile zipFile = new ZipFile())
            {
                zipFile.AddDirectory(helpSystemPath);
                zipFile.Save(zipStream);
            }

            zipStream.Seek(0, 0);

            return zipStream;
        }

        private static IEnumerable<string> GetLangPrefixes()
        {
            return (Enum.GetValues(typeof(EmsgLanguage)) as EmsgLanguage[]).Select(language => language.ToCultureInfo().TwoLetterISOLanguageName + "/");
        }

        private void RemoveReadOnlyRecoursive(FileSystemInfo fileSystemInfo)
        {
            fileSystemInfo.Attributes = FileAttributes.Normal;
            var di = fileSystemInfo as DirectoryInfo;

            if (di != null)
                foreach (var dirInfo in di.GetFileSystemInfos())
                    RemoveReadOnlyRecoursive(dirInfo);
        }
    }
}
