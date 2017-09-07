using System;

namespace ASTRA.EMSG.Business.Infrastructure.Reporting
{
    public static class OutputFormatExtensions
    {
        public static string ToFormatName(this OutputFormat outputFormat)
        {
            switch (outputFormat)
            {
                case OutputFormat.Image: return "IMAGE";
                case OutputFormat.Png: return "IMAGE";
                case OutputFormat.Pdf: return "PDF";
                case OutputFormat.Excel: return "EXCEL";
                case OutputFormat.Word: return "WORD";

                default:
                    throw new ArgumentOutOfRangeException("outputFormat");
            }
        }

        public static string ToFileExtension(this OutputFormat outputFormat)
        {
            switch (outputFormat)
            {
                case OutputFormat.Image: return "tiff";
                case OutputFormat.Png: return "png";
                case OutputFormat.Pdf: return "pdf";
                case OutputFormat.Excel: return "xls";
                case OutputFormat.Word: return "doc";
                case OutputFormat.Xml: return "xml";

                default:
                    throw new ArgumentOutOfRangeException("outputFormat");
            }
        }
    }
}