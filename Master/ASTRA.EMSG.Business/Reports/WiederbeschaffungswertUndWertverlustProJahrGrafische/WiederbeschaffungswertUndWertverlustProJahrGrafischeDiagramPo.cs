using System;

namespace ASTRA.EMSG.Business.Reports.WiederbeschaffungswertUndWertverlustProJahrGrafische
{
    public class WiederbeschaffungswertUndWertverlustProJahrGrafischeDiagramPo
    {
        public decimal FlaecheFahrbahn { get; set; }
        public decimal WertVerlust { get; set; }
        public decimal WiederBeschaffungsWert { get; set; }

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

        public Guid BelastungskategorieId { get; set; }
        public string BelastungskategorieBezeichnung { get; set; }
        public string BelastungskategorieTyp { get; set; }
        public int BelastungskategorieReihenfolge { get; set; }

        public string ColorCode { get; set; }
    }
}
