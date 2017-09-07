using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASTRA.EMSG.Common.Enums;

namespace ASTRA.EMSG.Business.Services.GIS.Shape
{
    public static class ZabShapeFileConstants
    {
        public static readonly string StrassenabschnittID = "RDID";
        public static readonly string Strassenname = "RDNAME";
        public static readonly string StrassenabschnittBezeichnungvon = "RDDESFROM";
        public static readonly string StrassenabschnittBezeichnungbis = "RDDESTO";
        public static readonly string Eigentuemer = "OWNER";
        public static readonly string Ortsbezeichnung = "DISTRICT";
        public static readonly string ID = "ID";
        public static readonly string Bezeichnungvon = "DESFROM";
        public static readonly string Bezeichnungbis = "DESTO";
        public static readonly string Laenge = "LENGTH";
        public static readonly string FlaecheFahrbahn = "RDSUR";
        public static readonly string FlaecheTrottoirlinks = "SWLSUR";
        public static readonly string FlaecheTrottoirrechts = "SWRSUR";
        public static readonly string Aufnahmedatum = "RECORDDATE";
        public static readonly string Aufnahmeteam = "RECORDTEAM";
        public static readonly string FBZustandsindex = "RDCONID";
        public static readonly string FBMassnahmenvorschlag = "RDMAINREC";
        public static readonly string FBKosten = "RDCOST";
        public static readonly string FBDringlichkeit = "RDPRIO";
        public static readonly string FBGesamtkosten = "RDTOTCOST";
        public static readonly string TRRZustandsindex = "SWRCONID";
        public static readonly string TRRMassnahmenvorschlag = "SWRMAINREC";
        public static readonly string TRRKosten = "SWRCOST";
        public static readonly string TRRDringlichkeit = "SWRPRIO";
        public static readonly string TRRGesamtkosten = "SWRTOTCOST";
        public static readonly string TRLZustandsindex = "SWLCONID";
        public static readonly string TRLMassnahmenvorschlag = "SWLMAINREC";
        public static readonly string TRLKosten = "SWLCOST";
        public static readonly string TRLDringlichkeit = "SWLPRIO";
        public static readonly string TRLGesamtkosten = "SWLTOTCOST";

        private static readonly string Unknown = "Unknown";
        private static readonly string Urgent = "Urgent";
        private static readonly string Mediumterm = "Mediumterm";
        private static readonly string Longterm = "Longterm";

        private static readonly string Good = "Good";
        private static readonly string Medium = "Medium";
        private static readonly string UnknownZustand = "Unknown";
        private static readonly string Sufficient = "Sufficient";
        private static readonly string Critical = "Critical";
        private static readonly string Bad = "Bad";

        public static string getDringlichkeit(DringlichkeitTyp typ)
        {
            switch (typ)
            {
                case DringlichkeitTyp.Unbekannt:
                    return Unknown;
                case DringlichkeitTyp.Dringlich:
                    return Urgent;
                case DringlichkeitTyp.Mittelfristig:
                    return Mediumterm;
                case DringlichkeitTyp.Langfristig:
                    return Longterm;
                default:
                    return string.Empty;
            }
        }
        public static string getTrottoirZustand(ZustandsindexTyp typ)
        {
            switch (typ)
            {
                case ZustandsindexTyp.Unbekannt:
                    return UnknownZustand;
                case ZustandsindexTyp.Gut:
                    return Good;
                case ZustandsindexTyp.Mittel:
                    return Medium;
                case ZustandsindexTyp.Ausreichend:
                    return Sufficient;
                case ZustandsindexTyp.Kritisch:
                    return Critical;
                case ZustandsindexTyp.Schlecht:
                    return Bad;
                default:
                    return string.Empty;
            }
            
        }
    }
}
