namespace ASTRA.EMSG.Business.Reports.RealisiertenMassnahmenWertverlustZustandsindexProJahrGrafische
{
    public class RealisiertenMassnahmenWertverlustZustandsindexProJahrGrafischeDiagramPo
    {
        public decimal RealisierteMassnahmen { get; set; }
        public decimal WertVerlust { get; set; }
        public decimal? MittlererZustandindex { get; set; }

        public int JahrVon { get; set; }
        public int JahrBis { get; set; }

        public string JahrBezeichnung
        {
            get
            {
                if (JahrVon == JahrBis)
                    return JahrVon.ToString();

                return string.Format("{0}-{1}", JahrVon, JahrBis);
            }
        }

        public string ColorCode { get; set; }
    }
}
