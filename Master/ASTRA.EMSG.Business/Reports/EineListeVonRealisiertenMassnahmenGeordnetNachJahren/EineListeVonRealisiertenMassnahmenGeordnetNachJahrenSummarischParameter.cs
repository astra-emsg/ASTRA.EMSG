using System;
using ASTRA.EMSG.Business.Reporting;
using ASTRA.EMSG.Business.Services.FilterBuilders;

namespace ASTRA.EMSG.Business.Reports.EineListeVonRealisiertenMassnahmenGeordnetNachJahren
{
    public class EineListeVonRealisiertenMassnahmenGeordnetNachJahrenSummarischParameter : EmsgTabellarischeReportParameter, IErfassungsPeriodVonBisFilter, IProjektnameFilter, ICurrentMandantFilter
    {
        public string Projektname { get; set; }

        public Guid ErfassungsPeriodIdVon { get; set; }

        public Guid ErfassungsPeriodIdBis { get; set; }
    }
}