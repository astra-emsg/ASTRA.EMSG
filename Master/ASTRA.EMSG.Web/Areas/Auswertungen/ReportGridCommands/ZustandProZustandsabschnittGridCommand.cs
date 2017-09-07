using ASTRA.EMSG.Common.Enums;

namespace ASTRA.EMSG.Web.Areas.Auswertungen.ReportGridCommands
{
    public class ZustandProZustandsabschnittGridCommand : GisReportGridCommand
    {
        public int? Eigentuemer { get; set; }
        public EigentuemerTyp? EigentuemerTyp { get { return Eigentuemer.HasValue ? (EigentuemerTyp)Eigentuemer.Value : (EigentuemerTyp?)null; } }

        public string Strassenname { get; set; }
        public decimal? ZustandsindexVon { get; set; }
        public decimal? ZustandsindexBis { get; set; }

        public string Ortsbezeichnung { get; set; }

        public string Laenge { get; set; }
    }
}