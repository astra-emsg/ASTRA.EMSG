using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Xml.Serialization;
using ASTRA.EMSG.Business.Entities.GIS;
using ASTRA.EMSG.Business.Models.GIS;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Business.Services.EntityServices.GIS;
using ASTRA.EMSG.Common.Master.ConfigurationHandling;
using Ionic.Zip;
using ASTRA.EMSG.Business.Services.EntityServices;
using System.Collections.Generic;
using ASTRA.EMSG.Business.Models;
using ASTRA.EMSG.Common;
using System.Linq;
using ASTRA.EMSG.Common.Master.Logging;
using ASTRA.EMSG.Common.DataTransferObjects;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using ASTRA.EMSG.Business.BusinessExceptions;
using ASTRA.EMSG.Common.Utils;


namespace ASTRA.EMSG.Business.Services.PackageService
{
    public interface IPackageService : IService
    {
        ImportResult Import(Stream stream);
        ImportResult ImportZip(Stream stream);
        ExportResult Export(CheckOutGISStreams checkoutGISStream, IList<CheckOutsGISModel> cogModelList);
        
    }

    public class PackageService : IPackageService
    {
        private readonly IServerConfigurationProvider serverConfigurationProvider;
        private readonly IInspektionsRouteGISService inspektionsRouteGISService;
        private readonly IInspektionsRouteGISOverviewService inspektionsRouteGISOverviewService;
        private readonly ICheckOutsGISService checkOutsGISService;
        private readonly ILocalizationService localizationService;

        public PackageService(IServerConfigurationProvider serverConfigurationProvider, 
            IInspektionsRouteGISService inspektionsRouteGISService, 
            IInspektionsRouteGISOverviewService inspektionsRouteGISOverviewService,
            ICheckOutsGISService checkOutsGISService,
            ILocalizationService localizationService)
        {
            this.localizationService = localizationService;
            this.checkOutsGISService = checkOutsGISService;
            this.inspektionsRouteGISOverviewService = inspektionsRouteGISOverviewService;
            this.inspektionsRouteGISService = inspektionsRouteGISService;
            this.serverConfigurationProvider = serverConfigurationProvider;
        }

        public ImportResult Import(Stream stream)
        {
            var importResult = new ImportResult();
            try
            {
                string clientFilesPath = serverConfigurationProvider.ClientFilesFolderPath;

                using (ZipFile zipFile = ZipFile.Read(stream))
                {
                    zipFile.ExtractAll(clientFilesPath, ExtractExistingFileAction.OverwriteSilently);
                }
            }
            catch (Exception ex)
            {
                importResult.Errors.Add(ex.Message);
            }

            return importResult;
        }

        public ImportResult ImportZip(Stream stream)
        {
            var importResult = new ImportResult();

            try
            {
                string clientFilesPath = serverConfigurationProvider.ClientFilesFolderPath;

                using (ZipFile zipFile = ZipFile.Read(stream))
                {
                    
                    try
                    {
                        importResult.descriptor = getClientPackageDescriptor(zipFile, FileNameConstants.ClientPackageDescriptorFileName);
                    }
                    catch (Exception e)
                    {
                        
                        throw new ImportException(localizationService.GetLocalizedError(ValidationError.InvalidPackage), e);
                    } 
                    if(!PackageVersioner.CheckClientPackageVersion(importResult.descriptor.Version))
                    {
                        throw new ImportException(localizationService.GetLocalizedError(ValidationError.PackageVersionNotSupported));
                    }
                    foreach (var id in importResult.descriptor.Inspektionsrouten)
                    {
                        InspektionsRouteGISOverviewModel irov = null;
                        try
                        {
                            irov = inspektionsRouteGISOverviewService.GetById(id.Key);
                        }
                        catch
                        {
                            throw new ImportException(localizationService.GetLocalizedError(ValidationError.InspektionsRouteNotFound));
                        }
                        if (irov == null)
                        {
                            throw new ImportException(localizationService.GetLocalizedError(ValidationError.InspektionsRouteNotFound));
                        }
                        if (irov.Status != InspektionsRouteStatus.RouteExportiert)
                        {

                            throw new ImportException(localizationService.GetLocalizedError(ValidationError.InspektionsRouteNotExported));
                        }


                    }

                    var importedCogList = importResult.descriptor.CheckOutsGISInspektionsroutenList.Where(kvp => importResult.descriptor.Inspektionsrouten.Select(ikvp => ikvp.Key).Contains(kvp.Value));
                    foreach (var cog in importedCogList)
                    {
                        if (!checkOutsGISService.GetById(cog.Key).IsCheckedOut)
                        {
                            throw new ImportException(localizationService.GetLocalizedError(ValidationError.InspektionsRouteNotExported));
                        }
                    }
                    foreach (var filecount in importResult.descriptor.FileCount)
                    {
                        if (!(zipFile.Entries.Where(ze => Path.GetExtension(ze.FileName) == filecount.Key).Count() == filecount.Value))
                        {
                            
                            throw new ImportException(localizationService.GetLocalizedError(ValidationError.InvalidPackage));
                        }
                    }
                   
                    importResult.InspektionsRouten = importResult.descriptor.Inspektionsrouten.Select(ir => ir.Key).ToList();
                    importResult.dtocontainer = getDTOContainer(zipFile, FileNameConstants.DTOContainerFileName);
                    
                }

                
            }
            catch (ImportException ex)
            {
                importResult.Errors.Add(ex.Message);
            }

            return importResult;
        }
        private ClientPackageDescriptor getClientPackageDescriptor(ZipFile zipFile, string filename)
        {
            ZipEntry ze = zipFile[filename];
            Stream ms = new MemoryStream();
            ze.Extract(ms);
            ms.Seek(0, SeekOrigin.Begin);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ClientPackageDescriptor));
            var descriptor = xmlSerializer.Deserialize(ms) as ClientPackageDescriptor;
            ms.Close();
            return descriptor;
            
        }
       
        private DTOContainer getDTOContainer(ZipFile zipFile, string filename)
        {
            ZipEntry ze = zipFile[filename];
            Stream ms = new MemoryStream();
            ze.Extract(ms);
            ms.Seek(0, SeekOrigin.Begin);
            IFormatter _form = new BinaryFormatter();
            var container = _form.Deserialize(ms) as DTOContainer;
            ms.Close();
            return container;

        }


        public ExportResult Export(CheckOutGISStreams checkoutGISStream, IList<CheckOutsGISModel> cogModelList)
        {
            var exportResult = new ExportResult();

            try
            {
                string clientFilesPath = serverConfigurationProvider.ClientFilesFolderPath;

                using (ZipFile zipFile = new ZipFile())
                {
                    zipFile.CompressionLevel = Ionic.Zlib.CompressionLevel.BestSpeed;
                    AddLocalizedResourcesToZip(clientFilesPath, zipFile);

                    zipFile.AddDirectory(Path.Combine(clientFilesPath, FileNameConstants.MobilePackageFolderName), FileNameConstants.MobilePackageFolderName);
                    zipFile.AddEntry(checkoutGISStream.Bezeichnung + ".model", checkoutGISStream.ModelsToExport);
                    foreach (var kvp in checkoutGISStream.LegendStreams)
                    {
                        zipFile.AddEntry(kvp.Key, kvp.Value);
                    }
                    
                    foreach (var tileInfo in checkoutGISStream.Tiles)
                    {
                        System.Uri uri1 = new Uri(Path.Combine(tileInfo.BasePath, tileInfo.RelativePath));
                       
                        foreach (string file in tileInfo.AbsoluteFilePaths)
                        {
                            FileInfo info = new FileInfo(file);
                            DirectoryInfo directory = info.Directory;
                            System.Uri uri2 = new Uri(directory.ToString());
                            Uri relativeUri = uri1.MakeRelativeUri(uri2);
                            zipFile.AddFile(file, Path.Combine(FileNameConstants.TileFolderName,tileInfo.RelativePath,relativeUri.ToString()));
                        }
                    }

                    cogModelList.ToList().ForEach(cog => cog = checkOutsGISService.CreateEntity(cog));
                    AddPackageDescriptor(zipFile, cogModelList, checkoutGISStream);

                    zipFile.Save(exportResult.Stream);
                }
                exportResult.Stream.Seek(0, 0);
            }
            catch (Exception ex)
            {
                Loggers.ApplicationLogger.Error("Exception while creating Zipfile for Export: " + ex.Message, ex);
                exportResult.Errors.Add(ex.Message);
            }

            return exportResult;
        }

        private static void AddLocalizedResourcesToZip(string clientFilesPath, ZipFile zipFile)
        {
            zipFile.AddDirectory(Path.Combine(clientFilesPath, "Resources"), "Resources");
            zipFile.AddDirectory(Path.Combine(clientFilesPath, "Help"), "Help");
        }

        private void AddPackageDescriptor(ZipFile zipFile, IList<CheckOutsGISModel> cogModelList, CheckOutGISStreams checkoutGis)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ServerPackageDescriptor));

            Stream packageDescriptorStream = new MemoryStream();
            var packageDescriptor = new ServerPackageDescriptor
            {
                Version = PackageVersioner.GetServerPackageVersion(),
                CurrentCulture = Thread.CurrentThread.CurrentUICulture.Name
            };
            foreach (var cog in cogModelList)
            {
                var id = cog.InspektionsRouteGIS;
                packageDescriptor.Inspektionsrouten.Add(new XMLKeyValuePair<Guid, string>( id, inspektionsRouteGISService.GetById(id).Bezeichnung));
                packageDescriptor.CheckOutsGISInspektionsroutenList.Add(new XMLKeyValuePair<Guid, Guid>(cog.Id, id)); 
            }
            var filepaths = zipFile.EntryFileNames.ToList();
            
            IList<string> fileextensions = new List<string>();
            filepaths.Where( fp => Path.GetExtension(fp) != string.Empty).ToList().ForEach(fp => fileextensions.Add(Path.GetExtension(fp)));
            foreach (var file in fileextensions.Distinct())
            {
                packageDescriptor.FileCount.Add(new XMLKeyValuePair<string, int>(file, fileextensions.Count(fe => fe==file)));
            }

            packageDescriptor.LayerConfig.AddRange(checkoutGis.Tiles);

            xmlSerializer.Serialize(packageDescriptorStream, packageDescriptor);
            packageDescriptorStream.Seek(0, 0);

            zipFile.AddEntry(FileNameConstants.ServerPackageDescriptorFileName, packageDescriptorStream);
        }
    }
}
