using System;
using ASTRA.EMSG.Common.Enums;

namespace ASTRA.EMSG.Web.Areas.Auswertungen.ReportGridCommands
{
    public class ListeDerInspektionsroutenGridCommand : GisReportGridCommand
    {
        public string Inspektionsroutename { get; set; }
        
        public string Strassenname { get; set; }
        
        public string InspektionsrouteInInspektionBei { get; set; }

        public DateTime? InspektionsrouteInInspektionBisVon { get; set; }
        public DateTime? InspektionsrouteInInspektionBisBis { get; set; }

        public int? Eigentuemer { get; set; }
        public EigentuemerTyp? EigentuemerTyp { get { return Eigentuemer.HasValue ? (EigentuemerTyp)Eigentuemer.Value : (EigentuemerTyp?)null; } }
    }
}