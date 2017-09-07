using ASTRA.EMSG.Business.Reporting;
using ASTRA.EMSG.Business.Services.FilterBuilders;
using ASTRA.EMSG.Common.Enums;

namespace ASTRA.EMSG.Business.Reports.MassnahmenvorschlagProZustandsabschnitt
{
    public class MassnahmenvorschlagProZustandsabschnittParameter : EmsgGisReportParameter, IErfassungsPeriodFilter, ICurrentMandantFilter, IEigentuemerFilter, IZustandsindexVonBisFilter, IStrassennameFilter, IDringlichkeitFilter, IOrtsbezeichnungFilter
    {
        public EigentuemerTyp? Eigentuemer { get; set; }
        public decimal? ZustandsindexVon { get; set; }
        public decimal? ZustandsindexBis { get; set; }
        public string Strassenname { get; set; }
        public DringlichkeitTyp? Dringlichkeit { get; set; }
        public string Ortsbezeichnung { get; set; }
    }
}