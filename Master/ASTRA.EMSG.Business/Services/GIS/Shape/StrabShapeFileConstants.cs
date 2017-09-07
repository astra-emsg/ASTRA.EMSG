using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASTRA.EMSG.Common.Enums;

namespace ASTRA.EMSG.Business.Services.GIS.Shape
{
    public static class StrabShapeFileConstants
    {
        public static readonly string ID = "ID";
        public static readonly string Strassenname = "RDNAME";
        public static readonly string Bezeichnungvon = "DESFROM";
        public static readonly string Bezeichnungbis = "DESTO";
        public static readonly string Eigentuemer = "OWNER";
        public static readonly string Ortsbezeichnung = "DISTRICT";
        public static readonly string Belastungskategorie = "LOADCAT";
        public static readonly string Belag = "PAVEMENT";
        public static readonly string BreiteFahrbahn = "RDWIDTH";
        public static readonly string Laenge = "LENGTH";
        public static readonly string FlaecheFahrbahn = "RDSUR";
        public static readonly string Trottoir = "SIDEWALK";
        public static readonly string BreiteTrottoirlinks = "SWLWIDTH";
        public static readonly string FlaecheTrottoirlinks = "SWLSUR";
        public static readonly string BreiteTrottoirrechts = "SWRWIDTH";
        public static readonly string FlaecheTrottoirrechts = "SWRSUR";
        public static readonly string FlaecheTrottoir = "SWSUR";
        public static readonly string Wiederbeschaffungswert = "REPVARD";
        public static readonly string AlterungsbeiwertI = "AGECOEFFI";
        public static readonly string WertverlustI = "VALOSSI";
        public static readonly string AlterungsbeiwertII = "AGECOEFFII";
        public static readonly string WertverlustII = "VALOSSII";

        private static readonly string IA = "IA";
        private static readonly string IB = "IB";
        private static readonly string IC = "IC";
        private static readonly string II = "II";
        private static readonly string III = "III";
        private static readonly string IV = "IV";
        private static readonly string Cobblers = "Cobblers";
        private static readonly string Macadamized = "Macadamized";
        private static readonly string Custom1 = "Custom1";
        private static readonly string Custom2 = "Custom2";
        private static readonly string Custom3 = "Custom3";

        private static readonly string Community = "Community";
        private static readonly string Private = "Private";
        private static readonly string Canton = "Canton";
        private static readonly string Corporation = "Corporation";

        private static readonly string SurfaceAsphalt = "Asphalt";
        private static readonly string SurfaceConcrete = "Concrete";
        private static readonly string SurfaceCobblers = "Cobblers";
        private static readonly string SurfaceMacadamized = "Macadamized";

        private static readonly string Notrecorded = "Notrecorded";
        private static readonly string None = "None";
        private static readonly string Left = "Left";
        private static readonly string Right = "Right";
        private static readonly string Both = "Both";

        public static string getOwner(EigentuemerTyp owner)
        {
            switch (owner)
            {
                case EigentuemerTyp.Gemeinde:
                    return Community;
                case EigentuemerTyp.Privat:
                    return Private;
                case EigentuemerTyp.Kanton:
                    return Canton;
                case EigentuemerTyp.Korporation:
                    return Corporation;
                default:
                    return string.Empty;                    
            }
        }

        public static string getBelag(BelagsTyp typ)
        {
            switch (typ)
            {
                case BelagsTyp.Asphalt:
                    return SurfaceAsphalt;
                case BelagsTyp.Beton:
                    return SurfaceConcrete;
                case BelagsTyp.Pflaesterung:
                    return SurfaceCobblers;
                case BelagsTyp.Chaussierung:
                    return SurfaceMacadamized;
                default:
                    return string.Empty;
            }
        }

        public static string getTrottoirTyp(TrottoirTyp typ)
        {
            switch (typ)
            {
                case TrottoirTyp.NochNichtErfasst:
                    return Notrecorded;
                case TrottoirTyp.KeinTrottoir:
                    return None;
                case TrottoirTyp.Links:
                    return Left;
                case TrottoirTyp.Rechts:
                    return Right;
                case TrottoirTyp.BeideSeiten:
                    return Both;
                default:
                    return string.Empty;
            }
        }

        public static string getBelastungsKategorie(string belkat)
        {
            switch (belkat)
            {
                case "IA":
                    return IA;
                case "IB":
                    return IB;
                case "IC":
                    return IC;
                case "II":
                    return II;
                case "III":
                    return III;
                case "IV":
                    return IV;
                case "Pflaesterung":
                    return Cobblers;
                case "Chaussierung":
                    return Macadamized;
                case "Benutzerdefiniert1":
                    return Custom1;
                case "Benutzerdefiniert2":
                    return Custom2;
                case "Benutzerdefiniert3":
                    return Custom3;
                default:
                    return string.Empty;
            }
        }

    }
}
