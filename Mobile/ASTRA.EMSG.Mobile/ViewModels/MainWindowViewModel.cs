using System.Windows.Input;
using ASTRA.EMSG.Common;
using ASTRA.EMSG.Localization;
using ASTRA.EMSG.Mobile.Services;
using ASTRA.EMSG.Mobile.Utils;
using ASTRA.EMSG.Map.Services;
using System;
using System.Diagnostics;
using System.ComponentModel;
using ASTRA.EMSG.Common.Mobile.Logging;
using ASTRA.EMSG.Mobile.Events;

namespace ASTRA.EMSG.Mobile.ViewModels
{
    public interface IMainWindowViewModel : IViewModel
    {
        
        IFormViewModel FormViewModel { get; set; }
        IMapViewModel MapViewModel { get; set; }
        IProgressViewModel ProgressViewModel { get; set; }
        ICommand ShowHideKarteCommand { get; }
        ICommand ShowHideFormularCommand { get; }

        ICommand ExitCommand { get; }
        ICommand ExportCommand { get; }
        ICommand ExportAllCommand { get; }
        ICommand ImportCommand { get; }
        ICommand SaveCommand { get; }
        ICommand LoadCommand { get; }
        void OnWindowClosing(object sender, CancelEventArgs e);
        bool IsEnabled { get; set; }
    }

    public class MainWindowViewModel : ViewModel, IMainWindowViewModel
    {
        public IFormViewModel FormViewModel { get; set; }
        public IMapViewModel MapViewModel { get; set; }
        public IProgressViewModel ProgressViewModel { get; set; }
        private bool isEnabled = true;
        
        private readonly IImportService importservice;
        private readonly IProgressService progressService;
        
        public bool IsEnabled
        {
            get { return isEnabled; }
            set
            {
                if (!(value == isEnabled))
                {
                    isEnabled = value;
                    Notify(() => IsEnabled);
                }
            }
        }

        public MainWindowViewModel(IFormViewModel formViewModel, IMapViewModel mapViewModel, IProgressViewModel progressViewModel,
            IFileDialogService fileDialogService, IPackageService packageService, 
            IImportService importservice, ISaveService saveService, ILoadService 
            loadService, ILogService logService, IWindowService windowService,
            IProgressService progressService )
        {
            Loggers.PerformanceLogger.Debug("MainWindowViewModel constructor started");

            this.loadService = loadService;
            loadService.PackageLoaded += PackageServiceOnPackageLoaded;
            loadService.PackageUnloaded += OnPackageUnloaded;
            this.saveService = saveService;
           // this.gisService = gisService;
            this.windowService = windowService;
            this.fileDialogService = fileDialogService;
            this.packageService = packageService;
            this.importservice = importservice;
            this.progressService = progressService;
            MapViewModel = mapViewModel;
            FormViewModel = formViewModel;
            ProgressViewModel = progressViewModel;

            ShowHideKarteCommand = new DelegateCommand(() => MapViewModel.IsVisible = !MapViewModel.IsVisible);
            ShowHideFormularCommand = new DelegateCommand(() => FormViewModel.IsVisible = !FormViewModel.IsVisible);

            ExitCommand = new DelegateCommand(App.Current.Shutdown);
            ImportCommand = new DelegateCommand(startImportWorker);
            SaveCommand = new DelegateCommand(() => fileDialogService.ShowSaveDialog(saveService.Save));
            ExportCommand = new DelegateCommand(exportSingle);
            ExportAllCommand = new DelegateCommand(() => fileDialogService.ShowExportDialog(packageService.Export, true));
            ExportLogCommand = new DelegateCommand(() => fileDialogService.ShowExportLogDialog(logService.ExportLog));
            WindowTitle = string.Format("{0} v{1}-{2}", LocalizationLocator.MobileLocalization.MainWindowTitle, typeof (MainWindow).Assembly.GetName().Version, VersionPostfix.Postfix);
            MenuItemsVisible = false;
            isEnabled = true;
            fileDialogService.ImportStart += new EventHandler(fileDialogServiceImportStart);
            fileDialogService.ImportFinished += new EventHandler(fileDialogServiceImportFinished);
            progressService.OnStart += progressOnStart;
            progressService.OnStop += progressOnStop;

            Loggers.PerformanceLogger.Debug("MainWindowViewModel constructor ended");
        }
        private void startImportWorker()
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += doImport;
            worker.ProgressChanged += workerProgressChanged;
            worker.RunWorkerCompleted += workerRunWorkerCompleted;           
            worker.RunWorkerAsync();
            
        }

        void workerRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            progressService.Update(String.Empty, 100);
            progressService.Stop();
        }

        void workerProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressService.Update(e.UserState as string, e.ProgressPercentage);
        }
        private void doImport(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            Action<int, string> progressCall = worker.ReportProgress;
            importservice.Import();

        }
        private void progressOnStart(object sender, ProgressStartEventArgs e)
        {
            IsEnabled = false;
            MapViewModel.IsVisible = false;
        }
        private void progressOnStop(object sender, EventArgs e)
        {
            IsEnabled = true;
            MapViewModel.IsVisible = fileDialogService.PackageLoaded;
        }

        void fileDialogServiceImportFinished(object sender, EventArgs e)
        {
            IsEnabled = true;            
        }

        void fileDialogServiceImportStart(object sender, EventArgs e)
        {
           IsEnabled = false;           
        }
        public void OnWindowClosing(object sender, CancelEventArgs e)
        {
            windowService.CloseAllLegendWindows();
            //windowService.CloseLegendWindowAll();
            try{
                saveService.SaveCurrentPackage();
            }catch(Exception ex){
                Loggers.TechLogger.ErrorException("Could not autosave active Package", ex);
            }
            //gisService.SetApplicationExiting();
        }

        private void PackageServiceOnPackageLoaded(object sender, EventArgs e)
        {
            MenuItemsVisible = true;

            Notify(() => MenuItemsVisible);
        }

        private void OnPackageUnloaded(object sender, EventArgs e)
        {
            MenuItemsVisible = false;

            Notify(() => MenuItemsVisible);
        }

        private void exportSingle()
        {
            fileDialogService.ShowExportDialog(packageService.Export, false);
            if (MapViewModel.InspektionsroutenDictionary.Count > 0)
            {
                MapViewModel.InspektionsRoutenSwitchSelectionIndex =0;
            }
        }
        private readonly IFileDialogService fileDialogService;
        private readonly IPackageService packageService;
        private readonly ILoadService loadService;
        
        private readonly ISaveService saveService;
        //private readonly IGISService gisService;
        private readonly IWindowService windowService;

        public string WindowTitle { get; set; }

        public bool MenuItemsVisible { get; private set; }

        public ICommand ShowHideKarteCommand { get; private set; }
        public ICommand ShowHideFormularCommand { get; private set; }
        public ICommand ExportLogCommand { get; private set; }
        public ICommand ExitCommand { get; private set; }
        public ICommand ExportCommand { get; private set; }
        public ICommand ExportAllCommand { get; private set; }
        public ICommand ImportCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }
        public ICommand LoadCommand { get; private set; }
    }
}
