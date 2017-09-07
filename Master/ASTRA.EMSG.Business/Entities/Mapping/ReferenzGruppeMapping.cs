using ASTRA.EMSG.Business.Entities.GIS;

namespace ASTRA.EMSG.Business.Entities.Mapping
{
    public class ReferenzGruppeMapping : ClassMapBase<ReferenzGruppe>
    {
        public ReferenzGruppeMapping()
        {
            HasMany(ar => ar.AchsenReferenzen).KeyColumn("ACR_ACR_RFG_NOR_ID").Cascade.All();
            HasMany(sa => sa.StrassenabschnittGISList).KeyColumn("STG_STG_RFG_NOR_ID"); // Should be HasOne -> Was not working -> Was getting Null 
            HasMany(za => za.ZustandsabschnittGISList).KeyColumn("ZSG_ZSG_RFG_NOR_ID"); // Should be HasOne -> Was not working -> Was getting Null 
            HasMany(za => za.KoordinierteMassnahmeGISList).KeyColumn("KMG_KMG_RFG_NOR_ID"); // Should be HasOne -> Was not working -> Was getting Null 
            HasMany(za => za.MassnahmenvorschlagTeilsystemeGISList).KeyColumn("MTG_MTG_RFG_NOR_ID"); // Should be HasOne -> Was not working -> Was getting Null 
            HasMany(za => za.RealisierteMassnahmeGISList).KeyColumn("RMG_RMG_RFG_NOR_ID"); // Should be HasOne -> Was not working -> Was getting Null 
            
            ReferencesTo(rg => rg.CopiedFrom);
        }

        protected override string TableName
        {
            get { return "RefGruppe"; }
        }

        protected override string PrefixTableName
        {
            get { return PrefixTableNameKlasse.InventarobjektBeziehungstabelle; }
        }
        
    }
}
