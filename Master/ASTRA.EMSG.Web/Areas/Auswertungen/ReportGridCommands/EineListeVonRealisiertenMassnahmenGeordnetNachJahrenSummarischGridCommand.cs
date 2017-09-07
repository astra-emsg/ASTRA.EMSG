using System;

namespace ASTRA.EMSG.Web.Areas.Auswertungen.ReportGridCommands
{
    public class EineListeVonRealisiertenMassnahmenGeordnetNachJahrenSummarischGridCommand : ReportGridCommand
    {
        public Guid? ErfassungsPeriodIdVon { get; set; }

        public Guid? ErfassungsPeriodIdBis { get; set; }

        public string Projektname { get; set; }
    }
}