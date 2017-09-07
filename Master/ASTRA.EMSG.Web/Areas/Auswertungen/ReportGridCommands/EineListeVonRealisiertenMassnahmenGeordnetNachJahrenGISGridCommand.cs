using System;

namespace ASTRA.EMSG.Web.Areas.Auswertungen.ReportGridCommands
{
    public class EineListeVonRealisiertenMassnahmenGeordnetNachJahrenGISGridCommand : ReportGridCommand
    {
        public Guid? ErfassungsPeriodIdVon { get; set; }

        public Guid? ErfassungsPeriodIdBis { get; set; }

        public string Projektname { get; set; }

        public string LeitendeOrganisation { get; set; }
    }
}