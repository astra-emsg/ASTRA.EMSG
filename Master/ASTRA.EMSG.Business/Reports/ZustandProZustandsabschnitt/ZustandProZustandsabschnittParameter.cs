using ASTRA.EMSG.Business.Reporting;
using ASTRA.EMSG.Business.Services.FilterBuilders;
using ASTRA.EMSG.Common.Enums;


namespace ASTRA.EMSG.Business.Reports.ZustandProZustandsabschnitt
{
    public class ZustandProZustandsabschnittParameter : EmsgGisReportParameter, IErfassungsPeriodFilter, ICurrentMandantFilter, IEigentuemerFilter, IZustandsindexVonBisFilter, IStrassennameFilter, IOrtsbezeichnungFilter
    {
        public EigentuemerTyp? Eigentuemer { get; set; }
        public string Strassenname { get; set; }
        public string ExternalId { get; set; }
        public decimal? ZustandsindexVon { get; set; }
        public decimal? ZustandsindexBis { get; set; }
        public string Ortsbezeichnung { get; set; }
        public string Laenge { get; set; }
    }
}