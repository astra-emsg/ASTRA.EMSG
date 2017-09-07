using ASTRA.EMSG.Business.Entities.Common;

namespace ASTRA.EMSG.Business.Entities.Mapping
{
    public class BenchmarkingDataMapping : ClassMapBase<BenchmarkingData>
    {
        public BenchmarkingDataMapping()
        {
            MapTo(b => b.FahrbahnflaecheProEinwohner, "FaFlPEin");
            MapTo(b => b.FahrbahnflaecheProSiedlungsflaeche, "FaFlPSie");
            MapTo(b => b.GesamtlaengeDesStrassennetzesProEinwohner, "GeStrPEin");
            MapTo(b => b.GesamtlaengeDesStrassennetzesProSiedlungsflaeche, "GeStrPSie");
            MapTo(b => b.GesamtstrassenflaecheProEinwohner, "GeStFlPEin");
            MapTo(b => b.GesamtstrassenflaecheProSiedlungsflaeche, "GeStFlPSie");
            MapTo(b => b.WertverlustProEinwohner, "WvlPEin");
            MapTo(b => b.WertverlustProFahrbahn, "WvlPFb");
            MapTo(b => b.WiederbeschaffungswertProEinwohner, "WbwPEin");
            MapTo(b => b.WiederbeschaffungswertProFahrbahn, "WbwPFb");
            MapTo(b => b.ZustandsindexNetz, "ZustandNetz");
            MapTo(b => b.MittleresAlterDerZustandsaufnahmenNetz, "MitAltZusNe");
            MapTo(b => b.RealisierteMassnahmenProEinwohner, "ReMaPEin");
            MapTo(b => b.RealisierteMassnahmenProFahrbahn, "ReMaPFb");
            MapTo(b => b.RealisierteMassnahmenProWertverlustNetz, "ReMaWv");
            MapTo(b => b.RealisierteMassnahmenProWiederbeschaffungswertNetz, "ReMaWbw");

            MapTo(b => b.CalculatedAt, "CalcAt");
            MapTo(b => b.NeedsRecalculation, "NeedsRecalc");

            HasMany(b => b.BenchmarkingDataDetails).KeyColumn("BDD_BDD_BDT_NOR_ID").Cascade.All();

            ReferencesTo(b => b.ErfassungsPeriod);
            ReferencesTo(b => b.Mandant);
        }

        protected override string TableName
        {
            get { return "BenchData"; }
        }

        protected override string PrefixTableName
        {
            get { return PrefixTableNameKlasse.InventarobjektNutzdatentabelle; }
        }
    }
}