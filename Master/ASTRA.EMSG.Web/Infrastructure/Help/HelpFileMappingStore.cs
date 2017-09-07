using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Xml.Serialization;
using ASTRA.EMSG.Business.Models.Common;
using ASTRA.EMSG.Business.Services.Administration;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Common.Enums;

namespace ASTRA.EMSG.Web.Infrastructure.Help
{
    public static class HelpFileMappingStore
    {
        private static readonly Dictionary<string, List<FilePath>> MappingDictionary = new Dictionary<string, List<FilePath>>();
        private static List<FilePath> helpContents;

        public static string GetHelpFilePath(string getHelpFileKey, string twoLetterISOLanguageName)
        {
            return MappingDictionary.ContainsKey(getHelpFileKey) ? GetFilePathByCulture(MappingDictionary[getHelpFileKey], twoLetterISOLanguageName) : null;
        }

        static HelpFileMappingStore()
        {
            BuildUpMappingDictionary();
        }

        public static void RefreshMappingDictionary()
        {
            MappingDictionary.Clear();
            BuildUpMappingDictionary();
        }

        private static void BuildUpMappingDictionary()
        {
            var helpFileMappingPath = Path.Combine(HttpContext.Current.Server.MapPath("~/Help"), HelpSystemService.HelpFileMappingFileName);

            if (!File.Exists(helpFileMappingPath))
                return;

            using (var fs = File.OpenRead(helpFileMappingPath))
            {
                var helpFileMapping = GetHelpFileMappingXmlSerializer().Deserialize(fs) as HelpFileMapping;

                if (helpFileMapping != null)
                {
                    helpContents = helpFileMapping.HelpContents;
                    foreach (var helpMappingItem in helpFileMapping.HelpMappingItems)
                        MappingDictionary.Add(helpMappingItem.Key, helpMappingItem.FilePath);
                }
            }
        }

        public static void GenerateSampleMappingXml(string helpFileMappingPath)
        {
            using (var fs = File.OpenWrite(helpFileMappingPath))
            {
                GetHelpFileMappingXmlSerializer().Serialize(fs,
                                        new HelpFileMapping
                                            {
                                               HelpContents = new List<FilePath>() { new FilePath(){Language = "test", Path = "tset"}}
                                                   ,
                                                HelpMappingItems =
                                                    new List<HelpMappingItem>
                                                        {
                                                            new HelpMappingItem
                                                                {Key = "Home_Index", FilePath = new List<FilePath>() { new FilePath() { Path = "Home.html"}}},
                                                            new HelpMappingItem
                                                                {Key = "Administration_ArbeitsmodusWechseln_Index",  FilePath = new List<FilePath>() { new FilePath() { Path= "AdminHelps/Arbeitsmodus Wechseln.html"}}},
                                                        }
                                            });
            }
        }

        private static XmlSerializer GetHelpFileMappingXmlSerializer()
        {
            return new XmlSerializer(typeof (HelpFileMapping), new[] {typeof (HelpMappingItem)});
        }

        public static string GetContentsPath(string cultureCode)
        {
            return GetFilePathByCulture(helpContents, cultureCode);
        }

        private static string GetFilePathByCulture(List<FilePath> filePaths, string twoLetterISOLanguageName)
        {
            var filePath = filePaths.SingleOrDefault(f => !string.IsNullOrWhiteSpace(f.Language) && f.Language.ToLowerInvariant() == twoLetterISOLanguageName.ToLowerInvariant());
            if (filePath == null)
                filePath = filePaths.Single(f => string.IsNullOrEmpty(f.Language));
            return filePath.Path;
        }
    }
}