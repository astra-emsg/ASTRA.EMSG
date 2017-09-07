using System.Configuration;
using System.IO;
using System.Reflection;
using ASTRA.EMSG.Common;
using ASTRA.EMSG.Common.ConfigurationHandling;
using ASTRA.EMSG.Localization.Utils;
using System;

namespace ASTRA.EMSG.Mobile.Services
{
    public interface IClientConfigurationProvider
    {
        string LocalizationResourceFileName { get; }
        string LocalizationResourceDirectoryPath { get; }
        string LocalizationResourceFilePath { get; }
        string Language { get; }
        string PackageFolderPath { get; }
        string TemporaryFolderPath { get; }
        string CurrentTemporaryFolder { get; set; }
        string HelpDirectoryPath { get; }
        string ExportPackageFolderPath { get; }
        string MobilePackageFolder { get; }
        string TileFolder { get; }
        bool UseLocalFiles { get; }
        Configuration Config { get; }

    }

    public class ClientConfigurationProvider : IClientConfigurationProvider
    {
        public string LocalizationResourceFileName { get { return ReadStringValue("LocalizationResourceFileName", FileNameConstants.DefaultMobileLocalizationFileName); } }
        public string LocalizationResourceDirectoryPath { get { return ReadStringValue("LocalizationResourceDirectoryPath", "Resources"); } }
        public string HelpDirectoryPath { get { return ReadStringValue("HelpDirectoryPath", "Help"); } }

        public string LocalizationResourceFilePath { get { return Path.Combine(LocalizationResourceDirectoryPath, LocalizationResourceFileName); } }
        public string Language { get { return ReadStringValue("Language", "de"); } }
        public bool UseLocalFiles { get { return ReadBoolValue("UseLocalFiles", false); } }
        public string PackageFolderPath
        {
            get
            {
                string localAppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                return Path.Combine(localAppDataPath, "ASTRA.EMSG", "Package");
            }
        }
        public string TemporaryFolderPath
        {
            get
            {
                string localAppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                return Path.Combine(localAppDataPath, "ASTRA.EMSG", "Temp");
            }
        }
        public string ExportPackageFolderPath
        {
            get
            {
                string localAppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                return Path.Combine(localAppDataPath, "ASTRA.EMSG", "Export");
            }
        }
        public string MobilePackageFolder
        {
            get
            {
                return Path.Combine(Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData), "ASTRA.EMSG", FileNameConstants.MobilePackageFolderName);
            }
        }
        public string TileFolder
        {
            get
            {
                return Path.Combine(Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData), "ASTRA.EMSG", FileNameConstants.TileFolderName);
            }
        }
        private string ReadStringValue(string settingName, string defaultValue)
        {
            var value = Config.AppSettings.Settings[settingName].Value;
            if (string.IsNullOrEmpty(value))
                value = defaultValue;

            return value;
        }
        private bool ReadBoolValue(string settingName, bool defaultValue)
        {
            var value = Config.AppSettings.Settings[settingName].Value;
            bool result = false;
            if (!Boolean.TryParse(value, out result))
                result = defaultValue;

            return result;
        }

        private Lazy<Configuration> config = new Lazy<Configuration>(() => ConfigurationManager.OpenExeConfiguration(Assembly.GetExecutingAssembly().Location));

        public Configuration Config { get { return config.Value; } }

        public string CurrentTemporaryFolder { get; set; }
        
    }
}
