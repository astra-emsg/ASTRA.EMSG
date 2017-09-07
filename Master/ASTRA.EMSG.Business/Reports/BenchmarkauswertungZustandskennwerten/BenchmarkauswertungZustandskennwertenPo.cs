namespace ASTRA.EMSG.Business.Reports.BenchmarkauswertungZustandskennwerten
{
    public class BenchmarkauswertungZustandskennwertenPo
    {
        public string BeschreibungDetails { get; set; }

        public string Einheit { get; set; }

        public int SortOrder { get; set; }

        public string Organisation { get; set; }

        public string AnzahlGemeindenInDerGruppe { get; set; }
        public string MittelwertInDerGruppe { get; set; }
        public string MinimalwertInDerGruppe { get; set; }
        public string MaximalwertInDerGruppe { get; set; }

        public string AnzahlGemeindenInAllerGemeinde { get; set; }
        public string MittelwertInAllerGemeinde { get; set; }
        public string MinimalwertInAllerGemeinde { get; set; }
        public string MaximalwertInAllerGemeinde { get; set; }
    }
}
