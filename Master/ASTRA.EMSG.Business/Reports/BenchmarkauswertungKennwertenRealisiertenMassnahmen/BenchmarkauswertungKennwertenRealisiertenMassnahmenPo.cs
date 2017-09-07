namespace ASTRA.EMSG.Business.Reports.BenchmarkauswertungKennwertenRealisiertenMassnahmen
{
    public class BenchmarkauswertungKennwertenRealisiertenMassnahmenPo
    {
        public string BeschreibungDetails { get; set; }
        public string Bezugsgroesse { get; set; }

        public string Einheit { get; set; }

        public int SortOrder { get; set; }

        public decimal? Organisation { get; set; }
        public decimal? GleitendesMittel { get; set; }

        public decimal? AnzahlGemeindenInDerGruppe { get; set; }
        public decimal? MittelwertInDerGruppe { get; set; }
        public decimal? MinimalwertInDerGruppe { get; set; }
        public decimal? MaximalwertInDerGruppe { get; set; }

        public decimal? AnzahlGemeindenInAllerGemeinde { get; set; }
        public decimal? MittelwertInAllerGemeinde { get; set; }
        public decimal? MinimalwertInAllerGemeinde { get; set; }
        public decimal? MaximalwertInAllerGemeinde { get; set; }
    }
}
