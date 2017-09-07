using Kendo.Mvc;

namespace ASTRA.EMSG.Web.Areas.Common.GridCommands
{
    public class StrassenabschnittGISGridCommand : GridCommand
    {
        public string StrassennameFilter { get; set; }
        public string Ortsbezeichnung { get; set; }
    }
}