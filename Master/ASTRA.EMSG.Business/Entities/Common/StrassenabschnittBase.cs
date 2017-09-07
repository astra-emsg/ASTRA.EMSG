using ASTRA.EMSG.Business.Entities.Katalogs;
using ASTRA.EMSG.Common.Enums;

namespace ASTRA.EMSG.Business.Entities.Common
{
    public abstract class StrassenabschnittBase : Entity, IStrassenabschnitt, IFlaecheProvider
    {
        public virtual string Strassenname { get; set; }
        public virtual int? Abschnittsnummer { get; set; }
        public virtual string BezeichnungVon { get; set; }
        public virtual string BezeichnungBis { get; set; }

        public virtual Belastungskategorie Belastungskategorie { get; set; }
        public virtual string BelastungskategorieTyp { get { return Belastungskategorie == null ? string.Empty : Belastungskategorie.Typ; } }
        public virtual BelagsTyp Belag { get; set; }
        public virtual decimal Laenge { get; set; }
        public virtual decimal BreiteFahrbahn { get; set; }
        public virtual TrottoirTyp Trottoir { get; set; }
        public virtual decimal? BreiteTrottoirLinks { get; set; }
        public virtual decimal? BreiteTrottoirRechts { get; set; }
        public virtual EigentuemerTyp Strasseneigentuemer { get; set; }
        public virtual string Ortsbezeichnung { get; set; }

        public virtual Mandant Mandant { get; set; }
        public virtual ErfassungsPeriod ErfassungsPeriod { get; set; }

        public virtual decimal GesamtFlaeche { get { return this.GesamtFlaeche(); } }
        public virtual decimal FlaecheTrottoir { get { return this.FlaecheTrottoir(); } }
        public virtual decimal FlaecheFahrbahn { get { return this.FlaecheFahrbahn(); } }
        public virtual decimal FlaecheTrottoirLinks { get { return this.FlaecheTrottoirLinks(); } }
        public virtual decimal FlaecheTrottoirRechts { get { return this.FlaecheTrottoirRechts(); } }

        public virtual bool HasTrottoir { get { return this.HasTrottoir(); } }
        public virtual bool HasTrottoirInformation { get { return this.HasTrottoirInformation(); } }
        public abstract bool ShouldCheckBelagChange { get; }
    }
}