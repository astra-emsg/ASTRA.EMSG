using ASTRA.EMSG.Common.Enums;

namespace ASTRA.EMSG.Business.Reports.ZustandsspiegelProJahrGrafische
{
    public class ZustandsspiegelProJahrGrafischeDiagramPo
    {
        public decimal FlaecheFahrbahn { get; set; }

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

        public ZustandsindexTyp ZustandsindexTyp { get; set; }
        public string ZustandsindexTypBezeichnung { get; set; }

        public string ColorCode { get; set; }
    }
}
