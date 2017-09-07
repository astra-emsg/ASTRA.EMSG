using ASTRA.EMSG.Common.DataTransferObjects;
using ASTRA.EMSG.Common.Enums;
using ASTRA.EMSG.Common.Services.SchadenMetadaten;
using ASTRA.EMSG.Localization;

namespace ASTRA.EMSG.Mobile.ViewModels
{
    public class GrobItemViewModel : ViewModel
    {
        public GrobItemViewModel(SchadengruppeMetadaten schadengruppeMetadaten, SchadengruppeDTO schadengruppeDto, bool isZebraColored)
        {
            if (schadengruppeDto != null)
            {
                SchadenschwereTyp = schadengruppeDto.SchadenschwereTyp;
                SchadenausmassTyp = schadengruppeDto.SchadenausmassTyp;
            }
            else
            {
                SchadenschwereTyp = SchadenschwereTyp.S1;
                SchadenausmassTyp = SchadenausmassTyp.A0;
            }

            Gewicht = schadengruppeMetadaten.Gewicht;
            SchadengruppeTyp = schadengruppeMetadaten.SchadengruppeTyp;

            SchadengruppeBezeichnung = LocalizationLocator.MobileLocalization.GetSchadengruppeBezeichnung(schadengruppeMetadaten.SchadengruppeTyp);

            IsZebraColored = isZebraColored;
        }

        private SchadengruppeTyp schadengruppeTyp;
        public SchadengruppeTyp SchadengruppeTyp { get { return schadengruppeTyp; } set { schadengruppeTyp = value; Notify(() => SchadengruppeTyp); } }

        private string schadengruppeBezeichnung;
        public string SchadengruppeBezeichnung { get { return schadengruppeBezeichnung; } set { schadengruppeBezeichnung = value; Notify(() => SchadengruppeBezeichnung); } }

        private SchadenschwereTyp schadenschwereTyp;
        public SchadenschwereTyp SchadenschwereTyp
        {
            get { return schadenschwereTyp; }
            set
            {
                schadenschwereTyp = value;

                Notify(() => S1);
                Notify(() => S2);
                Notify(() => S3);

                Notify(() => Matrix);
                Notify(() => Bewertung);
            }
        }

        public bool S1
        {
            get { return SchadenschwereTyp == SchadenschwereTyp.S1; }
            set
            {
                if (value)
                    SchadenschwereTyp = SchadenschwereTyp.S1;
                Notify(() => S1);
            }
        }

        public bool S2
        {
            get { return SchadenschwereTyp == SchadenschwereTyp.S2; }
            set
            {
                if (value)
                    SchadenschwereTyp = SchadenschwereTyp.S2;
                Notify(() => S2);
            }
        }

        public bool S3
        {
            get { return SchadenschwereTyp == SchadenschwereTyp.S3; }
            set
            {
                if (value)
                    SchadenschwereTyp = SchadenschwereTyp.S3;
                Notify(() => S3);
            }
        }

        private SchadenausmassTyp schadenausmassTyp;
        public SchadenausmassTyp SchadenausmassTyp
        {
            get { return schadenausmassTyp; }
            set
            {
                schadenausmassTyp = value;

                Notify(() => A0);
                Notify(() => A1);
                Notify(() => A2);
                Notify(() => A3);

                Notify(() => Matrix);
                Notify(() => Bewertung);
            }
        }

        public bool A0
        {
            get { return SchadenausmassTyp == SchadenausmassTyp.A0; }
            set
            {
                if (value)
                    SchadenausmassTyp = SchadenausmassTyp.A0;
                Notify(() => A0);
            }
        }

        public bool A1
        {
            get { return SchadenausmassTyp == SchadenausmassTyp.A1; }
            set
            {
                if (value)
                    SchadenausmassTyp = SchadenausmassTyp.A1;
                Notify(() => A1);
            }
        }

        public bool A2
        {
            get { return SchadenausmassTyp == SchadenausmassTyp.A2; }
            set
            {
                if (value)
                    SchadenausmassTyp = SchadenausmassTyp.A2;
                Notify(() => A2);
            }
        }

        public bool A3
        {
            get { return SchadenausmassTyp == SchadenausmassTyp.A3; }
            set
            {
                if (value)
                    SchadenausmassTyp = SchadenausmassTyp.A3;
                Notify(() => A3);
            }
        }

        public int Matrix { get { return (int)SchadenschwereTyp * (int)SchadenausmassTyp; } }

        private int gewicht;
        public int Gewicht { get { return gewicht; } set { gewicht = value; Notify(() => Gewicht); } }

        public int Bewertung { get { return Matrix * Gewicht; } }

        private bool isZebraColored;
        public bool IsZebraColored { get { return isZebraColored; } set { isZebraColored = value; Notify(() => IsZebraColored); } }
    }
}
