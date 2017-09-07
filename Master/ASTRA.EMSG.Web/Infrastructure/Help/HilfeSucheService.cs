using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using ASTRA.EMSG.Business.Models.Common;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Common.Enums;
using ASTRA.EMSG.Common.Master.Logging;
using HtmlAgilityPack;
using Resources;

namespace ASTRA.EMSG.Web.Infrastructure.Help
{
    public interface IHilfeSucheService : IService
    {
        List<HilfeSucheModel> Suche(EmsgLanguage language, string filter);
        void ReloadHelp();
    }

    public class HilfeSucheService : IHilfeSucheService
    {
        private readonly IServerPathProvider serverPathProvider;
        private Lazy<Dictionary<EmsgLanguage, Dictionary<string, string>>> helpTexts;
        private Lazy<Dictionary<EmsgLanguage, Dictionary<string, string>>> helpInhalten;

        public HilfeSucheService(IServerPathProvider serverPathProvider)
        {
            this.serverPathProvider = serverPathProvider;
            helpTexts = new Lazy<Dictionary<EmsgLanguage, Dictionary<string, string>>>(LoadHelpTexts);
            helpInhalten = new Lazy<Dictionary<EmsgLanguage, Dictionary<string, string>>>(ParseInhaltPages);
        }

        public void ReloadHelp()
        {
            helpTexts = new Lazy<Dictionary<EmsgLanguage, Dictionary<string, string>>>(LoadHelpTexts);
            helpInhalten = new Lazy<Dictionary<EmsgLanguage, Dictionary<string, string>>>(ParseInhaltPages);
        }

        private Dictionary<EmsgLanguage, Dictionary<string, string>> ParseInhaltPages()
        {
            var result = new Dictionary<EmsgLanguage, Dictionary<string, string>>();
            var helpFolderPath = serverPathProvider.MapPath("~/Help");
            foreach (EmsgLanguage language in Enum.GetValues(typeof(EmsgLanguage)))
            {
                result[language] = new Dictionary<string, string>();
                var contentFilePath = Path.Combine(Path.Combine(helpFolderPath, GetTwoLetterLanguageName(language), GetContentPageName(language)));
                if (!File.Exists(contentFilePath))
                    continue;

                var doc = new HtmlDocument();
                doc.Load(contentFilePath);
                foreach (HtmlNode node in doc.DocumentNode.SelectNodes(@"//a[@href]"))
                {
                    var link = node.Attributes["href"].Value.Split(new[] { "#" }, StringSplitOptions.RemoveEmptyEntries)[0];

                    if (!result[language].ContainsKey(link))
                    {
                        var text = node.InnerText;
                        text = Regex.Replace(text, @"(\.)*\s+\d+", "");
                        text = text.Trim();
                        if (!string.IsNullOrWhiteSpace(text))
                            result[language][link] = text;
                    }
                }
            }
            return result;
        }

        private string GetContentPageName(EmsgLanguage language)
        {
            return HelpFileMappingStore.GetContentsPath(language.ToCultureInfo().TwoLetterISOLanguageName);
        }

        private Dictionary<EmsgLanguage, Dictionary<string, string>> LoadHelpTexts()
        {
            var result = new Dictionary<EmsgLanguage, Dictionary<string, string>>();
            var helpFolderPath = serverPathProvider.MapPath("~/Help");
            foreach (EmsgLanguage language in Enum.GetValues(typeof(EmsgLanguage)))
            {
                result[language] = new Dictionary<string, string>();
                var languagePath = Path.Combine(helpFolderPath, GetTwoLetterLanguageName(language));
                foreach (var helpFile in helpInhalten.Value[language])
                {
                    string helpPath = Path.Combine(languagePath, helpFile.Key);
                    if (File.Exists(helpPath))
                        result[language][helpFile.Key] = File.ReadAllText(helpPath);
                    else
                    {
                        Loggers.HelpLogger.ErrorFormat("The expected help file {0} could not be found at {1}!", helpFile.Key, helpPath);
                        result[language][helpFile.Key] = "";
                    }
                }
            }
            return result;
        }

        private static string GetTwoLetterLanguageName(EmsgLanguage language)
        {
            return language.ToCultureInfo().TwoLetterISOLanguageName;
        }

        public List<HilfeSucheModel> Suche(EmsgLanguage language, string filter)
        {
            var result = new List<HilfeSucheModel>();
            if (string.IsNullOrWhiteSpace(filter))
                return helpInhalten.Value[language].Select(h => new HilfeSucheModel
                    {
                        InhaltLink = GetInhaltLink(language, h.Key),
                        InhaltText = h.Value,
                        MatchCountText = GetMatchCountText()
                    }).ToList(); ;

            var filterRegex = new Regex(Regex.Escape(filter), RegexOptions.IgnoreCase);
            foreach (var helpFile in helpTexts.Value[language])
            {
                var match = filterRegex.Matches(helpFile.Value);
                if (match.Count > 0)
                    result.Add(new HilfeSucheModel()
                        {
                            InhaltLink = GetInhaltLink(language, helpFile.Key),
                            MatchCount = match.Count,
                            MatchCountText = GetMatchCountText(match.Count),
                            InhaltText = helpInhalten.Value[language][helpFile.Key]
                        });
            }
            return result;
        }

        private string GetMatchCountText(int? matchCount = null)
        {
            if (matchCount == null)
                return string.Empty;
            return string.Format("{0} {1}", matchCount.Value, TextLocalization.FindCount);
        }

        private static string GetInhaltLink(EmsgLanguage language, string helpFile)
        {
            return new UrlHelper(HttpContext.Current.Request.RequestContext).Action("HilfeSucheResult", "HilfeSuche", new { helpFilePath = helpFile, culture = GetTwoLetterLanguageName(language) });
        }
    }
}