using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Configuration.Install;
using System.Linq;
using System.Windows;
using System.IO;
using System.Resources;


namespace ASTRA.EMSG.Mobile
{
    [RunInstaller(true)]
    public partial class InstallerStep : System.Configuration.Install.Installer
    {
        public InstallerStep()
        {
            InitializeComponent();
        }

        public override void Install(IDictionary stateSaver)
        {
            base.Install(stateSaver);
            string targetDirectory = Context.Parameters["targetdir"];

            string param1 = Context.Parameters["language"];

            string exePath = string.Format("{0}ASTRA.EMSG.Mobile.exe", targetDirectory);

            Configuration config = ConfigurationManager.OpenExeConfiguration(exePath);

            string language = "de";
            switch (param1)
            {
                case "1":
                    language = "de";
                    break;
                case "2":
                    language = "fr";
                    break;
                case "3":
                    language = "it";
                    break;
            }
            string loc = "MobileLocalization.resx";

            if (new[] { "de", "it", "fr" }.Contains(language))
                loc = string.Format("MobileLocalization.{0}.resx", language);

            config.AppSettings.Settings["LocalizationResourceFileName"].Value = loc;
            config.AppSettings.Settings["Language"].Value = language;
            config.Save();
        }
        public override void Uninstall(IDictionary savedState)
        {
            
            try
            {
                string message = "Do you want to delete local data?";
                try
                {
                    string exePath = Context.Parameters["assemblypath"];
                    Configuration config = ConfigurationManager.OpenExeConfiguration(exePath);
                    string filename = config.AppSettings.Settings["LocalizationResourceFileName"].Value;
                    FileInfo info = new FileInfo(exePath);
                    string filepath = Path.Combine(info.Directory.FullName, "Resources", filename);
                    ResXResourceSet resxSet = new ResXResourceSet(filepath);
                    string locMessage = resxSet.GetString("DeleteLocalData");
                    if (!string.IsNullOrEmpty(locMessage))
                    {
                        message = locMessage;
                    }
                }
                catch 
                {}
                if (MessageBox.Show(message, "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    deleteLocalContent();
                }
            }catch(Exception e)
            {
                MessageBox.Show(e.StackTrace, e.Message);
            }
            



            base.Uninstall(savedState);
        }
        private void deleteLocalContent()
        {
            string appdata =Path.Combine(Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData), "ASTRA.EMSG");
            if (Directory.Exists(appdata))
            {
                DirectoryInfo info = new DirectoryInfo(appdata);
                info.Attributes = FileAttributes.Normal;
                foreach (var file in info.GetFiles("*", SearchOption.AllDirectories))
                    file.Attributes = FileAttributes.Normal;
                info.Delete(true);
            }

            string myDocumentsFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string mydocs = Path.Combine(myDocumentsFolderPath, "Emsg");

            if (Directory.Exists(mydocs))
            {
                DirectoryInfo info = new DirectoryInfo(mydocs);
                info.Attributes = FileAttributes.Normal;
                foreach (var file in info.GetFiles("*", SearchOption.AllDirectories))
                    file.Attributes = FileAttributes.Normal;
                info.Delete(true);
            }
        }
        public override void Commit(IDictionary savedState)
        {
            base.Commit(savedState);
        }

        public override void Rollback(IDictionary savedState)
        {
            base.Rollback(savedState);
        }
    }
}
