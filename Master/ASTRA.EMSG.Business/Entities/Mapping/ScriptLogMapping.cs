using ASTRA.EMSG.Business.Entities.Common;

namespace ASTRA.EMSG.Business.Entities.Mapping
{
    public class ScriptLogMapping : ClassMapBase<ScriptLog>
    {
        public ScriptLogMapping()
        {
            MapTo(e => e.ScriptName);
            MapTo(e => e.Version);
            MapTo(e => e.ExecutionDateTime);
        } 

        protected override string TableName
        {
            get { return "ScriptLog"; }
        }

        protected override string PrefixTableName
        {
            get { return PrefixTableNameKlasse.InventarobjektNutzdatentabelle; }
        }
    }
}