using System;
using ASTRA.EMSG.Business.Services.FilterBuilders;

namespace ASTRA.EMSG.Web.Areas.Auswertungen.ReportGridCommands
{
    public class EineListeVonRealisiertenMassnahmenGeordnetNachJahrenGridCommand : ReportGridCommand
    {
        public Guid? ErfassungsPeriodIdVon { get; set; }

        public Guid? ErfassungsPeriodIdBis { get; set; }

        public string Projektname { get; set; }
    }
}