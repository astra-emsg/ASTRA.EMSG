using ASTRA.EMSG.Business.Entities.Common;

namespace ASTRA.EMSG.Business.Entities.Mapping
{
    public class MandantMapping : ClassMapBase<Mandant>
    {
        public MandantMapping()
        {
            MapTo(e => e.MandantName);
            MapTo(e => e.MandantBezeichnung);
            MapTo(e => e.OwnerId);
        }

        protected override string TableName
        {
            get { return "Mandant"; }
        }

        protected override string PrefixTableName
        {
            get { return PrefixTableNameKlasse.InventarobjektNutzdatentabelle; }
        }
    }
}