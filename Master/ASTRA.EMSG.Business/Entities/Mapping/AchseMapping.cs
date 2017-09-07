using ASTRA.EMSG.Business.Entities.GIS;

namespace ASTRA.EMSG.Business.Entities.Mapping
{
    public class AchseMapping : ClassMapBase<Achse>
    {
        public AchseMapping()
        {
            MapTo(s => s.BsId).Index("IDX_ACHSE_BSID");
            MapTo(s => s.VersionValidFrom, "ValidFrom");
            MapTo(s => s.Name);
            MapTo(s => s.Operation);
            MapTo(s => s.ImpNr);

            ReferencesTo(me => me.Mandant);
            ReferencesTo(me => me.ErfassungsPeriod);
            ReferencesTo(me => me.CopiedFrom);

            HasMany(ar => ar.AchsenSegmente).KeyColumn("ACS_ACS_ACH_NOR_ID").Cascade.All();
        }

        protected override string TableName
        {
            get { return "Achse"; }
        }

        protected override string PrefixTableName
        {
            get { return PrefixTableNameKlasse.RaumbezugNutzdatentabelle; }
        }
    }
}
