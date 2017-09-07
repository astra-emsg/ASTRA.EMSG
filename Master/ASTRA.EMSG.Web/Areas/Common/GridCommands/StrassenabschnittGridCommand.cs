using ASTRA.EMSG.Web.Areas.Auswertungen.ReportGridCommands;
using Kendo.Mvc;

namespace ASTRA.EMSG.Web.Areas.Common.GridCommands
{
    public class StrassenabschnittGridCommand : ReportGridCommand
    {
        public string StrassennameFilter { get; set; }
        public string Ortsbezeichnung { get; set; }
    }
}