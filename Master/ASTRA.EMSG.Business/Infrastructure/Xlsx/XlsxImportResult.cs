using System.Collections.Generic;

namespace ASTRA.EMSG.Business.Infrastructure.Xlsx
{
    public class XlsxImportResult<TModel> : IXlsxImportResult
    {
        public XlsxImportResult()
        {
            ModelInfos = new List<ModelInfo<TModel>>();
            ErrorList = new List<string>();
        }
        
        public List<ModelInfo<TModel>> ModelInfos { get; set; }
        public List<string> ErrorList { get; set; }

        public bool HasError { get { return ErrorList.Count != 0; } }
    }
}