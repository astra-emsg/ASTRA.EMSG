using System;
using System.Collections.Generic;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Entities.Katalogs;
using ASTRA.EMSG.Business.Entities.Mapping;
using ASTRA.EMSG.Common.Enums;
using GeoAPI.Geometries;

namespace ASTRA.EMSG.Business.Entities.GIS
{
    [TableShortName("KMG")]
    public class KoordinierteMassnahmeGIS : Entity, IMandantDependentEntity, IAbschnittGISBase
    {
        public virtual string Projektname { get; set; }
        public virtual string BezeichnungVon { get; set; }
        public virtual string BezeichnungBis { get; set; }
        public virtual decimal Laenge { get; set; }

        public virtual decimal BreiteFahrbahn { get; set; }
        public virtual decimal? BreiteTrottoirLinks { get; set; }
        public virtual decimal? BreiteTrottoirRechts { get; set; }

        public virtual decimal FlaecheFahrbahn { get { return Laenge * BreiteFahrbahn; } }
        public virtual decimal? FlaecheTrottoirLinks { get { return Laenge * BreiteTrottoirLinks; } }
        public virtual decimal? FlaecheTrottoirRechts { get { return Laenge * BreiteTrottoirRechts; } }

        public virtual MassnahmentypKatalog MassnahmenvorschlagFahrbahn { get; set; }

        public virtual IList<TeilsystemTyp> BeteiligteSysteme { get; set; }

        public virtual decimal? KostenGesamtprojekt { get; set; }
        public virtual decimal? KostenFahrbahn { get; set; }
        public virtual decimal? KostenTrottoirLinks { get; set; }
        public virtual decimal? KostenTrottoirRechts { get; set; }

        public virtual decimal? KostenStrasse
        {
            get
            {
                if (!KostenFahrbahn.HasValue && !KostenTrottoirLinks.HasValue && !KostenTrottoirRechts.HasValue)
                    return null;

                return (KostenFahrbahn ?? 0) + (KostenTrottoirLinks ?? 0) + (KostenTrottoirRechts ?? 0);
            }
        }

        public virtual string Beschreibung { get; set; }
        public virtual DateTime? AusfuehrungsAnfang { get; set; }
        public virtual DateTime? AusfuehrungsEnde { get; set; }

        public virtual string LeitendeOrganisation { get; set; }
        public virtual StatusTyp Status { get; set; }

        public virtual IGeometry Shape { get; set; }

        //public List<AchsenReferenz> AchsenReferenzList { get; set; }
        public virtual ReferenzGruppe ReferenzGruppe { get; set; }

        public virtual Mandant Mandant { get; set; }

        public virtual string DisplayName
        {
            get { return Projektname + " (" + BezeichnungVon + " - " + BezeichnungBis + ")"; }
        }
    }
}