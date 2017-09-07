using System;
using System.IO;
using System.Reflection;
using ASTRA.EMSG.Common.Mobile.Logging;
using ASTRA.EMSG.Localization;
using ASTRA.EMSG.Localization.Utils;
using Ionic.Zip;
using System.Linq;
using System.Collections.Generic;
using ASTRA.EMSG.Common;
using System.Xml.Serialization;
using ASTRA.EMSG.Common.DataTransferObjects;
using System.Collections.ObjectModel;
using ASTRA.EMSG.Common.Mobile.Utils;
using ASTRA.EMSG.Common.Utils;

using ASTRA.EMSG.Mobile.BusinessExceptions;
using ASTRA.EMSG.Map.Services;
using ASTRA.EMSG.Common.Enums;
using System.Resources;
using System.Collections;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Windows;

namespace ASTRA.EMSG.Mobile.Services
{
    public interface IPackageService
    {
        void Export(string file, bool exportAll);
        void Import(string file);
        ServerPackageDescriptor GetSeverpackageDescriptor(string path);
    }

    public class PackageService : IPackageService
    {
        private readonly IMessageBoxService messageBoxService;
        private readonly IClientConfigurationProvider clientConfigurationProvider;
        private readonly ILanguageService languageService;
        private readonly IExportService exportService;
        private readonly IDTOService dtoService;
        private readonly ILoadService loadService;
        private readonly IGeoJsonService geoJsonService;
        private readonly IFormService formService;       
        private readonly IWindowService windowService;
        private readonly IProgressService progressService;

        public PackageService(IMessageBoxService messageBoxService,
            IClientConfigurationProvider clientConfigurationProvider,
            ILanguageService languageService,
            IExportService exportService,
            IDTOService dtoService,
            ILoadService loadService,
            IGeoJsonService geoJsonService,
            IFormService formService,
            IWindowService windowService,
            IProgressService progressService
           )
        {
            this.dtoService = dtoService;
            this.exportService = exportService;
            this.messageBoxService = messageBoxService;
            this.clientConfigurationProvider = clientConfigurationProvider;
            this.languageService = languageService;
            this.loadService = loadService;
            this.geoJsonService = geoJsonService;
            this.formService = formService;
            this.windowService = windowService;
            this.progressService = progressService;
        }

        public void Export(string file, bool exportAll)
        {
            FileHelper.TryDeleteFiles(new DirectoryInfo(clientConfigurationProvider.ExportPackageFolderPath));


            ClientPackageDescriptor mdescriptor = new ClientPackageDescriptor();
            ServerPackageDescriptor sdescriptor = GetWorkingSeverpackageDescriptor();


            if (exportAll)
            {
                mdescriptor.CheckOutsGISInspektionsroutenList = sdescriptor.CheckOutsGISInspektionsroutenList;
                mdescriptor.Inspektionsrouten = sdescriptor.Inspektionsrouten;
            }
            else
            {
                //mdescriptor.CheckOutsGISInspektionsroutenList = sdescriptor.CheckOutsGISInspektionsroutenList.Where(kvp => kvp.Value == layerService.getActiveInspektionsroute()).ToList();
                //mdescriptor.Inspektionsrouten = sdescriptor.Inspektionsrouten.Where(kvp => kvp.Key == layerService.getActiveInspektionsroute()).ToList();
                mdescriptor.CheckOutsGISInspektionsroutenList = sdescriptor.CheckOutsGISInspektionsroutenList.Where(kvp => kvp.Value == formService.GetActiveInspektionsRoute()).ToList();
                mdescriptor.Inspektionsrouten = sdescriptor.Inspektionsrouten.Where(kvp => kvp.Key == formService.GetActiveInspektionsRoute()).ToList();

            }

            mdescriptor.CurrentCulture = sdescriptor.CurrentCulture;


            mdescriptor.Version = PackageVersioner.GetClientPackageVersion();

            string tempExportDirectory = Path.Combine(clientConfigurationProvider.ExportPackageFolderPath, Guid.NewGuid().ToString());
            EnsurePackageFolder(tempExportDirectory);
            DTOContainer dtoContainer = exportService.Export(tempExportDirectory, exportAll);

            string[] allfilePaths = Directory.GetFiles(tempExportDirectory, "*",
                                             SearchOption.AllDirectories);



            var dtofileNames = allfilePaths.Where(p => Path.GetExtension(p) == ".model");
            mdescriptor.FileCount.Add(new XMLKeyValuePair<string, int>(".model", dtofileNames.Count()));

            var fileNames = dtofileNames;

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ClientPackageDescriptor));
            Stream packageDescriptorStream = new MemoryStream();
            xmlSerializer.Serialize(packageDescriptorStream, mdescriptor);
            packageDescriptorStream.Seek(0, 0);


            try
            {
                if (File.Exists(file))
                    File.Delete(file);

                using (var zipFile = new ZipFile(file))
                {
                    zipFile.AddFiles(fileNames, false, string.Empty);
                    zipFile.AddEntry(FileNameConstants.ClientPackageDescriptorFileName, packageDescriptorStream);
                    zipFile.Save(file);
                }

                if (exportAll)
                {
                    //Delete the Inspektionsrouten entries in the Packagedescriptor lying in the Working Dir (CurrentTemporaryFolder),
                    //OnApplicationExiting there is a Save (copy WorkingDir to PackageDir =>If there are no Inspektionsrouten, then the PackageDir will be deleted)

                    var descriptor = GetSeverpackageDescriptor(Directory.GetFiles(clientConfigurationProvider.CurrentTemporaryFolder, FileNameConstants.ServerPackageDescriptorFileName,
                                                  SearchOption.AllDirectories).SingleOrDefault());
                    descriptor.Inspektionsrouten.Clear();
                    xmlSerializer = new XmlSerializer(typeof(ServerPackageDescriptor));
                    packageDescriptorStream = new System.IO.FileStream(Path.Combine(clientConfigurationProvider.CurrentTemporaryFolder, FileNameConstants.ServerPackageDescriptorFileName), FileMode.Create, FileAccess.ReadWrite);
                    xmlSerializer.Serialize(packageDescriptorStream, descriptor);
                    packageDescriptorStream.Close();

                    loadService.ClearAll(true);
                }
                else
                {
                    foreach (var dto in dtoContainer.DataTransferObjects)
                    {
                        dtoService.DeleteDTO(dto.Id);
                    }


                    //Delete the to be exported Inspektionsroute in the Shape Files

                    //Delete the Inspektionsrouten entries in the Packagedescriptor lying in the Working Dir (CurrentTemporaryFolder)
                    var descriptor = GetSeverpackageDescriptor(Directory.GetFiles(clientConfigurationProvider.CurrentTemporaryFolder, FileNameConstants.ServerPackageDescriptorFileName,
                                              SearchOption.AllDirectories).SingleOrDefault());




                    descriptor.Inspektionsrouten.Remove(descriptor.Inspektionsrouten.Where(ir => ir.Key == formService.GetActiveInspektionsRoute()).SingleOrDefault());
                    descriptor.CheckOutsGISInspektionsroutenList.Remove(descriptor.CheckOutsGISInspektionsroutenList.Where(ir => ir.Value == formService.GetActiveInspektionsRoute()).SingleOrDefault());


                    xmlSerializer = new XmlSerializer(typeof(ServerPackageDescriptor));
                    packageDescriptorStream = new System.IO.FileStream(Path.Combine(clientConfigurationProvider.CurrentTemporaryFolder, FileNameConstants.ServerPackageDescriptorFileName), FileMode.Create, FileAccess.ReadWrite);
                    xmlSerializer.Serialize(packageDescriptorStream, descriptor);
                    packageDescriptorStream.Close();




                    //Delete the Inspektionsroute from the Dictionary for displaying
                    //all changes to an Observable Collection must be done by the same Thread it was created by, becaus of Thread Affinity
                    Action action = new Action(() => { loadService.InspektionsroutenDictionary.Remove(loadService.InspektionsroutenDictionary.Where(ir => ir.Key == formService.GetActiveInspektionsRoute()).SingleOrDefault()); });
                    Application.Current.Dispatcher.Invoke(action);                    
                    if (descriptor.Inspektionsrouten.IsEmpty())
                    {
                        loadService.ClearAll(true);
                    }



                    //Let Map know about the export
                    IEnumerable<AchsenSegmentDTO> achsenSegmente = dtoService.Get<AchsenSegmentDTO>();
                    IEnumerable<StrassenabschnittGISDTO> strabs = dtoService.Get<StrassenabschnittGISDTO>();
                    IEnumerable<ZustandsabschnittGISDTO> zabs = dtoService.Get<ZustandsabschnittGISDTO>();

                    string achsString = geoJsonService.GenerateGeoJsonStringFromEntities(achsenSegmente);
                    string strabString = geoJsonService.GenerateGeoJsonStringFromEntities(strabs);
                    string zabString = geoJsonService.GenerateGeoJsonStringFromEntities(zabs);

                    DataChangedEventArgs args = new DataChangedEventArgs();
                    args.AchsenGeoJson = achsString;
                    args.StrabsGeoJson = strabString;
                    args.ZabsGeoJson = zabString;
                    sdescriptor.LayerConfig.ForEach(c => c.BasePath = clientConfigurationProvider.TileFolder);
                    args.LayerInfo = sdescriptor.LayerConfig;
                    args.ActiveInspectionRouteId = formService.GetActiveInspektionsRoute();
                    MobileLocalization localization = LocalizationLocator.MobileLocalization;
                    foreach (var propertyInfo in typeof(MobileLocalization).GetProperties())
                    {
                        if (propertyInfo.CanRead)
                        {
                            args.MobileLocalization.Add(propertyInfo.Name, propertyInfo.GetValue(localization, null).ToString());
                        }
                    }
                    string[] sldPaths;
                    if (clientConfigurationProvider.UseLocalFiles)
                    {
                        sldPaths = Directory.GetFiles(".\\Mobile\\style\\sld", "*.sld", SearchOption.TopDirectoryOnly);
                    }
                    else
                    {
                        sldPaths = Directory.GetFiles(Path.Combine(clientConfigurationProvider.MobilePackageFolder, "style\\sld"), "*.sld", SearchOption.TopDirectoryOnly);
                    }

                    string[] sldcontent = new string[sldPaths.Length];
                    int i = 0;
                    foreach (string sldpath in sldPaths)
                    {
                        StringBuilder sb = new StringBuilder();
                        using (StreamReader sr = new StreamReader(sldpath))
                        {
                            String line;
                            while ((line = sr.ReadLine()) != null)
                            {
                                sb.AppendLine(line);
                            }
                        }
                        sldcontent[i] = sb.ToString();
                    }
                    args.Slds = sldcontent;
                    formService.OnDataChanged(args);
                }

                messageBoxService.Information(LocalizationLocator.MobileLocalization.SuccessfullExport);
            }
            catch (EmsgException ex)
            {
                Loggers.TechLogger.ErrorException(ex.Message, ex);
                messageBoxService.Information(ex.Message);
            }
            catch (Exception ex)
            {
                Loggers.TechLogger.ErrorException(ex.Message, ex);
                messageBoxService.Information(LocalizationLocator.MobileLocalization.ExportError);
            }
        }

        public void Import(string fileName)
        {
            windowService.CloseAllLegendWindows();
            //windowService.CloseLegendWindowAll();
            progressService.Start(LocalizationLocator.MobileLocalization.StartImport);
            this.unpack(fileName);
            loadService.Load();
            messageBoxService.Information(LocalizationLocator.MobileLocalization.SuccessfullImport);

        }
        private void unpack(string fileName)
        {
            EnsurePackageFolder();
            FileSystemInfo fsi = new DirectoryInfo(clientConfigurationProvider.PackageFolderPath);
            FileHelper.TryDeleteFiles(fsi);


            //ToDo: Handle better already imported case
            using (var zipFile = ZipFile.Read(fileName))
            {
                zipFile.ExtractProgress += (object sender, ExtractProgressEventArgs e) => {
                    if (e.EntriesTotal > 0)
                    {
                        progressService.Update(LocalizationLocator.MobileLocalization.UnzipPackage, (e.EntriesExtracted * 100) / e.EntriesTotal);
                    }
                };
                zipFile.ExtractAll(clientConfigurationProvider.PackageFolderPath, ExtractExistingFileAction.OverwriteSilently);
            }
            try
            {
                var descriptor = GetSeverpackageDescriptor(Directory.GetFiles(clientConfigurationProvider.PackageFolderPath, FileNameConstants.ServerPackageDescriptorFileName,
                                                      SearchOption.AllDirectories).SingleOrDefault());
                if (PackageVersioner.CheckServerPackageVersion(descriptor.Version) > 0)
                {
                    throw new InvalidPackageVersionNewException();
                }
                if (PackageVersioner.CheckServerPackageVersion(descriptor.Version) < 0)
                {
                    throw new InvalidPackageVersionNewException();
                }
            }
            catch (EmsgException)
            {
                throw;
            }
            catch
            {
                throw new InvalidOrIncompatiblePackageException();
            }
            UpdateLocalizationResources();
        }

        

        private ServerPackageDescriptor GetWorkingSeverpackageDescriptor()
        {
            return this.GetSeverpackageDescriptor(Path.Combine(clientConfigurationProvider.CurrentTemporaryFolder, FileNameConstants.ServerPackageDescriptorFileName));
        }

        private  ServerPackageDescriptor GetSeverpackageDescriptor()
        {
            return this.GetSeverpackageDescriptor(Path.Combine(clientConfigurationProvider.PackageFolderPath, FileNameConstants.ServerPackageDescriptorFileName));
        }

        public ServerPackageDescriptor GetSeverpackageDescriptor(string path)
        {
            ServerPackageDescriptor desc;
            XmlSerializer mySerializer = new XmlSerializer(typeof(ServerPackageDescriptor));

            using (System.IO.FileStream XMLFileStream = new System.IO.FileStream(path, FileMode.Open))
            {
                desc = (ServerPackageDescriptor)mySerializer.Deserialize(XMLFileStream);
            }
            return desc;
        }

        private void UpdateLocalizationResources()
        {
            languageService.ReloadLocalization();
        }

        private void DeleteRecoursive(FileSystemInfo fileSystemInfo)
        {
            if (fileSystemInfo.Exists)
            {

                fileSystemInfo.Attributes = FileAttributes.Normal;
                var directoryInfo = fileSystemInfo as DirectoryInfo;

                if (directoryInfo != null)
                {
                    foreach (var dirInfo in directoryInfo.GetFileSystemInfos())
                    {
                        DeleteRecoursive(dirInfo);
                    }
                }

                fileSystemInfo.Delete();
            }
        }

        private void RemoveReadOnlyRecoursive(FileSystemInfo fileSystemInfo)
        {
            fileSystemInfo.Attributes = FileAttributes.Normal;
            var di = fileSystemInfo as DirectoryInfo;

            if (di != null)
            {
                foreach (var dirInfo in di.GetFileSystemInfos())
                {
                    RemoveReadOnlyRecoursive(dirInfo);
                }
            }
        }



        private void MoveLocalizationResources(string applicationDirectory, string sourceDirectoryName, string targetDirectoryName)
        {
            var sourceDirectoryInfo = new DirectoryInfo(Path.Combine(clientConfigurationProvider.PackageFolderPath, sourceDirectoryName));
            var targetDirectoryInfo = new DirectoryInfo(Path.Combine(applicationDirectory, targetDirectoryName));
            CopyAll(sourceDirectoryInfo, targetDirectoryInfo);
        }

        private void MoveResource(string localizationFile)
        {
            string sourceFileName = Path.Combine(clientConfigurationProvider.PackageFolderPath, localizationFile);
            CopyResource(localizationFile);
            File.Delete(sourceFileName);
        }
        private void CopyResource(string localizationFile)
        {
            string sourceFileName = Path.Combine(clientConfigurationProvider.PackageFolderPath, localizationFile);
            if (!File.Exists(sourceFileName))
            {
                Loggers.TechLogger.Warn(string.Format("Resource file {0} not found in the package!", localizationFile));
                return;
            }

            File.Copy(sourceFileName, Path.Combine(clientConfigurationProvider.LocalizationResourceDirectoryPath, localizationFile), true);
        }

        public void CopyAll(DirectoryInfo source, DirectoryInfo target)
        {
            if (Directory.Exists(target.FullName) == false)
                Directory.CreateDirectory(target.FullName);

            foreach (FileInfo fi in source.GetFiles())
                fi.CopyTo(Path.Combine(target.ToString(), fi.Name), true);

            foreach (DirectoryInfo diSourceDir in source.GetDirectories())
            {
                DirectoryInfo nextTargetDir = target.CreateSubdirectory(diSourceDir.Name);
                CopyAll(diSourceDir, nextTargetDir);
            }
        }


        private void EnsurePackageFolder()
        {
            if (!Directory.Exists(clientConfigurationProvider.PackageFolderPath))
                Directory.CreateDirectory(clientConfigurationProvider.PackageFolderPath);
        }
        private void EnsurePackageFolder(string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }
    }
}
