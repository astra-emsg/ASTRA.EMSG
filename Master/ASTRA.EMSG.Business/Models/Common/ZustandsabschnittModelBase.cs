using System;
using System.ComponentModel.DataAnnotations;
using ASTRA.EMSG.Common;
using ASTRA.EMSG.Common.Enums;
using ASTRA.EMSG.Common.Utils;

namespace ASTRA.EMSG.Business.Models.Common
{
    [Serializable]
    public abstract class ZustandsabschnittModelBase : Model
    {
        public abstract Guid StrassenabschnittBaseId { get; }

        public string Strassenname { get; set; }
        public int? Abschnittsnummer { get; set; }
        public string BezeichnungVon { get; set; }
        public string BezeichnungBis { get; set; }
        public string ExternalId { get; set; }
        public string Ortsbezeichnung { get; set; }

        public string StrasseBezeichnungVon { get; set; }
        public string StrasseBezeichnungBis { get; set; }
        public string StrasseOrtsbezeichnung { get; set; }
        public decimal? StrasseLaenge { get; set; }

        public bool HasTrottoir { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = FormatStrings.LongDecimalFormat, NullDisplayText = "")]
        public decimal? Laenge { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = FormatStrings.NoDecimalFormat, NullDisplayText = "")]
        public decimal? FlaecheFahrbahn { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = FormatStrings.NoDecimalFormat, NullDisplayText = "")]
        public decimal? FlaceheTrottoirLinks { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = FormatStrings.NoDecimalFormat, NullDisplayText = "")]
        public decimal? FlaceheTrottoirRechts { get; set; }

        public DateTime? Aufnahmedatum { get; set; }

        public string Aufnahmeteam { get; set; }

        public WetterTyp Wetter { get; set; }

        public string Bemerkung { get; set; }

        public string BemerkungShort { get; set; }

        public string GetStrassenInfo(string von = " von ", string bis = " bis ")
        {
            var label = Strassenname;
            if (StrasseBezeichnungBis.HasText() || StrasseBezeichnungVon.HasText())
            {
                label += ",";
                if (StrasseBezeichnungVon.HasText())
                {
                    label += (von + StrasseBezeichnungVon);
                }

                if (StrasseBezeichnungBis.HasText())
                {
                    label += (bis + StrasseBezeichnungBis);
                }
            }
            return label;
        }

        public bool IsAufnahmedatumInAktuellenErfassungsperiod { get; set; }
    }
}
