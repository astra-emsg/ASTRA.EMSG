namespace ASTRA.EMSG.Business.Reports.RealisiertenMassnahmenWertverlustZustandsindexProJahrGrafische
{
    public class RealisiertenMassnahmenWertverlustZustandsindexProJahrGrafischeTablePo
    {
        public decimal? Value { get; set; }

        public int JahrVon { get; set; }
        public int JahrBis { get; set; }

        public int CurrentJahr { get; set; }

        public string JahrBezeichnung
        {
            get
            {
                if (JahrBis >= CurrentJahr)
                    return AktualString;

                if (JahrVon == JahrBis)
                    return JahrVon.ToString();

                return string.Format("{0}-{1}", JahrVon, JahrBis);
            }
        }

        public string AktualString { get; set; }

        public string Bezeichnung { get; set; }

        public int SortOrder { get; set; }

        public string ColorCode { get; set; }

        public string FormatString { get; set; }

        public string LegendUrl { get; set; }
    }
}
