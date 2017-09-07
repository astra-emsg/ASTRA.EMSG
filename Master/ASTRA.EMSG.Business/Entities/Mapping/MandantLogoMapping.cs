using ASTRA.EMSG.Business.Entities.Common;

namespace ASTRA.EMSG.Business.Entities.Mapping
{
    public class MandantLogoMapping : ClassMapBase<MandantLogo>
    {
        public MandantLogoMapping()
        {
            MapTo(e => e.Logo).Length(2000000);
            MapTo(e => e.Height);
            MapTo(e => e.Width);

            ReferencesTo(e => e.Mandant);
        }

        protected override string TableName
        {
            get { return "MandantLogo"; }
        }

        protected override string PrefixTableName
        {
            get { return PrefixTableNameKlasse.InventarobjektNutzdatentabelle; }
        }
    }
}