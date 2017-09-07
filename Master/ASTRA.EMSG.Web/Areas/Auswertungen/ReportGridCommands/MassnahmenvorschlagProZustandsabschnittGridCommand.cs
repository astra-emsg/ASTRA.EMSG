using ASTRA.EMSG.Common.Enums;

namespace ASTRA.EMSG.Web.Areas.Auswertungen.ReportGridCommands
{
    public class MassnahmenvorschlagProZustandsabschnittGridCommand : GisReportGridCommand
    {
        public int? Eigentuemer { get; set; }
        public EigentuemerTyp? EigentuemerTyp { get { return Eigentuemer.HasValue ? (EigentuemerTyp)Eigentuemer.Value : (EigentuemerTyp?)null; } }

        public int? Dringlichkeit { get; set; }
        public DringlichkeitTyp? DringlichkeitTyp { get { return Dringlichkeit.HasValue ? (DringlichkeitTyp)Dringlichkeit.Value : (DringlichkeitTyp?)null; } }

        public string Strassenname { get; set; }
        public decimal? ZustandsindexVon { get; set; }
        public decimal? ZustandsindexBis { get; set; }
        public string Ortsbezeichnung { get; set; }
    }
}