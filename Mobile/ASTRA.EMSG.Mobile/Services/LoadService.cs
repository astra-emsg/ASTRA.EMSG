using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using ASTRA.EMSG.Common.Mobile.Logging;
using ASTRA.EMSG.Localization;
using ASTRA.EMSG.Common.Mobile.Enums;
using ASTRA.EMSG.Common.Mobile;
using ASTRA.EMSG.Common.DataTransferObjects;
using ASTRA.EMSG.Map.Services;
using ASTRA.EMSG.Common.Mobile.Utils;
using ASTRA.EMSG.Common;
using System.Collections.ObjectModel;
using System.Xml.Serialization;
using Microsoft.Win32;
using System.Windows;
using ASTRA.EMSG.Common.Utils;
using ASTRA.EMSG.Mobile.BusinessExceptions;
using ASTRA.EMSG.Mobile.Views.Windows;
using GeoJSON;
using NetTopologySuite.Geometries.MGeometries;
using System.Resources;
using System.Collections;
using System.Runtime.Serialization.Json;
using System.Windows.Threading;

namespace ASTRA.EMSG.Mobile.Services
{
    public interface ILoadService
    {
        event EventHandler PackageLoaded;
        event EventHandler MobileChanged;
        event EventHandler PackageUnloaded;
        ServerPackageDescriptor PackageDescriptor { get; set; }
        ObservableCollection<XMLKeyValuePair<Guid, string>> InspektionsroutenDictionary { get; set; }
        event EventHandler PackageDescriptorChanged;
        void Load(bool overwriteMobile = true, bool overwriteTiles = true);
        void Load(string path, bool overwriteMobile = true, bool overwriteTiles = true);
        void ClearAll(bool isScenarioExportAll = false);
        
    }
    public class LoadService : ILoadService
    {
        public event EventHandler PackageLoaded;
        public event EventHandler MobileChanged;
        public event EventHandler PackageUnloaded;
        private readonly IClientConfigurationProvider clientConfigurationProvider;
        private readonly IDTOService dtoService;
        private readonly IFormService formService;
        private readonly IGeoJsonService geoJsonService;
        private readonly IProgressService progressService;
        
        private readonly IMapService mapService;
        public ServerPackageDescriptor PackageDescriptor { get; set; }
        public ObservableCollection<XMLKeyValuePair<Guid, string>> InspektionsroutenDictionary { get; set; }
        public event EventHandler PackageDescriptorChanged;

        public LoadService(IDTOService dtoService, 
            IClientConfigurationProvider clientConfigurationProvider,             
            IMapService mapService, 
            IFormService formService, 
            IGeoJsonService geoJsonService,
            IProgressService progressService)
        {
            
            this.formService = formService;
            this.dtoService = dtoService;
            this.mapService = mapService;
            this.geoJsonService = geoJsonService;
            this.progressService = progressService;

            Action action = new Action(() => InspektionsroutenDictionary = new ObservableCollection<XMLKeyValuePair<Guid, string>>());
            Application.Current.Dispatcher.Invoke(action);
            this.clientConfigurationProvider = clientConfigurationProvider;

        }
        private void OnPackageLoaded()
        {
            var handler = PackageLoaded;
            if (handler != null) handler(this, new EventArgs());
        }
        private void OnMobileChanged()
        {
            var handler = MobileChanged;
            if (handler != null) handler(this, new EventArgs());
        }
        public void Load(bool copyMobile = true, bool copyMap = true)
        {
            Load(clientConfigurationProvider.PackageFolderPath, copyMobile, copyMap);
        }
        public void ClearAll(bool isScenarioExportAll = false)
        {
            mapService.OnZustandsabschnittCancelled();
            clearPackageMetaData(isScenarioExportAll);
            dtoService.Clear();
            this.OnPackageUnloaded();
        }
        private void OnPackageUnloaded()
        {
            if (PackageUnloaded != null)
                PackageUnloaded(this, new EventArgs());
        }
        public void Load(string path, bool overwriteMobile = true, bool overwriteTiles = true )
        {
            Loggers.PerformanceLogger.Debug("Loading package from {0} started.", path);
            FileHelper.TryDeleteFiles(new DirectoryInfo(clientConfigurationProvider.TemporaryFolderPath));

            ClearAll();

            DirectoryInfo pathInfo = new DirectoryInfo(path);
            IEnumerable<DirectoryInfo> directoriesToCopy = pathInfo.GetDirectories().Where(di => di.Name != FileNameConstants.TileFolderName && di.Name != FileNameConstants.MobilePackageFolderName);
            IEnumerable<FileInfo> filesToCopy = pathInfo.EnumerateFiles();
            clientConfigurationProvider.CurrentTemporaryFolder = Path.Combine(clientConfigurationProvider.TemporaryFolderPath, Guid.NewGuid().ToString());
            foreach (DirectoryInfo dirinfo in directoriesToCopy)
            {
                FileHelper.CopyAll(dirinfo, new DirectoryInfo(clientConfigurationProvider.CurrentTemporaryFolder));
            }
            
            foreach (FileInfo fileinfo in filesToCopy)
            {
                fileinfo.CopyTo(Path.Combine(clientConfigurationProvider.CurrentTemporaryFolder, fileinfo.Name), true);
            }
            string[] allfilePaths = Directory.GetFiles(clientConfigurationProvider.CurrentTemporaryFolder, "*",
                                            SearchOption.AllDirectories);
            var filePathsDTO = allfilePaths.Where(p => Path.GetExtension(p) == ".model");

            var filePathPackageDescriptor = allfilePaths.Where(p => Path.GetFileName(p) == FileNameConstants.ServerPackageDescriptorFileName).SingleOrDefault();

            PackageDescriptor = GetServerPackageDescriptor(filePathPackageDescriptor);

            Loggers.PerformanceLogger.Debug("Package consistency check started.");
            foreach (var filecount in PackageDescriptor.FileCount)
            {
                if (!((Directory.GetFiles(path, "*" + filecount.Key, SearchOption.AllDirectories).Count(s => s.EndsWith(filecount.Key))) == filecount.Value))
                {
                    throw new IncompletePackageException();
                }
            }
            Loggers.PerformanceLogger.Debug("Package consistency check finished.");

            DirectoryInfo tileDirectory = new DirectoryInfo(Path.Combine(path, FileNameConstants.TileFolderName));
            if (tileDirectory.Exists && overwriteTiles || (tileDirectory.Exists && !Directory.Exists(clientConfigurationProvider.TileFolder)))
            {
                FileHelper.CopyAll(tileDirectory, new DirectoryInfo(clientConfigurationProvider.TileFolder), (int progress, string text) => { this.progressService.Update(LocalizationLocator.MobileLocalization.CreateMapCache, progress); });
            }
            DirectoryInfo mobileDirectory = new DirectoryInfo(Path.Combine(path, FileNameConstants.MobilePackageFolderName));
            if ((mobileDirectory.Exists && overwriteMobile) || (mobileDirectory.Exists && !Directory.Exists(clientConfigurationProvider.MobilePackageFolder)))
            {
                FileHelper.CopyAll(mobileDirectory, new DirectoryInfo(clientConfigurationProvider.MobilePackageFolder));
                this.OnMobileChanged();
            }
           
            //all changes to an Observable Collection must be done by the same Thread it was created by, becaus of Thread Affinity
            Func<ObservableCollection<XMLKeyValuePair<Guid, string>>> func = new Func<ObservableCollection<XMLKeyValuePair<Guid, string>>>(PackageDescriptor.GetObservableCollection);
            InspektionsroutenDictionary = (ObservableCollection<XMLKeyValuePair<Guid, string>>)Application.Current.Dispatcher.Invoke(func);

            OnPackageDescriptorChanged(EventArgs.Empty);

            Loggers.PerformanceLogger.Debug("Loading Vector-Data started.");
            
            dtoService.LoadFile();

            //Vectorlayers
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
            PackageDescriptor.LayerConfig.ForEach(c => c.BasePath = clientConfigurationProvider.TileFolder);
            args.LayerInfo = PackageDescriptor.LayerConfig;
            args.ActiveInspectionRouteId = InspektionsroutenDictionary.First().Key;            
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
                i++;
            }
            args.Slds = sldcontent;            
            formService.OnDataChanged(args);
            Loggers.PerformanceLogger.Debug("Loading Vector-Data finished.");
            Loggers.PerformanceLogger.Debug("Package loaded");
            OnPackageLoaded();
        }



        private void OnPackageDescriptorChanged(EventArgs e)
        {
            if (PackageDescriptorChanged != null)
                PackageDescriptorChanged(this, e);
        }
        public void clearPackageMetaData(bool isScenarioExportAll)
        {
            if (!isScenarioExportAll)
            {
                clientConfigurationProvider.CurrentTemporaryFolder = null;
            }
            PackageDescriptor = null;
            //all changes to an Observable Collection must be done by the same Thread it was created by, becaus of Thread Affinity
            Action action = InspektionsroutenDictionary.Clear;
            Application.Current.Dispatcher.Invoke(action);
            OnPackageDescriptorChanged(new EventArgs());
        }
        
        private ServerPackageDescriptor GetServerPackageDescriptor(string path)
        {
            ServerPackageDescriptor desc;
            XmlSerializer mySerializer = new XmlSerializer(typeof(ServerPackageDescriptor));
            using (System.IO.FileStream XMLFileStream = new System.IO.FileStream(path, FileMode.Open))
            {
                desc = (ServerPackageDescriptor)mySerializer.Deserialize(XMLFileStream);
            }
            return desc;
        }
    }
}
