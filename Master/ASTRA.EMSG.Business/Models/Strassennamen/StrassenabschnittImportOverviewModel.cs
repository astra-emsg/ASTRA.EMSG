namespace ASTRA.EMSG.Business.Models.Strassennamen
{
    public class StrassenabschnittImportOverviewModel
    {
        public string Strassenname { get; set; }
        public string BezeichnungVon { get; set; }
        public string BezeichnungBis { get; set; }

        public int? Abschnittsnummer { get; set; }

        public decimal Laenge { get; set; }
    }
}