using System.Windows.Input;
using ASTRA.EMSG.Mobile.Services;
using ASTRA.EMSG.Mobile.Utils;
using System.Windows.Forms.Integration;

using System.Collections.Generic;
using System;
using System.Linq;
using System.Windows.Controls;
using System.ComponentModel;
using System.Collections.ObjectModel;
using ASTRA.EMSG.Common;
using HtmlAgilityPack;
using ASTRA.EMSG.Mobile.Model;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Data;
using ASTRA.EMSG.Localization;
using ASTRA.EMSG.Common.Mobile;
using System.Threading;
using ASTRA.EMSG.Common.Mobile.Utils;
using ASTRA.EMSG.Common.Utils;
using System.Windows.Controls.Primitives;
using ASTRA.EMSG.Mobile.Views.Windows;
using ASTRA.EMSG.Map;
using ASTRA.EMSG.Map.Services;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Windows;

namespace ASTRA.EMSG.Mobile.ViewModels
{
    public interface IMapViewModel : IViewModel, INotifyPropertyChanged
    {
        CollectionView InspektionsroutenDictionary { get; set; }
        int InspektionsRoutenSwitchSelectionIndex { get; set; }
        void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e);
        void InspektionsRoutenSwitch_SelectionChanged(object sender, SelectionChangedEventArgs e);
    }

    public class MapViewModel : ViewModel, IMapViewModel
    {        
        public CollectionView InspektionsroutenDictionary { get; set; }
        public Object HtmlSource { get; set; }
        public Object ScriptingObject { get; set; }
        public Object JSEventContainer { get; set; }

        private int inspektionsRoutenSwitchSelectionIndex;
        private readonly IFormService formService;
        private readonly ILoadService loadService;
        private readonly IClientConfigurationProvider clientConfigurationProvider;
        private readonly IWindowService windowService;

        
        public int InspektionsRoutenSwitchSelectionIndex
        {
            get
            { return inspektionsRoutenSwitchSelectionIndex; }
            set
            {
                inspektionsRoutenSwitchSelectionIndex = value;
                Notify(ExpressionHelper.GetPropertyName(() => InspektionsRoutenSwitchSelectionIndex));
            }
        }
        public MapViewModel(IWindowService windowService,
            IMapService mapService, IPackageService packageService, IImportService importService, ILoadService loadService,
            IFormService formService, IClientConfigurationProvider clientConfigurationProvider)
        {

            this.loadService = loadService;
            this.formService = formService;
            this.clientConfigurationProvider = clientConfigurationProvider;
            this.windowService = windowService;
            this.JSEventContainer = formService;
            this.ScriptingObject = mapService;
            
            loadService.PackageLoaded += LoadServiceOnPackageLoaded;
            loadService.PackageUnloaded += LoadServiceOnPackageUnLoaded;
            loadService.PackageDescriptorChanged += new EventHandler(packageService_PackageDescriptorChanged);
            loadService.MobileChanged += loadServiceMobileChanged;

            OpenHelpWindowCommand = new DelegateCommand(() => windowService.OpenHelpWindow(HelpFileNames.ZustandsabschnittMapHelpPage));
            //OpenLegendWindowAllCommand = new DelegateCommand(() => windowService.OpenLegendWindowAll());
            //all changes to an Observable Collection must be done by the same Thread it was created by, becaus of Thread Affinity
            Action action = new Action(() => { InspektionsroutenDictionary = new CollectionView(new ObservableCollection<XMLKeyValuePair<Guid, string>>()); });
            Application.Current.Dispatcher.Invoke(action);
            this.IsVisible = false;
            this.setDefaultHtmlSource();

            mapService.ShowLegend += onOpenLegendWindow;
        }
        private void onOpenLegendWindow(object sender, ShowLegendEventArgs args){
            string filename;
            if (this.LegendFiles.TryGetValue(args.Layer, out filename))
            {
                windowService.OpenLegendWindow(new LegendViewModel(Path.Combine(clientConfigurationProvider.CurrentTemporaryFolder, filename), args.Layer));
            }
        }
        private readonly Dictionary<string, string> LegendFiles = new Dictionary<string, string>()
       {
            {"MV_ACHSENSEGMENT",FileNameConstants.AchsenSegmentLayerLegendFilename },
            {"STRASSENABSCHNITTGIS",FileNameConstants.StrassenabschnittLayerLegendFilename },
            {"ZUSTANDSABSCHNITTGIS",FileNameConstants.ZustandsabschnittLayerLegendFilename },
            {"ZUSTANDTROTTOIRLEFT",FileNameConstants.ZustandsabschnittLayerTrottoirLegendFilename }
        };

        public void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                loadService.Load(false, false);
            }
            catch
            {
                loadService.ClearAll();
            }
        }

        public void InspektionsRoutenSwitch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selection = ((ComboBox)sender).SelectedItem;
            if (selection != null)
            {
                this.formService.OnInspektionsRouteChanged(new InspektionsRouteChangedEventArgs(((XMLKeyValuePair<Guid, string>)selection).Key));
            }
            else
            {
                this.formService.OnInspektionsRouteChanged(new InspektionsRouteChangedEventArgs(Guid.Empty));
            }
        }

        public ICommand OpenHelpWindowCommand { get; set; }
        public ICommand OpenLegendWindowAllCommand { get; set; }
        public ICommand SwitchLegendVisibilityCommand { get; set; }
        private void packageService_PackageDescriptorChanged(object o, EventArgs e)
        {
            //all changes to an Observable Collection must be done by the same Thread it was created by, becaus of Thread Affinity
            Action action = new Action(() => { InspektionsroutenDictionary = new CollectionView(loadService.InspektionsroutenDictionary); });
            //InspektionsroutenDictionary = new CollectionView(loadService.InspektionsroutenDictionary);
            Application.Current.Dispatcher.Invoke(action);
            OnPropertyChanged(ExpressionHelper.GetPropertyName(() => InspektionsroutenDictionary));
        }
        private void loadServiceMobileChanged(object o, EventArgs e)
        {
            this.setDefaultHtmlSource();
        }
        private void setDefaultHtmlSource()
        {
            string indexPath = null;
            if(Directory.Exists(clientConfigurationProvider.MobilePackageFolder)){
                indexPath = Directory.GetFiles(clientConfigurationProvider.MobilePackageFolder, "index.*", SearchOption.TopDirectoryOnly).SingleOrDefault();
            }
            if (indexPath != null)
            {
                if (clientConfigurationProvider.UseLocalFiles)
                {
                    this.HtmlSource = new DirectoryInfo(Directory.GetFiles(".\\Mobile", "index.*", SearchOption.TopDirectoryOnly).Single()).FullName;
                }
                else
                {
                    this.HtmlSource = new DirectoryInfo(indexPath).FullName;
                }
                this.OnPropertyChanged(ExpressionHelper.GetPropertyName(() => this.HtmlSource));
            }
        }
        private void LoadServiceOnPackageUnLoaded(object sender, EventArgs e)
        {
            IsVisible = false;
            this.HtmlSource = "about:blank";
        }
        private void LoadServiceOnPackageLoaded(object sender, EventArgs e)
        {
            IsVisible = true;
        }
    }
}
