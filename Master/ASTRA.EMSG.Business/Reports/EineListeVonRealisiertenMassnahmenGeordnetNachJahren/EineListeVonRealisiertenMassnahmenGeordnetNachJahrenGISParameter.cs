using System;
using ASTRA.EMSG.Business.Reporting;
using ASTRA.EMSG.Business.Services.FilterBuilders;

namespace ASTRA.EMSG.Business.Reports.EineListeVonRealisiertenMassnahmenGeordnetNachJahren
{
    public class EineListeVonRealisiertenMassnahmenGeordnetNachJahrenGISParameter : EmsgTabellarischeReportParameter, IProjektnameFilter, ICurrentMandantFilter, ILeitendeOrganisation, IErfassungsPeriodVonBisFilter
    {
        public string Projektname { get; set; }

        public string LeitendeOrganisation { get; set; }

        public Guid ErfassungsPeriodIdVon { get; set; }

        public Guid ErfassungsPeriodIdBis { get; set; }
    }
}