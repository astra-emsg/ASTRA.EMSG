using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Entities.Mapping;
using ASTRA.EMSG.Common.Enums;

namespace ASTRA.EMSG.Business.Entities.Katalogs
{
    public class MassnahmenvorschlagKatalogBase : Entity
    {
        public virtual MassnahmentypKatalog Parent { get; set; }
        public virtual string Typ { get { return Parent.Typ; } }
        public virtual MassnahmenvorschlagKatalogTyp KatalogTyp { get { return Parent.KatalogTyp; } }
        
        public virtual decimal DefaultKosten { get; set; }
        public virtual Belastungskategorie Belastungskategorie { get; set; }
        public virtual string BelastungskategorieTyp { get { return Belastungskategorie.Typ; } }
    }

    [TableShortName("MTK")]
    public class MassnahmentypKatalog : Entity
    {
        public virtual string Typ { get; set; }
        public virtual MassnahmenvorschlagKatalogTyp KatalogTyp { get; set; }
        public virtual int LegendNumber { get; set; }
    }
}