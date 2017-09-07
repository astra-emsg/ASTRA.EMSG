using System;
using ASTRA.EMSG.Business.Reporting;
using ASTRA.EMSG.Business.Services.FilterBuilders;
using ASTRA.EMSG.Common.Enums;

namespace ASTRA.EMSG.Business.Reports.ListeDerInspektionsrouten
{
    public class ListeDerInspektionsroutenParameter : EmsgGisReportParameter, IErfassungsPeriodFilter, ICurrentMandantFilter, IStrassennameFilter, IEigentuemerFilter
    {
        public string Inspektionsroutename { get; set; }
        public string Strassenname { get; set; }
        public EigentuemerTyp? Eigentuemer { get; set; }
        public string InspektionsrouteInInspektionBei { get; set; }
        public DateTime? InspektionsrouteInInspektionBisVon { get; set; }
        public DateTime? InspektionsrouteInInspektionBisBis { get; set; }
        public string LegendImageBaseUrl { get; set; }
    }
}
