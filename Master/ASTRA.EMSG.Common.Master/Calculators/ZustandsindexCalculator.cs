using System;
using ASTRA.EMSG.Common.Enums;

namespace ASTRA.EMSG.Common.Master.Calculators
{
    public static class ZustandsindexCalculator
    {
        //0.0-0.9 – gut
        //1.0-1.9 – mittel
        //2.0-2.9 – ausreichend
        //3.0-3.9 – kritisch
        //4.0-5.0 – schlecht

        public static decimal GetWert(ZustandsindexTyp zustandsindexTrottoirTyp)
        {
            switch (zustandsindexTrottoirTyp)
            {
                case ZustandsindexTyp.Gut:
                    return 0m;
                case ZustandsindexTyp.Mittel:
                    return 1m;
                case ZustandsindexTyp.Ausreichend:
                    return 2m;
                case ZustandsindexTyp.Kritisch:
                    return 3m;
                case ZustandsindexTyp.Schlecht:
                    return 4m;
                default:
                    throw new ArgumentOutOfRangeException("zustandsindexTrottoirTyp", zustandsindexTrottoirTyp, "Value is out of range!");
            }
        }

        public static ZustandsindexTyp GetTyp(decimal? zustandsindex)
        {
            if (zustandsindex == null)
                return ZustandsindexTyp.Unbekannt;

            if (zustandsindex < 0m)
                throw new ArgumentOutOfRangeException("zustandsindex", zustandsindex, "Value is out of range!");
            if (zustandsindex < 1m)
                return ZustandsindexTyp.Gut;
            if (zustandsindex < 2m)
                return ZustandsindexTyp.Mittel;
            if (zustandsindex < 3m)
                return ZustandsindexTyp.Ausreichend;
            if (zustandsindex < 4m)
                return ZustandsindexTyp.Kritisch;
            if (zustandsindex <= 5m)
                return ZustandsindexTyp.Schlecht;
            
            throw new ArgumentOutOfRangeException("zustandsindex", zustandsindex, "Value is out of range!");
        }

        public static string ToColorCode(this ZustandsindexTyp zustandsindexTyp)
        {
            switch (zustandsindexTyp)
            {
                case ZustandsindexTyp.Unbekannt:
                    return "#9C9C9C";
                case ZustandsindexTyp.Gut:
                    return "#1CCF34";
                case ZustandsindexTyp.Mittel:
                    return "#F4F42D";
                case ZustandsindexTyp.Ausreichend:
                    return "#F2A232";
                case ZustandsindexTyp.Kritisch:
                    return "#EF0000";
                case ZustandsindexTyp.Schlecht:
                    return "#48024C";
                default:
                    throw new ArgumentOutOfRangeException("zustandsindexTyp");
            }
        }
    }
}
