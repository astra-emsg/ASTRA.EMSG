namespace ASTRA.EMSG.Business.Infrastructure.Reporting
{
    public class Report
    {
        public Report(bool hasData, byte[] reportData, string fileName, string fileExtension, string mimeType, int? maxImagePreviewPageHeight, int? maxImagePreviewPageWidth)
        {
            HasData = hasData;
            ReportData = reportData;
            FileName = fileName;
            FileExtension = fileExtension;
            MimeType = mimeType;

            MaxImagePreviewPageHeight = maxImagePreviewPageHeight;
            MaxImagePreviewPageWidth = maxImagePreviewPageWidth;
        }
        
        public byte[] ReportData { get; set; }
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public string MimeType { get; set; }
        public string SavedFilePath { get; set; }
        public bool HasData { get; set; }

        public int? MaxImagePreviewPageHeight { get; set; }
        public int? MaxImagePreviewPageWidth { get; set; }
    }
}
