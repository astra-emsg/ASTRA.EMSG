using System;
using ASTRA.EMSG.Business.Reporting;
using ASTRA.EMSG.Business.Services.FilterBuilders;
using ASTRA.EMSG.Common.Enums;

namespace ASTRA.EMSG.Business.Reports.EineListeVonKoordiniertenMassnahmen
{
    public class EineListeVonKoordiniertenMassnahmenParameter : EmsgGisReportParameter, IStatusFilter, IProjektnameFilter, IAusfuehrungsanfangVonBisFilter, ICurrentMandantFilter
    {
        public StatusTyp? Status { get; set; }
        public string Projektname { get; set; }
        public DateTime? AusfuehrungsanfangVon { get; set; }
        public DateTime? AusfuehrungsanfangBis { get; set; }
    }
}