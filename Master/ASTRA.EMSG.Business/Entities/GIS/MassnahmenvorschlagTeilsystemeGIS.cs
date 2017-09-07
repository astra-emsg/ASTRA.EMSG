using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Entities.Mapping;
using ASTRA.EMSG.Common.Enums;
using GeoAPI.Geometries;

namespace ASTRA.EMSG.Business.Entities.GIS
{
    [TableShortName("MTG")]
    public class MassnahmenvorschlagTeilsystemeGIS : Entity, IMandantDependentEntity, IAbschnittGISBase
    {
        public virtual string Projektname { get; set; }
        public virtual string BezeichnungVon { get; set; }
        public virtual string BezeichnungBis { get; set; }
        public virtual TeilsystemTyp Teilsystem { get; set; }
        public virtual string ZustaendigeOrganisation { get; set; }
        public virtual string Beschreibung { get; set; }
        public virtual DringlichkeitTyp Dringlichkeit { get; set; }
        public virtual decimal? Kosten { get; set; }
        public virtual StatusTyp Status { get; set; }
        public virtual decimal Laenge { get; set; }

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
