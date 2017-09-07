using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Resources;
using System.Text;
using ASTRA.EMSG.Localization;
using Moq;

namespace ASTRA.EMSG.Tests.ResourceTests
{
    public class ResourceComparingService
    {
        private Dictionary<string, string> defaultResourceEntries;
        private Dictionary<string, Dictionary<string, string>> resourceEntries; 
        private Dictionary<string, ResourceKeyComparisonContainer> resourceKeyDifferences;
        private Dictionary<string, ResourceValueComparisonContainer> resourceValueDifferences;

        private readonly List<string> languagesAbbreviations = new List<string>() {"de", "fr", "it"};
        private readonly string resourceFileName;
        private readonly string resourcePath = "ResourceTests/Resources/";
        private string comparedLanguage;

        public ResourceComparingService(string resourceFileName, bool forMobile = false)
        {
            this.resourceFileName = resourceFileName;
            if (forMobile)
                resourcePath = "ResourceTests/MobileResources/";

            InitializeDictionaries();
            ReadAllEntries(forMobile);
        }

        private void InitializeDictionaries()
        {
            defaultResourceEntries = new Dictionary<string, string>();
            resourceEntries = new Dictionary<string, Dictionary<string, string>>();
            resourceKeyDifferences = new Dictionary<string, ResourceKeyComparisonContainer>();
            foreach (var lang in languagesAbbreviations)
            {
                resourceEntries[lang] = new Dictionary<string, string>();
                resourceKeyDifferences[lang] = new ResourceKeyComparisonContainer();
            }
        }

        private void ReadAllEntries(bool forMobile)
        {
            if (forMobile)
                ReadMobileDefaultEntries();
            else
                ReadEntries();

            foreach (var lang in languagesAbbreviations)
                ReadEntries(lang);
        }

        private void ReadMobileDefaultEntries()
        {
            var localization = new MobileLocalization();
            var type = typeof(MobileLocalization);
            var keys = type.GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(DefaultValueAttribute))); ;
            foreach (var key in keys)
            {
                defaultResourceEntries[key.Name] = key.GetValue(localization, new object[0]).ToString();
            }

            var lookupLocalizations = localization.LookupLocalizations;
            foreach (var item in lookupLocalizations)
                defaultResourceEntries[item.Key] = item.Value;
        }

        private void ReadEntries(string language = "")
        {
            bool defaultResource = language == "";

            var langSuffix = !defaultResource ? string.Format(".{0}", language) : "";

            var filePath = string.Format("{0}{1}{2}.resx", resourcePath, resourceFileName, langSuffix);

            var reader = new ResXResourceReader(filePath);
            var dict = reader.GetEnumerator();

            while (dict.MoveNext())
                if (defaultResource)
                    defaultResourceEntries[dict.Key.ToString()] = dict.Value.ToString();
                else
                    resourceEntries[language][dict.Key.ToString()] = dict.Value.ToString();
        }

        private void FindMissingKeys(string language = "")
        {
            var languages = language == "" ? languagesAbbreviations : new List<string>() { language };
            foreach (var key in defaultResourceEntries.Keys)
                foreach (var lang in languages.Where(lang => !resourceEntries[lang].Keys.Contains((key))))
                    resourceKeyDifferences[lang].MissingKeys.Add(key);
        }

        private void FindUnnecessaryKeys(string language = "")
        {
            var languages = language == "" ? languagesAbbreviations : new List<string>() { language };
            foreach (var lang in languages)
            {
                if (resourceEntries[lang].Keys.Count + resourceKeyDifferences[lang].MissingKeys.Count !=
                    defaultResourceEntries.Count)
                    foreach (var key in resourceEntries[lang].Keys.Where(key => !defaultResourceEntries.Keys.Contains(key)))
                        resourceKeyDifferences[lang].UnnecesaryKeys.Add(key);
            }

        }

        private void CompareDefaultAndReferencedLocalizations()
        {
            resourceValueDifferences = new Dictionary<string, ResourceValueComparisonContainer>();
            foreach (var item in defaultResourceEntries)
            {
                var key = item.Key;
                var defValue = item.Value;
                if (resourceEntries[comparedLanguage].ContainsKey(key))
                {
                    var refValue = resourceEntries[comparedLanguage][key];
                    if (refValue != defValue)
                    {
                        resourceValueDifferences[key] = new ResourceValueComparisonContainer(defValue,refValue);
                    }
                }
            }
        }

        public bool AreKeysIdentical()
        {
            FindMissingKeys();
            FindUnnecessaryKeys();
            return languagesAbbreviations.All(lang => resourceKeyDifferences[lang].NoDifferences());
        }

        public bool AreValuesConsistent(string comparedLanguage = "de")
        {
            this.comparedLanguage = comparedLanguage;

            FindMissingKeys(comparedLanguage);
            FindUnnecessaryKeys(comparedLanguage);

            CompareDefaultAndReferencedLocalizations();

            return resourceValueDifferences.Count == 0 &&
                   resourceKeyDifferences[comparedLanguage].MissingKeys.Count == 0 &&
                   resourceKeyDifferences[comparedLanguage].UnnecesaryKeys.Count == 0;
        }

        public string GetDifferentKeysString()
        {
            var builder = new StringBuilder();
            builder.AppendLine(string.Format("Differences found for {0}", resourceFileName));
            AppendKeyInformation(builder);

            return builder.ToString();

        }

        public string GetDifferentValueString()
        {
            var builder = new StringBuilder();
            builder.AppendLine(string.Format("Inconsistent values found for {0}", resourceFileName));

            AppendKeyInformation(builder, comparedLanguage);

            builder.AppendLine();
            builder.AppendLine("Inconsistent values: ");
            AppendValueInformation(builder);

            return builder.ToString();
        }

        private void AppendKeyInformation(StringBuilder builder, string language = "")
        {
            var languages = language == "" ? languagesAbbreviations : new List<string>() {language};
            foreach (var lang in languages)
            {
                if (resourceKeyDifferences[lang].MissingKeys.Count > 0)
                {
                    builder.AppendLine();
                    builder.AppendLine(string.Format("###Missing keys in {0} localization", lang.ToUpper()));
                    foreach (var key in resourceKeyDifferences[lang].MissingKeys)
                    {
                        builder.AppendLine(string.Format("   {0}", key));
                    }
                    var emptyKeyFormat = "<data name=\"{0}\" xml:space=\"preserve\">\n\t<value></value>\n</data>";
                    builder.AppendLine("Empty keys:");
                    foreach (var key in resourceKeyDifferences[lang].MissingKeys)
                    {
                        builder.AppendLine(string.Format(emptyKeyFormat, key));
                    }
                }
                if (resourceKeyDifferences[lang].UnnecesaryKeys.Count > 0)
                {
                    builder.AppendLine();
                    builder.AppendLine(string.Format("###Possible unecessary keys in {0} localization", lang.ToUpper()));
                    foreach (var key in resourceKeyDifferences[lang].UnnecesaryKeys)
                    {
                        builder.AppendLine(string.Format("   {0}", key));
                    }
                }
            }
        }

        private void AppendValueInformation(StringBuilder builder)
        {
            foreach (var item in resourceValueDifferences)
            {
                builder.AppendLine(string.Format("###Key:           {0}", item.Key));
                builder.AppendLine(string.Format("   Default value: {0}", item.Value.DefaultValue));
                builder.AppendLine(string.Format("   {1}      value: {0}", item.Value.ComparedValue, comparedLanguage.ToUpper()));
                builder.AppendLine();
            }
        }
    }
}
