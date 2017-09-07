using ASTRA.EMSG.Business.Entities.Common;

namespace ASTRA.EMSG.Business.Entities.Mapping
{
    public class ErfassungsPeriodMapping : ClassMapBase<ErfassungsPeriod>
    {
        public ErfassungsPeriodMapping()
        {
            MapTo(e => e.Name);
            MapTo(e => e.NetzErfassungsmodus, "NetzModus");
            MapTo(e => e.IsClosed, "IstAbgeschl").OptimisticLock();
            MapTo(e => e.Erfassungsjahr, "ErfJahr").OptimisticLock();

            OptimisticLock.Dirty();

            ReferencesTo(b => b.Mandant);
            DynamicUpdate();
        }

        protected override string TableName
        {
            get { return "ErfPeriode"; }
        }

        protected override string PrefixTableName
        {
            get { return PrefixTableNameKlasse.InventarobjektNutzdatentabelle; }
        }
    }
}