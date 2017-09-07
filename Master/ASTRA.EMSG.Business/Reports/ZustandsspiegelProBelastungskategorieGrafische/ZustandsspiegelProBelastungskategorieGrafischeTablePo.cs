using System;
using ASTRA.EMSG.Business.Reporting;

namespace ASTRA.EMSG.Business.Reports.ZustandsspiegelProBelastungskategorieGrafische
{
    public class ZustandsspiegelProBelastungskategorieGrafischeTablePo : IPercentHolder
    {
        public string Value { get { return Format(DecimalValue); } }
        public decimal? DecimalValue { get; set; }
        public Func<decimal?, string> Format { get; set; }
        
        public string RowBezeichnung { get; set; }

        public string ColumnBezeichnung { get; set; }

        public int RowSortOrder { get; set; }
        public int ColumnSortOrder { get; set; }

        public string LegendImageUrl { get; set; }
        public string ColorCode { get; set; }

        public bool IsSummaryRow { get; set; }

        public int SortOrder { get { return RowSortOrder; } }
        public string Group { get { return ColumnBezeichnung; } }
    }
}