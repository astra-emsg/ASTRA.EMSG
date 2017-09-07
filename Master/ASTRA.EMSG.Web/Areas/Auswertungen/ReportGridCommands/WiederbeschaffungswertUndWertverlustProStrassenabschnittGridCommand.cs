using ASTRA.EMSG.Common.Enums;

namespace ASTRA.EMSG.Web.Areas.Auswertungen.ReportGridCommands
{
    public class WiederbeschaffungswertUndWertverlustProStrassenabschnittGridCommand : ReportGridCommand
    {
        public string Strassenname { get; set; }
        public string Ortsbezeichnung { get; set; }
        public EigentuemerTyp? Eigentuemer { get; set; }
    }
}