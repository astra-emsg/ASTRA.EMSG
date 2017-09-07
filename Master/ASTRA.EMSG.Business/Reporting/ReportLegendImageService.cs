using ASTRA.EMSG.Business.Services.Common;

namespace ASTRA.EMSG.Business.Reporting
{
    public interface IReportLegendImageService : IService
    {
        string GetLegendUrlForEnum<TEnum>(TEnum enumValue) where TEnum : struct;
        string GetLegendUrlForBelastungskategorie(string belastungskategorieKey);
        string GetLegendUrl(string key);
    }

    public class ReportLegendImageService : IReportLegendImageService
    {
        private readonly IServerPathProvider serverPathProvider;

        public ReportLegendImageService(IServerPathProvider serverPathProvider)
        {
            this.serverPathProvider = serverPathProvider;
        }

        public string GetLegendUrlForEnum<TEnum>(TEnum enumValue) where TEnum : struct
        {
            return GetLegendUrl(string.Format("{0}_{1}", typeof (TEnum).Name, enumValue.ToString()));
        }

        public string GetLegendUrlForBelastungskategorie(string belastungskategorieKey)
        {
            return GetLegendUrl(belastungskategorieKey);
        }

        public string GetLegendUrl(string key)
        {
            return string.Format("file:///{0}", serverPathProvider.MapPath(string.Format(@"~/Content/reports/{0}.png", key)).Replace("\\", "/"));
        }
    }
}
