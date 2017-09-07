using System.Collections.Generic;
using System.IO;

namespace ASTRA.EMSG.Business.Services.PackageService
{
    public class ExportResult
    {
        public ExportResult()
        {
            Errors = new List<string>();
            Stream = new MemoryStream();
        }

        public List<string> Errors { get; set; }
        public Stream Stream { get; set; }
    }
}