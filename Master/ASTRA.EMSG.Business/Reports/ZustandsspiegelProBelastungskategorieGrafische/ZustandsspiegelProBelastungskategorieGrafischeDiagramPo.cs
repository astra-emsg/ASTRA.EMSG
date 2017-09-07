using ASTRA.EMSG.Common.Enums;

namespace ASTRA.EMSG.Business.Reports.ZustandsspiegelProBelastungskategorieGrafische
{
    public class ZustandsspiegelProBelastungskategorieGrafischeDiagramPo
    {
        public decimal Value { get; set; }

        public string Bezeichnung { get; set; }

        public ZustandsindexTyp ZustandsindexTyp { get; set; }

        public FlaecheTyp FlaecheTyp { get; set; }

        public int ColumnSortOrder { get; set; }
        public int RowSortOrder { get; set; }

        public string ColorCode { get; set; }
        public bool ShowInDiagram { get { return ZustandsindexTyp != ZustandsindexTyp.Unbekannt; } }
    }
}
