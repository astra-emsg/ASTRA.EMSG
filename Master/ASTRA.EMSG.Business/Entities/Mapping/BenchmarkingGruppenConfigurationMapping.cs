using ASTRA.EMSG.Business.Entities.Common;

namespace ASTRA.EMSG.Business.Entities.Mapping
{
    public class BenchmarkingGruppenConfigurationMapping : ClassMapBase<BenchmarkingGruppenConfiguration>
    {
        public BenchmarkingGruppenConfigurationMapping()
        {
            MapTo(s => s.EigenschaftTyp);
            MapTo(s => s.Grenzwert);
        }

        protected override string TableName
        {
            get { return "BenchGrCfg"; }
        }

        protected override string PrefixTableName
        {
            get { return PrefixTableNameKlasse.InventarobjektNutzdatentabelle; }
        }
    }
}