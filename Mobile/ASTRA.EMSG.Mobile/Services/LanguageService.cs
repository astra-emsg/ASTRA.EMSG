using System;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Markup;
using ASTRA.EMSG.Common;
using ASTRA.EMSG.Common.Mobile.Logging;
using ASTRA.EMSG.Localization;
using ASTRA.EMSG.Localization.Utils;
using ASTRA.EMSG.Mobile.Container;
using Autofac;

namespace ASTRA.EMSG.Mobile.Services
{
    public interface ILanguageService
    {
        void ReloadLocalization();
    }

    public class LanguageService : ILanguageService
    {


        private bool isLoaded;

        public void ReloadLocalization()
        {
            var localizationHandler = ClientContainerLocator.Container.Resolve<ILocalizationHandler>();
            var clientConfigurationProvider = ClientContainerLocator.Container.Resolve<IClientConfigurationProvider>();
            var pacakgeService = ClientContainerLocator.Container.Resolve<IPackageService>();


            string loc = clientConfigurationProvider.LocalizationResourceFileName;

            var serverPackage = Path.Combine(clientConfigurationProvider.PackageFolderPath, FileNameConstants.ServerPackageDescriptorFileName);
            string language = "de";
            if (File.Exists(serverPackage))
            {
                var serverPackageDescriptor = pacakgeService.GetSeverpackageDescriptor(serverPackage);

                string newLanguage = serverPackageDescriptor.CurrentCulture.Substring(0, 2);
                string lng = newLanguage.ToLower();

                if (new[] {"de", "it", "fr"}.Contains(lng))
                {
                    loc = string.Format("{0}.{1}.resx", FileNameConstants.MobileLocalizationFileNameWithoutExtension,lng);
                    language = lng;
                }
            }

            string localizationResourceFileName = Path.Combine(clientConfigurationProvider.PackageFolderPath,
                                                               clientConfigurationProvider.LocalizationResourceDirectoryPath,
                                                               loc);
                
            if (!File.Exists(localizationResourceFileName))   
                localizationResourceFileName = clientConfigurationProvider.LocalizationResourceFilePath;

            if (File.Exists(localizationResourceFileName))
                LocalizationLocator.SetMobileLocalization(localizationHandler.ReadResourceFile(localizationResourceFileName));
            else
                Loggers.TechLogger.Warn(string.Format("Localization Resource file not found! ({0})", localizationResourceFileName));

            CultureInfo currentCulture = CultureInfo.CreateSpecificCulture("de-CH");
            if (language != "de")
            {
                var newCulture = CultureInfo.CreateSpecificCulture(language + "-CH");
                newCulture.NumberFormat = currentCulture.NumberFormat;
                currentCulture = newCulture;
            }
            currentCulture.NumberFormat.NumberDecimalSeparator = ".";
            currentCulture.NumberFormat.NumberGroupSeparator = "'";
            Thread.CurrentThread.CurrentCulture = currentCulture;
            Thread.CurrentThread.CurrentUICulture = currentCulture;

            if(!isLoaded)
            {
                isLoaded = true;
                FrameworkElement.LanguageProperty.OverrideMetadata(typeof(FrameworkElement), new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));
            }
            ((ResourceWrapper)Application.Current.Resources["Resx"]).Refresh();
        }
    }
}