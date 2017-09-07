namespace ASTRA.EMSG.Business.Reports.MengeProBelastungskategorieGrafische
{
    public class MengeProBelastungskategorieGrafischePo
    {
        public decimal Menge { get; set; }

        public string BelastungskategorieKurzBezeichnung { get; set; }
        public string BelastungskategorieTyp { get; set; }
        public string MengeArtKurzBezeichnung { get; set; }

        public int BelastungskategorieReihenfolge { get; set; }

        public string ColorCode { get; set; }
    }
}
