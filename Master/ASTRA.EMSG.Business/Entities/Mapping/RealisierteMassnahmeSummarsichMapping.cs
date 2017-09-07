using ASTRA.EMSG.Business.Entities.Summarisch;

namespace ASTRA.EMSG.Business.Entities.Mapping
{
    public class RealisierteMassnahmeSummarsichMapping : ClassMapBase<RealisierteMassnahmeSummarsich>
    {
        public RealisierteMassnahmeSummarsichMapping()
        {
            MapToLongText(rm => rm.Beschreibung, "Beschr");
            MapTo(rm => rm.KostenFahrbahn, "FbKosten");
            MapTo(rm => rm.Projektname);
            MapTo(rm => rm.Fahrbahnflaeche, "FbFlaeche");
            MapTo(za => za.Strasseneigentuemer, "Eigentuemer");

            ReferencesTo(rm => rm.ErfassungsPeriod);
            ReferencesTo(rm => rm.Mandant);
            ReferencesTo(rm => rm.Belastungskategorie);
        }

        protected override string TableName
        {
            get { return "RelMassSum"; }
        }

        protected override string PrefixTableName
        {
            get { return PrefixTableNameKlasse.InventarobjektNutzdatentabelle; }
        }
    }
}