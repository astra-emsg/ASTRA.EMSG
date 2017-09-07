using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ASTRA.EMSG.Common
{
    public static class CheckOutGISDefaults
    {
        //Layer
        public const string AchsensegmentLayer = "Achsensegmente";
        public const string StrassenabschnitteLayer = "Strassenabschnitte";
        public const string StrassenabschnittAchsenreferenzenLayer = "StrassenabschnitteAR";
        public const string ZustandsabschnitteLayer = "Zustandsabschnitte";
        public const string ZustandsabschnitteAchsenreferenzenLayer = "ZustandsabschnitteAR";

        //Export Package - DBF Table Field Names
        public const string AchsensegmentID = "ACS_ID";
        public const string IRSStrassenabschnittID = "IRS_ID";
        public const string StrassenabschnittGISID = "STG_ID";
        public const string BelastungsKategorie = "BLK_TYP_VL";
        public const string StrassenabschnittReihenfolge = "IRS_RFolge";
        public const string AchsenreferenzID = "ACR_ID";
        public const string ReferenzgruppeID = "RFG_ID";
        public const string ZustandsabschnittID = "ZSG_ID";
        public const string InspektionsRouteID = "IRG_ID";
        public const string ZustandsIndex = "ZST_INDEX";

        public const string IsEdited = "EDITED";
        public const string IsAdded = "ADDED";
        public const string IsDeleted = "DELETED";
    }
}
