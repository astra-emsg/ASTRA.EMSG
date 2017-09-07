using System.Collections.Generic;

namespace ASTRA.EMSG.Business.Models.Strassennamen
{
    public class StrassenabschnittImportResultModel : ImportResultModel<StrassenabschnittImportModel, StrassenabschnittImportOverviewModel>
    {
        public StrassenabschnittImportResultModel(List<string> errors = null, List<string> warnings = null)
            : base (errors, warnings)
        {
        }
    }
}