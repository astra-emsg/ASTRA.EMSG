using System;
using ASTRA.EMSG.Common.Mobile.Logging;
using ASTRA.EMSG.Localization;
using Microsoft.Win32;
using ASTRA.EMSG.Mobile.BusinessExceptions;
using System.Windows.Input;
using ASTRA.EMSG.Mobile.Views.Windows;
using System.IO;


namespace ASTRA.EMSG.Mobile.Services
{
    public interface IFileDialogService
    {
        bool ShowImportDialog(Action<string> openAction);
        void ShowExportDialog(Action<string, bool> saveAction, bool exportAll);
        void ShowSaveDialog(Action<string> saveAction);
        void ShowExportLogDialog(Action<string> saveAction);
        bool PackageLoaded { get; }
        event EventHandler ImportStart;
        event EventHandler ImportFinished;
    }

    public class FileDialogService : IFileDialogService
    {
        private readonly IMessageBoxService messageBoxService;
        private readonly ILoadService loadService;
        private bool packageLoaded;
        public bool PackageLoaded { get { return packageLoaded; } }
        public event EventHandler ImportStart;
        public event EventHandler ImportFinished;
        public FileDialogService(IMessageBoxService messageBoxService, ILoadService loadService)
        {
            this.messageBoxService = messageBoxService;
            this.loadService = loadService;
            loadService.PackageLoaded += PackageServiceOnPackageLoaded;
            loadService.PackageUnloaded += PackageServiceOnPackageUnLoaded;
        }
        private void OnImportStart(EventArgs e)
        {
            if (ImportStart != null)
                ImportStart(this, e);
        }
        private void OnImportFinished(EventArgs e)
        {
            if (ImportFinished != null)
                ImportFinished(this, e);
        }
        public bool ShowImportDialog(Action<string> openAction)
        {
            if (packageLoaded)
            {
                System.Windows.Forms.DialogResult dialogResult = System.Windows.Forms.MessageBox.Show(LocalizationLocator.MobileLocalization.ImportWarning, "", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Warning);

                if (dialogResult != System.Windows.Forms.DialogResult.Yes)
                {
                    return false;
                }
            }
            

            var openFileDialog = new OpenFileDialog
                                     {
                                         CheckFileExists = true,
                                         CheckPathExists = true,
                                         Filter = "EMSGE (*.emsge)|*.emsge",
                                         InitialDirectory = Environment.CurrentDirectory,
                                         Multiselect = false,
                                         Title = LocalizationLocator.MobileLocalization.Import
                                     };
            string importPath = Properties.Settings.Default.ImportFolderPath;
            if(!string.IsNullOrEmpty(importPath) && Directory.Exists(importPath)){
                openFileDialog.InitialDirectory = importPath;
            }
            if ((bool)openFileDialog.ShowDialog())
            {
                FileInfo file = new FileInfo(openFileDialog.FileName);
                Properties.Settings.Default.ImportFolderPath = file.Directory.FullName;
                Properties.Settings.Default.Save();

                OnImportStart(new EventArgs());
                //Mouse.OverrideCursor = Cursors.Wait;
                Loggers.TechLogger.Info("Importing File: " + openFileDialog.FileName);
                try
                {
                    openAction(openFileDialog.FileName);
                }
                catch (EmsgException ex)
                {
                   
                    OnImportFinished(new EventArgs());
                    //Mouse.OverrideCursor = null;
                    Loggers.TechLogger.ErrorException(ex.Message, ex);
                    loadService.ClearAll();
                    messageBoxService.Information(ex.Message);
                    return false;
                }
                catch (Exception ex)
                {
                    
                    //Mouse.OverrideCursor = null;
                    OnImportFinished(new EventArgs());
                    Loggers.TechLogger.ErrorException(ex.Message, ex);
                    loadService.ClearAll();
                    messageBoxService.Information(LocalizationLocator.MobileLocalization.ImportError);
                    return false;
                }
                
                //Mouse.OverrideCursor = null;
                OnImportFinished(new EventArgs());
                return true;
            }
            else
            {
                return false;
            }

        }

        public void ShowExportDialog(Action<string, bool> saveAction, bool exportAll)
        {
            var saveFileDialog = new SaveFileDialog
                                     {
                                         AddExtension = true,
                                         CheckPathExists = true,
                                         FileName = "Route",
                                         Filter = "EMSGI (*.emsgi)|*.emsgi",
                                         InitialDirectory = Environment.CurrentDirectory,
                                         Title = LocalizationLocator.MobileLocalization.Export
                                     };

            string exportPath = Properties.Settings.Default.ExportFolderPath;
            if (!string.IsNullOrEmpty(exportPath) && Directory.Exists(exportPath))
            {
                saveFileDialog.InitialDirectory = exportPath;
            }

            if ((bool)saveFileDialog.ShowDialog())
            {
                FileInfo file = new FileInfo(saveFileDialog.FileName);
                Properties.Settings.Default.ExportFolderPath = file.Directory.FullName;
                Properties.Settings.Default.Save();

                Loggers.TechLogger.Info("Exporting to File: " + saveFileDialog.FileName);
                try
                {
                    saveAction(saveFileDialog.FileName, exportAll);
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
        }
        public void ShowSaveDialog(Action<string> saveAction)
        {
            var saveFileDialog = new SaveFileDialog
            {
                AddExtension = true,
                CheckPathExists = true,
                Filter = "EMSGE (*.emsge)|*.emsge",
                InitialDirectory = Environment.CurrentDirectory,
                Title = LocalizationLocator.MobileLocalization.Export
            };

            if ((bool)saveFileDialog.ShowDialog())
            {
                Loggers.TechLogger.Info("Saving to File: " + saveFileDialog.FileName);
                try
                {
                    saveAction(saveFileDialog.FileName);
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
        }
        public void ShowExportLogDialog(Action<string> saveAction)
        {
            
            var saveFileDialog = new SaveFileDialog
            {
                AddExtension = true,
                CheckPathExists = true,
                Filter = "ZIP (*.zip)|*.zip",
                InitialDirectory = Environment.CurrentDirectory,
                Title = LocalizationLocator.MobileLocalization.Export
            };

            

            if ((bool)saveFileDialog.ShowDialog())
            {
                Loggers.TechLogger.Info("Exporting Log to File: " + saveFileDialog.FileName);
                try
                {
                    saveAction(saveFileDialog.FileName);
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
        }
        private void PackageServiceOnPackageLoaded(object sender, EventArgs e)
        {
            packageLoaded = true;
        }

        private void PackageServiceOnPackageUnLoaded(object sender, EventArgs e)
        {
            packageLoaded = false;
        }
    }
}
