using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Resources;
using ASTRA.EMSG.Business.Services.Common;
using System.Linq;
using ASTRA.EMSG.Common;
using ASTRA.EMSG.Common.Master.ConfigurationHandling;
using Ionic.Zip;
using ASTRA.EMSG.Common.Utils;

namespace ASTRA.EMSG.Business.Services.Administration
{
    public interface IGlobalResourceHandlerService : IService
    {
        Stream DownloadResources();
        List<string> UploadResource(Stream stream, string uploadFileName);
    }

    public class GlobalResourceHandlerService : IGlobalResourceHandlerService
    {
        private readonly ILocalizationService localizationService;

        public GlobalResourceHandlerService(
            ILocalizationService localizationService, 
            IServerConfigurationProvider serverConfigurationProvider, 
            IServerPathProvider serverPathProvider)
        {
            this.localizationService = localizationService;
            mobileClientResources = Path.Combine(serverConfigurationProvider.ClientFilesFolderPath, "Resources");
            appGlobalResources = serverPathProvider.MapPath(@"~/App_GlobalResources");
        }

        private readonly string[] resourceTypes = new[] { ".de.resx", ".fr.resx", ".it.resx" };
        private readonly string appGlobalResources;
        private readonly string mobileClientResources;

        public Stream DownloadResources()
        {
            var resourceFiles = GetResourceFiles();

            var zipStream = new MemoryStream();
            using (ZipFile zipFile = new ZipFile())
            {
                foreach (var resourceFile in resourceFiles)
                    zipFile.AddFile(resourceFile, string.Empty);

                zipFile.Save(zipStream);
            }
            
            zipStream.Seek(0, 0);
            return zipStream;
        }

        public List<string> UploadResource(Stream stream, string uploadFileName)
        {
            string fileName = Path.GetFileName(uploadFileName);

            if(!fileName.HasText())
                return GetLocalizedError(ValidationError.InvalidResourceFileName);

            var resourceFiles = GetResourceFiles();
            var resourceType = fileName.Substring(fileName.Length - 8, 8);

            var currentResourceFile = resourceFiles.SingleOrDefault(rf => Path.GetFileName(rf) == fileName);
            if (!resourceTypes.Contains(resourceType) || !currentResourceFile.HasText())
                return GetLocalizedError(ValidationError.InvalidResourceFileName);

            try
            {
                using(var currentResourceFileStream = File.OpenRead(currentResourceFile))
                {
                    if (GetResourceFileKeyCount(stream) < GetResourceFileKeyCount(currentResourceFileStream))
                        return GetLocalizedError(ValidationError.InvalidResourceFileKeyCount);
                }
            }
            catch (Exception)
            {
                return GetLocalizedError(ValidationError.InvalidResourceFileFormat);
            }

            using (var targetStream = File.Open(currentResourceFile, FileMode.Truncate))
            {
                stream.Seek(0, 0);
                stream.CopyTo(targetStream);
            }

            return new List<string>();
        }

        private List<string> GetLocalizedError(ValidationError validationError)
        {
            return new List<string> { localizationService.GetLocalizedError(validationError) };
        }

        private int GetResourceFileKeyCount(Stream stream)
        {
            int keyCount;
            using (var resXResourceReader = new ResXResourceReader(stream))
            {
                keyCount = resXResourceReader.Cast<DictionaryEntry>().Count();
            }

            return keyCount;
        }

        private List<string> GetResourceFiles()
        {
            var resourceFiles = resourceTypes.SelectMany(rt => Directory.GetFiles(appGlobalResources, string.Format("*{0}", rt), SearchOption.AllDirectories)).ToList();
            resourceFiles.Add(Path.Combine(mobileClientResources, FileNameConstants.GermanMobileLocalizationFileName));
            resourceFiles.Add(Path.Combine(mobileClientResources, FileNameConstants.FrenchMobileLocalizationFileName));
            resourceFiles.Add(Path.Combine(mobileClientResources, FileNameConstants.ItalyMobileLocalizationFileName));
            return resourceFiles;
        }
    }
}
