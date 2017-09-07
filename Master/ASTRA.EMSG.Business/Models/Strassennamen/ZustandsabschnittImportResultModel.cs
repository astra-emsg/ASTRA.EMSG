using System.Collections.Generic;

namespace ASTRA.EMSG.Business.Models.Strassennamen
{
    public class ZustandsabschnittImportResultModel : ImportResultModel<ZustandsabschnittImportModel, ZustandsabschnittImportOverviewModel>
    {
        public ZustandsabschnittImportResultModel(List<string> errors = null, List<string> warnings = null)
            : base (errors, warnings)
        {
        }
    }
}