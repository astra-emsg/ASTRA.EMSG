using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using ASTRA.EMSG.Common.Utils;

namespace ASTRA.EMSG.IntegrationTests.ResourceTests
{
    public class ResourceComparingService
    {
        private Dictionary<string, string> defaultResourceEntries;
        private Dictionary<string, Dictionary<string, string>> resourceEntries; 
        private Dictionary<string, ResourceKeyComparisonContainer> differences;

        private readonly List<string> languagesAbbreviations;
        private readonly string resourceFileName;
        private const string ResourcePath = "ResourceTests/Resources/";

        public ResourceComparingService(string resourceFileName, IEnumerable<string> languageAbbreviations = null)
        {
            this.resourceFileName = resourceFileName;
            languagesAbbreviations = languageAbbreviations != null
                                         ? languageAbbreviations.ToList()
                                         : new List<string>() {"de", "fr", "it"};
            InitializeDictionaries();
            ReadAllEntries();
            FindMissingKeys();
            FindUnnecessaryKeys();
        }

        private void InitializeDictionaries()
        {
            defaultResourceEntries = new Dictionary<string, string>();
            resourceEntries = new Dictionary<string, Dictionary<string, string>>();
            differences = new Dictionary<string, ResourceKeyComparisonContainer>();
            foreach (var lang in languagesAbbreviations)
            {
                resourceEntries[lang] = new Dictionary<string, string>();
                differences[lang] = new ResourceKeyComparisonContainer();
            }
        }

        private void ReadAllEntries()
        {
            ReadEntries();
            foreach (var lang in languagesAbbreviations)
                ReadEntries(lang);
        }

        private void ReadEntries(string language = "")
        {
            bool defaultResource = language == "";

            var langSuffix = !defaultResource ? string.Format(".{0}", language) : "";

            var filePath = string.Format("{0}{1}{2}.resx", ResourcePath, resourceFileName, langSuffix);

            var reader = new ResXResourceReader(filePath);
            var dict = reader.GetEnumerator();

            while (dict.MoveNext())
                if (defaultResource)
                    defaultResourceEntries[dict.Key.ToString()] = dict.Value.ToString();
                else
                    resourceEntries[language][dict.Key.ToString()] = dict.Value.ToString();
        }

        private void FindMissingKeys()
        {
            foreach (var key in defaultResourceEntries.Keys)
                foreach (var lang in languagesAbbreviations.Where(lang => !resourceEntries[lang].Keys.Contains((key))))
                    differences[lang].MissingKeys.Add(key);
        }

        private void FindUnnecessaryKeys()
        {
            foreach (var lang in languagesAbbreviations)
            {
                if (resourceEntries[lang].Keys.Count + differences[lang].MissingKeys.Count != defaultResourceEntries.Count)
                {
                    foreach (var key in resourceEntries[lang].Keys.Where(key => !defaultResourceEntries.Keys.Contains(key)))
                        differences[lang].UnnecesaryKeys.Add(key);
                }
            }
        }

        public bool AreKeysIdentical()
        {
            return languagesAbbreviations.All(lang => differences[lang].NoDifferences());
        }

        public bool AllValuesExist(string[] ignoredKeys)
        {
            return 
                defaultResourceEntries.Where(k => !ignoredKeys.Contains(k.Key)).All(v => v.Value.HasText())
                &&
                languagesAbbreviations
                    .All(lang => resourceEntries[lang].Where(k => !ignoredKeys.Contains(k.Key)).All(v => v.Value.HasText()));
        }

        public string GetMissingsKeysString(string[] ignoredKeys)
        {
            var builder = new StringBuilder();
            builder.AppendLine(string.Format("Missing values found found for {0}", resourceFileName));
            builder.AppendLine();
            builder.AppendLine(string.Format("###Missing values in the default localization"));
            foreach (var entry in defaultResourceEntries.Where(e => !ignoredKeys.Contains(e.Key) && !e.Value.HasText()))
            {
                builder.AppendLine(string.Format("   {0}", entry.Key));
            }

            foreach (var lang in languagesAbbreviations)
            {
                builder.AppendLine();
                builder.AppendLine(string.Format("###Missing values in {0} localization", lang.ToUpper()));
                foreach (var entry in resourceEntries[lang].Where(e => !ignoredKeys.Contains(e.Key) && !e.Value.HasText()))
                {
                    builder.AppendLine(string.Format("   {0}", entry.Key));
                }
            }

            return builder.ToString();
        }

        public string GetDifferentKeysString()
        {
            var builder = new StringBuilder();
            builder.AppendLine(string.Format("Differences found for {0}", resourceFileName));
            AppendInformation(builder);

            return builder.ToString();

        }

        private void AppendInformation(StringBuilder builder)
        {
            foreach (var lang in languagesAbbreviations)
            {
                if (differences[lang].MissingKeys.Count > 0)
                {
                    builder.AppendLine();
                    builder.AppendLine(string.Format("###Missing keys in {0} localization", lang.ToUpper()));
                    foreach (var key in differences[lang].MissingKeys)
                    {
                        builder.AppendLine(string.Format("   {0}", key));
                    }
                }
                if (differences[lang].UnnecesaryKeys.Count > 0)
                {
                    builder.AppendLine();
                    builder.AppendLine(string.Format("###Possible unecessary keys in {0} localization", lang.ToUpper()));
                    foreach (var key in differences[lang].UnnecesaryKeys)
                    {
                        builder.AppendLine(string.Format("   {0}", key));
                    }
                }
            }
        }
    }
}
