using ASTRA.EMSG.Business.Entities.GIS;

namespace ASTRA.EMSG.Business.Entities.Mapping
{
    public class SektorMapping : ClassMapBase<Sektor>
    {
        public SektorMapping()
        {
            ReferencesTo(me => me.AchsenSegment).Index("IDX_SEKTOR_SEGMENT");
            MapTo(s => s.BsId).Index("IDX_SEKTOR_BSID"); ;
            MapTo(s => s.Km);
            MapTo(s => s.SectorLength, "SektorLen");
            MapTo(s => s.Name);
            MapTo(s => s.Sequence);
            MapTo(s => s.MarkerGeom);
            MapTo(s => s.Operation);
            MapTo(s => s.ImpNr);

            ReferencesTo(s => s.CopiedFrom);
        }

        protected override string TableName
        {
            get { return "Sektor"; }
        }

        protected override string PrefixTableName
        {
            get { return PrefixTableNameKlasse.RaumbezugNutzdatentabelle; }
        }
    }
}
