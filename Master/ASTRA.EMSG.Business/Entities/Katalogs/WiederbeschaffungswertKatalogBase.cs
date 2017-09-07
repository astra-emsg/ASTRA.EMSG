using ASTRA.EMSG.Business.Entities.Common;

namespace ASTRA.EMSG.Business.Entities.Katalogs
{
    public class WiederbeschaffungswertKatalogBase : Entity
    {
        public virtual decimal GesamtflaecheFahrbahn { get; set; }
        public virtual decimal FlaecheFahrbahn { get; set; }
        public virtual decimal FlaecheTrottoir { get; set; }

        public virtual decimal AlterungsbeiwertI { get; set; }
        public virtual decimal AlterungsbeiwertII { get; set; }

        public virtual Belastungskategorie Belastungskategorie { get; set; }
        public virtual string BelastungskategorieTyp { get { return Belastungskategorie.Typ; } }
    }
}