using System;
using System.ComponentModel.DataAnnotations;
using ASTRA.EMSG.Common;
using ASTRA.EMSG.Common.Enums;

namespace ASTRA.EMSG.Business.Reports.WiederbeschaffungswertUndWertverlustProStrassenabschnitt
{
    [Serializable]
    public class WiederbeschaffungswertUndWertverlustProStrassenabschnittPo
    {
        public Guid Id { get; set; }

        public string Strassenname { get; set; }        
        public string BezeichnungVon { get; set; }
        public string BezeichnungBis { get; set; }
        public virtual EigentuemerTyp Strasseneigentuemer { get; set; }
        public string StrasseneigentuemerBezeichnung { get; set; }

        public string BelastungskategorieTyp { get; set; }
        public string BelastungskategorieBezeichnung { get; set; }

        [DisplayFormat(DataFormatString = FormatStrings.ShortDecimalFormat, NullDisplayText = "")]
        public decimal? Laenge { get; set; }

        [DisplayFormat(DataFormatString = FormatStrings.NoDecimalFormat, NullDisplayText = "")]
        public decimal? FlaecheFahrbahn { get; set; }

        public int? Abschnittsnummer { get; set; }
        public TrottoirTyp Trottoir { get; set; }

        public string TrottoirBezeichnung { get; set; }
        public string Ortsbezeichnung { get; set; }

        private decimal? flaecheTrottoirLinks;
        [DisplayFormat(DataFormatString = FormatStrings.NoDecimalFormat, NullDisplayText = "")]
        public decimal? FlaecheTrottoirLinks
        {
            get { return GetFlaecheTrottoir(flaecheTrottoirLinks, TrottoirTyp.Links); }
            set { flaecheTrottoirLinks = value; }
        }

        private decimal? flaecheTrottoirRechts;
        [DisplayFormat(DataFormatString = FormatStrings.NoDecimalFormat, NullDisplayText = "")]
        public decimal? FlaecheTrottoirRechts
        {
            get { return GetFlaecheTrottoir(flaecheTrottoirRechts, TrottoirTyp.Rechts); }
            set { flaecheTrottoirRechts = value; }
        }

        private decimal? GetFlaecheTrottoir(decimal? flaecheTrottoir, TrottoirTyp trottoirTyp)
        {
            if (Trottoir == TrottoirTyp.NochNichtErfasst)
                return null;

            if (Trottoir == TrottoirTyp.BeideSeiten || Trottoir == trottoirTyp)
                return flaecheTrottoir;

            return 0;
        }

        [DisplayFormat(DataFormatString = FormatStrings.NoDecimalFormat, NullDisplayText = "")]
        public decimal Wiederbeschaffungswert { get; set; }

        [DisplayFormat(DataFormatString = FormatStrings.NoDecimalFormat, NullDisplayText = "")]
        public decimal AlterungsbeiwertI { get; set; }

        [DisplayFormat(DataFormatString = FormatStrings.NoDecimalFormat, NullDisplayText = "")]
        public decimal AlterungsbeiwertII { get; set; }

        [DisplayFormat(DataFormatString = FormatStrings.NoDecimalFormat, NullDisplayText = "")]
        public decimal WertlustI { get; set; }

        [DisplayFormat(DataFormatString = FormatStrings.NoDecimalFormat, NullDisplayText = "")]
        public decimal WertlustII { get; set; }
    }
}
