using ASTRA.EMSG.Business.Entities.Katalogs;
using ASTRA.EMSG.Business.Entities.Mapping;

namespace ASTRA.EMSG.Business.Entities.Common
{
    [TableShortName("KFD")]
    public class KenngroessenFruehererJahreDetail : Entity, IMandantDependentEntity, IBelastungskategorieHolder, IFlaecheFahrbahnUndTrottoirHolder, IRealisierteMassnahmeKostenHolder, IRealisierteFlaecheHolder
    {
        public virtual decimal? MittlererZustand { get; set; }

        public virtual decimal Fahrbahnlaenge { get; set; }
        public virtual int Fahrbahnflaeche { get; set; }

        public virtual decimal FlaecheFahrbahn { get { return Fahrbahnflaeche; } }
        public virtual decimal FlaecheTrottoir { get { return 0; } }
        public virtual bool HasTrottoirInformation { get { return false; } }

        public virtual KenngroessenFruehererJahre KenngroessenFruehererJahre { get; set; }

        public virtual Belastungskategorie Belastungskategorie { get; set; }

        public virtual string BelastungskategorieTyp { get { return Belastungskategorie == null ? null : Belastungskategorie.Typ; } }

        public virtual Mandant Mandant
        {
            get { return KenngroessenFruehererJahre.Mandant; }
            set { /*NOP */ }
        }

        public virtual decimal Kosten { get { return KenngroessenFruehererJahre.KostenFuerWerterhaltung / KenngroessenFruehererJahre.KenngroesseFruehereJahrDetails.Count; } }

        public virtual decimal WbwKosten { get { return 0; } }
        public virtual decimal RealisierteFlaeche { get { return 0; } }
    }
}