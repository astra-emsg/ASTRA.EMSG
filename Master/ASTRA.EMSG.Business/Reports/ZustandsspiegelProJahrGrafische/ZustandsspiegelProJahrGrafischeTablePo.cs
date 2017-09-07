using System;
using ASTRA.EMSG.Business.Reporting;
using ASTRA.EMSG.Common.Enums;

namespace ASTRA.EMSG.Business.Reports.ZustandsspiegelProJahrGrafische
{
    public class ZustandsspiegelProJahrGrafischeTablePo : IPercentHolder
    {
        public string Value { get { return Format(DecimalValue); } }
        public Func<decimal?, string> Format { get; set; }

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

        public ZustandsindexTyp ZustandsindexTyp { get; set; }
        public string ZustandsindexTypBezeichnung { get; set; }

        public int SortOrder { get; set; }

        public string LegendImageUrl { get; set; }

        public decimal? DecimalValue { get; set; }
        public string Group { get { return JahrBezeichnung; } }
    }
}