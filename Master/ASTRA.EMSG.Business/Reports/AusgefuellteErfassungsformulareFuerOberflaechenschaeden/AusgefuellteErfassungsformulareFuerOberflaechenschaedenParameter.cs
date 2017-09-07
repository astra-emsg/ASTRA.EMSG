using System;
using ASTRA.EMSG.Business.Reporting;
using ASTRA.EMSG.Business.Services.FilterBuilders;
using ASTRA.EMSG.Common.Enums;

namespace ASTRA.EMSG.Business.Reports.AusgefuellteErfassungsformulareFuerOberflaechenschaeden
{
    public class AusgefuellteErfassungsformulareFuerOberflaechenschaedenParameter : EmsgTabellarischeReportParameter, IStrassennameFilter, IInspektionsroutenameFilter, IEigentuemerFilter, IAufnahmedatumVonBisFilter, IZustandsindexVonBisFilter
    {
        public string Strassenname { get; set; }
        public string Inspektionsroutename { get; set; }
        public EigentuemerTyp? Eigentuemer { get; set; }

        public DateTime? AufnahmedatumVon { get; set; }
        public DateTime? AufnahmedatumBis { get; set; }

        public decimal? ZustandsindexVon { get; set; }
        public decimal? ZustandsindexBis { get; set; }
    }
}
