using System.Collections.Generic;

namespace ASTRA.EMSG.Business.Infrastructure.Xlsx
{
    public interface IXlsxImportResult
    {
        List<string> ErrorList { get; set; }
        bool HasError { get; }
    }
}