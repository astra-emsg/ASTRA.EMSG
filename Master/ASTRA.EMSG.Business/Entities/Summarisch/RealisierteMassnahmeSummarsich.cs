using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Entities.Katalogs;
using ASTRA.EMSG.Business.Entities.Mapping;
using ASTRA.EMSG.Common.Enums;

namespace ASTRA.EMSG.Business.Entities.Summarisch
{
    [TableShortName("RMS")]
    public class RealisierteMassnahmeSummarsich : Entity, IErfassungsPeriodDependentEntity, IFlaecheFahrbahnUndTrottoirHolder, IBelastungskategorieHolder, IRealisierteMassnahmeKostenHolder, IRealisierteFlaecheHolder
    {
        public virtual string Projektname { get; set; }
        public virtual string Beschreibung { get; set; }
        public virtual int? KostenFahrbahn { get; set; }

        public virtual int Fahrbahnflaeche { get; set; }

        public virtual decimal FlaecheFahrbahn { get { return Fahrbahnflaeche; } }
        public virtual decimal FlaecheTrottoir { get { return 0; } }
        public virtual bool HasTrottoirInformation { get { return false; } }
        public virtual EigentuemerTyp Strasseneigentuemer { get; set; }

        public virtual Belastungskategorie Belastungskategorie { get; set; }
        public virtual string BelastungskategorieTyp { get { return Belastungskategorie == null ? null : Belastungskategorie.Typ; } }

        public virtual ErfassungsPeriod ErfassungsPeriod { get; set; }

        public virtual Mandant Mandant { get; set; }

        public virtual decimal Kosten { get { return KostenFahrbahn ?? 0; } }

        public virtual decimal WbwKosten { get { return Kosten; } }
        public virtual decimal RealisierteFlaeche { get { return FlaecheFahrbahn; } }
    }
}