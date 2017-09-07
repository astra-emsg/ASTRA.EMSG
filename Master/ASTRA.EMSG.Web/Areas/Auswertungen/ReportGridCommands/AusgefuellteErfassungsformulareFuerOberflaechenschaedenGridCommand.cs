using System;
using ASTRA.EMSG.Common.Enums;

namespace ASTRA.EMSG.Web.Areas.Auswertungen.ReportGridCommands
{
    public class AusgefuellteErfassungsformulareFuerOberflaechenschaedenGridCommand : ReportGridCommand
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