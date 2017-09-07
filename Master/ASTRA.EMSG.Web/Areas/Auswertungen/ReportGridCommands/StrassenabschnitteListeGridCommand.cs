using System;
using ASTRA.EMSG.Common.Enums;

namespace ASTRA.EMSG.Web.Areas.Auswertungen.ReportGridCommands
{
    public class StrassenabschnitteListeGridCommand : GisReportGridCommand
    {
        public Guid? Belastungskategorie { get; set; }

        public string Ortsbezeichnung { get; set; }

        public int? Eigentuemer { get; set; }
        public EigentuemerTyp? EigentuemerTyp { get { return Eigentuemer.HasValue ? (EigentuemerTyp)Eigentuemer.Value : (EigentuemerTyp?)null; } }
    }
}