using System;
using System.Collections.Generic;
using System.Linq;
using ASTRA.EMSG.Business.Entities.Katalogs;
using ASTRA.EMSG.Common.Enums;

namespace ASTRA.EMSG.Business.Entities.Common
{
    public abstract class ZustandsabschnittBase : Entity, IZustandsabschnitt
    {
        protected ZustandsabschnittBase()
        {
            Schadengruppen = new List<Schadengruppe>();
            Schadendetails = new List<Schadendetail>();
        }

        public abstract string Strassenname { get; }

        public virtual int? Abschnittsnummer { get; set; }
        public virtual string BezeichnungVon { get; set; }
        public virtual string BezeichnungBis { get; set; }
        public virtual string ExternalId { get; set; }

        public virtual decimal FlaecheTrottoir { get { return (FlaceheTrottoirLinks ?? 0) + (FlaceheTrottoirRechts ?? 0); } }
        public virtual bool HasTrottoirInformation { get { return StrassenabschnittBase.HasTrottoirInformation; } }

        public virtual bool IstDetaillierteSchadenserfassungsformular { get { return Erfassungsmodus == ZustandsErfassungsmodus.Detail; } }

        public virtual Belastungskategorie Belastungskategorie { get { return StrassenabschnittBase.Belastungskategorie; } }
        public virtual string BelastungskategorieTyp { get { return StrassenabschnittBase.BelastungskategorieTyp; } }
        public virtual BelagsTyp Belag { get { return StrassenabschnittBase.Belag; } }
        public abstract StrassenabschnittBase StrassenabschnittBase { get; }

        public virtual decimal Zustandsindex { get; set; }
        public virtual ZustandsErfassungsmodus Erfassungsmodus { get; set; }
        public virtual decimal Laenge { get; set; }
        public virtual DateTime Aufnahmedatum { get; set; }
        public virtual string Aufnahmeteam { get; set; }
        public virtual WetterTyp Wetter { get; set; }
        public virtual string Bemerkung { get; set; }
        public virtual ZustandsindexTyp ZustandsindexTrottoirLinks { get; set; }
        public virtual ZustandsindexTyp ZustandsindexTrottoirRechts { get; set; }

        public virtual bool IsAufnahmedatumInAktuellenErfassungsperiod { get { return Aufnahmedatum.Year >= ErfassungsPeriod.Erfassungsjahr.Year; } }

        public virtual bool HasTrottoir
        {
            get
            {
                return StrassenabschnittBase.Trottoir != TrottoirTyp.NochNichtErfasst && StrassenabschnittBase.Trottoir != TrottoirTyp.KeinTrottoir;
            }
        }

        public virtual decimal? FlaecheFahrbahn
        {
            get
            {
                decimal flaeche = Laenge * StrassenabschnittBase.BreiteFahrbahn;
                return flaeche == 0 ? (decimal?)null : flaeche;
            }
        }

        decimal IFlaecheFahrbahnUndTrottoirHolder.FlaecheFahrbahn { get { return FlaecheFahrbahn ?? 0; } }

        public virtual decimal? FlaceheTrottoirLinks { get { return Laenge * StrassenabschnittBase.BreiteTrottoirLinks; } }
        public virtual decimal? FlaceheTrottoirRechts { get { return Laenge * StrassenabschnittBase.BreiteTrottoirRechts; } }

        public virtual Guid? MassnahmenvorschlagKatalogId { get { return MassnahmenvorschlagFahrbahn == null ? (Guid?)null : MassnahmenvorschlagFahrbahn.Id; } }

        public virtual DringlichkeitTyp Dringlichkeit { get { return DringlichkeitFahrbahn; } }
        public virtual decimal? Kosten { get { return KostenMassnahmenvorschlagFahrbahn; } }

        public virtual decimal KostenFahrbahn { get { return GetKosten(KostenMassnahmenvorschlagFahrbahn, MassnahmenvorschlagFahrbahn, FlaecheFahrbahn); } }
        public virtual decimal KostenTrottoirLinks { get { return GetKosten(KostenMassnahmenvorschlagTrottoirLinks, MassnahmenvorschlagTrottoirLinks, FlaceheTrottoirLinks); } }
        public virtual decimal KostenTrottoirRechts { get { return GetKosten(KostenMassnahmenvorschlagTrottoirRechts, MassnahmenvorschlagTrottoirRechts, FlaceheTrottoirRechts); } }

        public virtual decimal? KostenMassnahmenvorschlagFahrbahn { get; set; }
        public virtual decimal? KostenMassnahmenvorschlagTrottoirLinks { get; set; }
        public virtual decimal? KostenMassnahmenvorschlagTrottoirRechts { get; set; }

        public virtual MassnahmenvorschlagKatalog MassnahmenvorschlagFahrbahn { get; set; }
        public virtual MassnahmenvorschlagKatalog MassnahmenvorschlagTrottoirLinks { get; set; }
        public virtual MassnahmenvorschlagKatalog MassnahmenvorschlagTrottoirRechts { get; set; }

        //Used by the MassnahmenvorschlagProZustandsabschnitt report
        public virtual string MassnahmenvorschlagKatalogTypFahrbahn { get { return MassnahmenvorschlagFahrbahn == null ? null : MassnahmenvorschlagFahrbahn.Typ; } }
        public virtual string MassnahmenvorschlagKatalogTypTrottoirLinks { get { return MassnahmenvorschlagTrottoirLinks == null ? null : MassnahmenvorschlagTrottoirLinks.Typ; } }
        public virtual string MassnahmenvorschlagKatalogTypTrottoirRechts { get { return MassnahmenvorschlagTrottoirRechts == null ? null : MassnahmenvorschlagTrottoirRechts.Typ; } }


        public virtual DringlichkeitTyp DringlichkeitFahrbahn { get; set; }
        public virtual DringlichkeitTyp DringlichkeitTrottoirLinks { get; set; }
        public virtual DringlichkeitTyp DringlichkeitTrottoirRechts { get; set; }

        public abstract string InspektionsroutenName { get; }

        public virtual IList<Schadengruppe> Schadengruppen { get; set; }
        public virtual IList<Schadendetail> Schadendetails { get; set; }

        private decimal GetKosten(decimal? kostenMassnahmenvorschlag, MassnahmenvorschlagKatalog massnahmenvorschlag, decimal? flacehe)
        {
            if (kostenMassnahmenvorschlag.HasValue)
                return kostenMassnahmenvorschlag.Value * (flacehe ?? 0);
            if (massnahmenvorschlag != null)
                return massnahmenvorschlag.DefaultKosten * (flacehe ?? 0);
            return 0;
        }

        public virtual Mandant Mandant
        {
            get { return StrassenabschnittBase.Mandant; }
            set { /*NOP */ }
        }

        public virtual ErfassungsPeriod ErfassungsPeriod
        {
            get { return StrassenabschnittBase.ErfassungsPeriod; }
            set { /*NOP */ }
        }

        public virtual void AddSchadengruppe(Schadengruppe schadengruppe)
        {
            Schadengruppen.Add(schadengruppe);
        }

        public virtual void RemoveSchadengruppe(Schadengruppe schadengruppe)
        {
            Schadengruppen.Remove(schadengruppe);
        }

        public virtual void AddSchadendetail(Schadendetail schadendetail)
        {
            Schadendetails.Add(schadendetail);
        }

        public virtual void RemoveSchadendetail(Schadendetail schadendetail)
        {
            Schadendetails.Remove(schadendetail);
        }

        public virtual void DeleteSchadenFormData()
        {
            foreach (var schadendetail in Schadendetails.ToList())
                RemoveSchadendetail(schadendetail);

            foreach (var schadengruppe in Schadengruppen.ToList())
                RemoveSchadengruppe(schadengruppe);

            Zustandsindex = 0.0m;
        }
    }
}