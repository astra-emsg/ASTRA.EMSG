using System;
using System.Collections.Generic;
using ASTRA.EMSG.Common;
using ASTRA.EMSG.Common.Enums;

namespace ASTRA.EMSG.Business.Models.Katalogs
{
    public class MassnahmenvorschlagKatalogAdminModel : Model
    {
        public string Typ { get; set; }
        public MassnahmenvorschlagKatalogTyp KatalogTyp { get; set; }
        public IList<MassnahmenvorschlagKatalogKonstenEditModel> KonstenModels { get; set; }
    }

    public class MassnahmenvorschlagKatalogEditModel : MassnahmenvorschlagKatalogAdminModel
    {
    }

    public class MassnahmenvorschlagKatalogCreateModel : MassnahmenvorschlagKatalogAdminModel
    {
    }

    public class MassnahmenvorschlagKatalogOverviewModel : MassnahmenvorschlagKatalogAdminModel
    {
        public string TypBezeichnung { get; set; }
        public string KatalogTypBezeichnung { get; set; }
        public bool IsUsed { get; set; }
    }

    public class MassnahmenvorschlagKatalogKonstenEditModel : Model
    {
        public decimal? DefaultKosten { get; set; }
        public Guid Belastungskategorie { get; set; }
        public string BelastungskategorieBezeichnung { get; set; }
    }
}
