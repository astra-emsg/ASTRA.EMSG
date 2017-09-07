using ASTRA.EMSG.Common.Enums;

namespace ASTRA.EMSG.Web.Areas.Auswertungen.ReportGridCommands
{
    public class NochNichtInspizierteStrassenabschnitteGridCommand : ReportGridCommand
    {
        public string Strassenname { get; set; }
        public EigentuemerTyp? Eigentuemer { get; set; }
    }
}