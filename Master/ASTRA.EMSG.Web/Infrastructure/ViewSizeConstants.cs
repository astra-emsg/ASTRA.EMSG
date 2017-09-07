namespace ASTRA.EMSG.Web.Infrastructure
{
    public class ViewSizeConstants
    {
        public const int GisReportFilterFieldLabelWidth = 118;
        public const int GisReportFilterInputFieldWidth = 185;

        public static int GisReportFilterBlockWidth
        {
            get { return GisReportFilterFieldLabelWidth + GisReportFilterInputFieldWidth + 38; }
        }
    }
}