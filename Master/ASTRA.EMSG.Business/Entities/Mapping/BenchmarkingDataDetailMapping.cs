using ASTRA.EMSG.Business.Entities.Common;

namespace ASTRA.EMSG.Business.Entities.Mapping
{
    public class BenchmarkingDataDetailMapping : ClassMapBase<BenchmarkingDataDetail>
    {
        public BenchmarkingDataDetailMapping()
        {
            MapTo(b => b.FahrbahnflaecheAnteil, "FbFlAnteil");
            MapTo(b => b.Zustandsindex);
            MapTo(b => b.RealisierteMassnahmenProWiederbeschaffungswertNetz, "ReMaPWbw");

            ReferencesTo(b => b.Belastungskategorie);
            ReferencesTo(b => b.BenchmarkingData);
        }

        protected override string TableName
        {
            get { return "BeDataDet"; }
        }

        protected override string PrefixTableName
        {
            get { return PrefixTableNameKlasse.InventarobjektNutzdatentabelle; }
        }
    }
}