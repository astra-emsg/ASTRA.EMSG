using System;
using System.Globalization;
using ASTRA.EMSG.Common.Enums;

namespace ASTRA.EMSG.Web.Infrastructure
{
    public static class EmsgLanguageExtensions
    {
        public static CultureInfo ToCultureInfo(this EmsgLanguage? emsgLanguage)
        {
            if(!emsgLanguage.HasValue)
                return EmsgLanguage.Ch.ToCultureInfo();

            return emsgLanguage.Value.ToCultureInfo();
        }

        public static CultureInfo ToCultureInfo(this EmsgLanguage emsgLanguage)
        {
            //alway use the de-ch formatting according TFS8338: Bitte in allen drei Sprachen die Zahlen mit de-ch formatieren!

            CultureInfo deChCulture = CultureInfo.CreateSpecificCulture("de-ch");
            CultureInfo resultCulture;
            switch (emsgLanguage)
            {
                case EmsgLanguage.Ch:
                    resultCulture = deChCulture;
                    break;
                case EmsgLanguage.Fr:
                    resultCulture = CultureInfo.CreateSpecificCulture("fr-ch");
                    resultCulture.NumberFormat = deChCulture.NumberFormat;
                    break;
                case EmsgLanguage.It:
                    resultCulture = CultureInfo.CreateSpecificCulture("it-ch");
                    resultCulture.NumberFormat = deChCulture.NumberFormat;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("emsgLanguage");
            }

            return resultCulture;
        }
    }
}