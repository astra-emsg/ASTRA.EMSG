using System;
using System.ComponentModel;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using ASTRA.EMSG.Mobile.Installer.Packaging;
using ASTRA.EMSG.Mobile.Installer.Utils;
using IWshRuntimeLibrary;
using File = System.IO.File;
using Path = System.IO.Path;

namespace ASTRA.EMSG.Mobile.Installer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private CultureInfo currentLanguage = new CultureInfo("de");

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string userDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

                string packageResourceName = "ASTRA.EMSG.Mobile.Installer.InstallerPackage.Application.pkg";
                string packageName = "Application.pkg";
                string applicationDirectoryName = "EMSG.Mobile";
                string applicationName = "ASTRA.EMSG.Mobile.exe";

                string installDirectory = System.IO.Path.Combine(userDirectory, applicationDirectoryName);
                if (!Directory.Exists(installDirectory))
                    Directory.CreateDirectory(installDirectory);

                string packageFilePath = Path.Combine(installDirectory, packageName);

                string applicationPath = Path.Combine(installDirectory, applicationName);

                using (Stream resourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(packageResourceName))
                {
                    using (var fw = new FileStream(packageFilePath, FileMode.Create))
                    {
                        byte[] bytes = ReadAllByte(resourceStream);
                        fw.Write(bytes, 0, bytes.Length);
                    }
                }

                var packaging = new Packaging.Packaging();

                var worker = new BackgroundWorker();
                worker.DoWork +=
                    (s, a) =>
                    {
                        Dispatcher.Invoke(new Action(() =>
                        {
                            InstallStep.Visibility = Visibility.Collapsed;
                            ProgressStep.Visibility = Visibility.Visible;
                        }));
                        ReturnResult returnResult = packaging.UncompressFile(installDirectory, packageFilePath, true);
                        if (returnResult.Success)
                        {
                            CreateIcon("EMSG Mobile", applicationPath);
                            SetLanguageInConfig(applicationPath);
                        }
                    };
                worker.RunWorkerCompleted += (s, a) =>
                {
                    Dispatcher.Invoke(new Action(() =>
                    {
                        ProgressStep.Visibility =
                            Visibility.Collapsed;
                        SuccessStep.Visibility =
                            Visibility.Visible;
                    }));
                };
                worker.RunWorkerAsync();
            }
            catch (Exception ex)
            {
                using (var file = File.CreateText("errorlog.txt"))
                {
                    file.WriteLine(string.Format("{0} - {1}", DateTime.Now, ex));
                }
            }
            
        }

        public static byte[] ReadAllByte(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

        public void SetLanguageInConfig(string exePath)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(exePath);
            string language = currentLanguage.TwoLetterISOLanguageName.ToLower();
            string loc = "MobileLocalization.resx";

            if (new[] { "de", "it", "fr" }.Contains(language))
                loc = string.Format("MobileLocalization.{0}.resx", language);

            config.AppSettings.Settings["LocalizationResourceFileName"].Value = loc;
            config.AppSettings.Settings["Language"].Value = language;
            config.Save();
        }

        public void CreateIcon(string applicationName, string appPath)
        {
            try
            {
                object shDesktop = (object)"Desktop";
                WshShell shell = new WshShell();
                string shortcutAddress = (string)shell.SpecialFolders.Item(ref shDesktop) + "\\" + applicationName + ".lnk";
                IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutAddress);
                shortcut.IconLocation = appPath;
                shortcut.Description = "EMSG Mobile";
                shortcut.WorkingDirectory = Path.GetDirectoryName(appPath);
                shortcut.TargetPath = appPath;
                shortcut.Save();
            }
            catch (Exception)
            {
                //ToDo: Notify user?
            }
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            var language = (string)((RadioButton) sender).Tag;
            var languageSelector = ((LanguageSelector) Application.Current.Resources["Resx"]);
            switch (language)
            {
                case "German":
                    currentLanguage = new CultureInfo("de");
                    break;
                case "French":
                    currentLanguage = new CultureInfo("fr");
                    break;
                case "Italian":
                    currentLanguage = new CultureInfo("it");
                    break;
            }
            languageSelector.SetLanguage(currentLanguage);
        }

        private void CloseButtonClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
