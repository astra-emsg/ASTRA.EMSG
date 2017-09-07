using ASTRA.EMSG.Common.Enums;

namespace ASTRA.EMSG.Web.Areas.Auswertungen.ReportGridCommands
{
    public class MengeProBelastungskategorieGridCommand : ReportGridCommand
    {
        public string BelastungskategorieTyp { get; set; }
        public EigentuemerTyp? Eigentuemer { get; set; }
    }
}