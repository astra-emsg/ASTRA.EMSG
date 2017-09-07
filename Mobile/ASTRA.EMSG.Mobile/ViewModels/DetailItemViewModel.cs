using ASTRA.EMSG.Common.DataTransferObjects;
using ASTRA.EMSG.Common.Enums;
using ASTRA.EMSG.Common.Services.SchadenMetadaten;
using ASTRA.EMSG.Localization;

namespace ASTRA.EMSG.Mobile.ViewModels
{
    public class DetailItemViewModel : ViewModel
    {
        public DetailItemViewModel(SchadendetailMetadaten schadendetailMetadaten, SchadendetailDTO schadendetailDto, bool isZebraColored)
        {
            if (schadendetailDto != null)
            {
                SchadenschwereTyp = schadendetailDto.SchadenschwereTyp;
                SchadenausmassTyp = schadendetailDto.SchadenausmassTyp;
            }
            else
            {
                SchadenschwereTyp = SchadenschwereTyp.S1;
                SchadenausmassTyp = SchadenausmassTyp.A0;
            }

            SchadendetailTyp = schadendetailMetadaten.SchadendetailTyp;

            SchadendetailBezeichnung = LocalizationLocator.MobileLocalization.GetSchadendetailBezeichnung(schadendetailMetadaten.SchadendetailTyp);
            
            IsZebraColored = isZebraColored;
        }

        private SchadendetailTyp schadendetailTyp;
        public SchadendetailTyp SchadendetailTyp { get { return schadendetailTyp; } set { schadendetailTyp = value; Notify(() => SchadendetailTyp); } }

        private string schadendetailBezeichnung;
        public string SchadendetailBezeichnung { get { return schadendetailBezeichnung; } set { schadendetailBezeichnung = value; Notify(() => SchadendetailBezeichnung); } }

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

        private bool isZebraColored;
        public bool IsZebraColored { get { return isZebraColored; } set { isZebraColored = value; Notify(() => IsZebraColored); } }
    }
}