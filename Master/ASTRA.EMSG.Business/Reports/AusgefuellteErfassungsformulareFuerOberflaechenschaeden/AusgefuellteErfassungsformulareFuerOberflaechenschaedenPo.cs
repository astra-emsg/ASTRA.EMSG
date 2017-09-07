using System;
using ASTRA.EMSG.Common.Enums;

namespace ASTRA.EMSG.Business.Reports.AusgefuellteErfassungsformulareFuerOberflaechenschaeden
{
    [Serializable]
    public class AusgefuellteErfassungsformulareFuerOberflaechenschaedenPo
    {
        public Guid ZustandsabschnittId { get; set; }
        public Guid StrassenabschnittId { get; set; }

        public string Strassenname { get; set; }
        public string ZustandsabschnittBezeichnungVon { get; set; }
        public string ZustandsabschnittBezeichnungBis { get; set; }

        public decimal? Laenge { get; set; }
        public decimal? FlaecheFahrbahn { get; set; }
        public DateTime? AufnahmeDatum { get; set; }
        public string Aufnahmeteam { get; set; }
        public WetterTyp? Wetter { get; set; }
        public string WetterBezeichnung { get; set; }
        public string Bemerkung { get; set; }

        public SchadengruppeTyp SchadengruppeTyp { get; set; }
        public string SchadengruppeBezeichnung { get; set; }
        public int SchadengruppeReihung { get; set; }
        
        public SchadendetailTyp SchadendetailTyp { get; set; }
        public string SchadendetailBezeichnung { get; set; }
        public int SchadendetailReihung { get; set; }

        public string SchadenschwereS1 { get; set; }
        public string SchadenschwereS2 { get; set; }
        public string SchadenschwereS3 { get; set; }

        public string SchadenausmassA0 { get; set; }
        public string SchadenausmassA1 { get; set; }
        public string SchadenausmassA2 { get; set; }
        public string SchadenausmassA3 { get; set; }

        public decimal? Matrix { get; set; }
        public decimal Gewicht { get; set; }
        public decimal? Bewertung { get; set; }

        public decimal? Schadensumme { get; set; }
        public decimal? Zustandsindex { get; set; }
    }
}
