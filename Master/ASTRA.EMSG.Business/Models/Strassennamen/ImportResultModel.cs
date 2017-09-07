using System.Collections.Generic;

namespace ASTRA.EMSG.Business.Models.Strassennamen
{
    public class ImportResultModel<TImportModel, TImportOverviewModel>
    {
        public ImportResultModel(List<string> errors = null, List<string> warnings = null)
        {
            Errors = errors ?? new List<string>();
            Warnings = warnings ?? new List<string>();

            CreateImportOverviewModels = new List<TImportOverviewModel>();
            UpdateImportOverviewModels = new List<TImportOverviewModel>();

            CreateImportModels = new List<TImportModel>();
            UpdateImportModels = new List<TImportModel>();
        }

        public List<string> Errors { get; set; }
        public List<string> Warnings { get; set; }

        public List<TImportModel> CreateImportModels { get; set; }
        public List<TImportModel> UpdateImportModels { get; set; }

        public List<TImportOverviewModel> CreateImportOverviewModels { get; set; }
        public List<TImportOverviewModel> UpdateImportOverviewModels { get; set; }
    }
}
