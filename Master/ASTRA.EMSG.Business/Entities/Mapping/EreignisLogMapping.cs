using ASTRA.EMSG.Business.Entities.Common;

namespace ASTRA.EMSG.Business.Entities.Mapping
{
    public class EreignisLogMapping : ClassMapBase<EreignisLog>
    {
        public EreignisLogMapping()
        {
            MapTo(rm => rm.Benutzer);
            MapTo(za => za.Zeit);
            MapTo(za => za.EreignisTyp);
            MapToLongText(za => za.EreignisData);
            MapTo(za => za.MandantName);
        }

        protected override string TableName
        {
            get { return "EreignisLog"; }
        }

        protected override string PrefixTableName
        {
            get { return PrefixTableNameKlasse.InventarobjektNutzdatentabelle; }
        }
    }
}