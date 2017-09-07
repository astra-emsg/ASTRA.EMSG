using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Resources;

namespace ASTRA.EMSG.Localization.Utils
{
    public interface ILocalizationHandler
    {
        void GenerateResourceFile(string fileName, LocalizationValue localizationValue = LocalizationValue.Default);
        MobileLocalization ReadResourceFile(string fileName);
    }

    public class LocalizationHandler : ILocalizationHandler
    {
        public void GenerateResourceFile(string fileName, LocalizationValue localizationValue = LocalizationValue.Default)
        {
            var localizations = new MobileLocalization();

            using (var resXResourceWriter = new ResXResourceWriter(fileName))
            {
                foreach (var propertyInfo in typeof(MobileLocalization).GetProperties())
                {
                    if (propertyInfo.GetCustomAttributes(false).Any(a => a is DefaultValueAttribute))
                    {
                        object value = string.Empty;
                        if (localizationValue == LocalizationValue.Default)
                            value = ((DefaultValueAttribute)propertyInfo.GetCustomAttributes(false).Single(a => a is DefaultValueAttribute)).Value;
                        else if (localizationValue == LocalizationValue.Key)
                            value = propertyInfo.Name;

                        resXResourceWriter.AddResource(propertyInfo.Name, value);
                    }
                }

                foreach (var loc in localizations.LookupLocalizations)
                    resXResourceWriter.AddResource(loc.Key, loc.Value);
            }
        }

        public MobileLocalization ReadResourceFile(string fileName)
        {
            var localizations = new MobileLocalization();

            using (var resXResourceReader = new ResXResourceReader(fileName))
            {
                Type type = typeof(MobileLocalization);
                foreach (DictionaryEntry dictionaryEntry in resXResourceReader)
                {
                    string resourceKey = dictionaryEntry.Key.ToString();
                    var propertyInfo = type.GetProperty(resourceKey);
                    if(propertyInfo != null)
                    {
                        propertyInfo.SetValue(localizations, dictionaryEntry.Value, new object[0]);
                    }
                    else
                    {
                        if(resourceKey.StartsWith(MobileLocalization.MassnahmenvorschlagkKatalogPrefix))
                            localizations.LookupLocalizations[resourceKey] = (string) dictionaryEntry.Value;
                        else
                            Debug.WriteLine(string.Format("Unused resource key found: {0}", resourceKey));
                    }
                }
            }

            return localizations;
        }
    }
}
