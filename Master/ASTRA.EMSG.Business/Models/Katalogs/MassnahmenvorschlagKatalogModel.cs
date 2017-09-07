using System;
using ASTRA.EMSG.Common;
using ASTRA.EMSG.Common.Enums;

namespace ASTRA.EMSG.Business.Models.Katalogs
{
    public class BLKIndependentMassnahmenvorschlagKatalogModel : Model
    {
        public string Typ { get; set; }
        public string TypBezeichnung { get; set; }
        public MassnahmenvorschlagKatalogTyp KatalogTyp { get; set; }
        public Guid? Mandant { get; set; }
    }

    public class MassnahmenvorschlagKatalogModel : BLKIndependentMassnahmenvorschlagKatalogModel
    {
        public decimal DefaultKosten { get; set; }
    }
}