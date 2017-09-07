using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Entities.Katalogs;
using ASTRA.EMSG.Common.Enums;

namespace ASTRA.EMSG.Business.Entities.Strassennamen
{
    public abstract class RealisierteMassnahmeBase : Entity, IErfassungsPeriodDependentEntity, IFlaecheFahrbahnUndTrottoirHolder, IBelastungskategorieHolder, IRealisierteMassnahmeKostenHolder, IRealisierteFlaecheHolder
    {
        public virtual string Projektname { get; set; }
        public virtual string BezeichnungVon { get; set; }
        public virtual string BezeichnungBis { get; set; }

        public virtual decimal Laenge { get; set; }
        public virtual decimal BreiteFahrbahn { get; set; }
        public virtual decimal? BreiteTrottoirLinks { get; set; }
        public virtual decimal? BreiteTrottoirRechts { get; set; }
        public virtual EigentuemerTyp Strasseneigentuemer { get; set; }

        public virtual decimal FlaecheFahrbahn { get { return Laenge * BreiteFahrbahn; } }
        public virtual decimal FlaecheTrottoir { get { return FlaecheTrottoirLinks + FlaecheTrottoirRechts; } }
        public virtual decimal FlaecheTrottoirLinks { get { return Laenge * (BreiteTrottoirLinks ?? 0); } }
        public virtual decimal FlaecheTrottoirRechts { get { return Laenge * (BreiteTrottoirRechts ?? 0); } }

        public virtual bool HasTrottoirInformation { get { return BreiteTrottoirLinks.HasValue || BreiteTrottoirRechts.HasValue; } }

        public virtual string Beschreibung { get; set; }
        public virtual decimal? KostenFahrbahn { get; set; }
        public virtual decimal? KostenTrottoirLinks { get; set; }
        public virtual decimal? KostenTrottoirRechts { get; set; }

        public virtual Belastungskategorie Belastungskategorie { get; set; }

        public virtual ErfassungsPeriod ErfassungsPeriod { get; set; }
        public virtual Mandant Mandant { get; set; }

        public virtual decimal Kosten { get { return (KostenFahrbahn ?? 0) + (KostenTrottoirLinks ?? 0) + (KostenTrottoirRechts ?? 0); } }

        public virtual decimal WbwKosten { get { return Kosten; } }

        public virtual MassnahmentypKatalog MassnahmenvorschlagFahrbahn { get; set; }
        public virtual MassnahmentypKatalog MassnahmenvorschlagTrottoir { get; set; }
        public virtual decimal RealisierteFlaeche { get { return FlaecheFahrbahn; } }
    }
}