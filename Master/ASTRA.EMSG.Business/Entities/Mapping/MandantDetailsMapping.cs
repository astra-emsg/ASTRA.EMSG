using ASTRA.EMSG.Business.Entities.Common;

namespace ASTRA.EMSG.Business.Entities.Mapping
{
    public class MandantDetailsMapping: ClassMapBase<MandantDetails>
    {
        public MandantDetailsMapping()
        {
            MapTo(ns => ns.DifferenzHoehenlageSiedlungsgebiete);
            MapTo(ns => ns.Einwohner);
            MapTo(ns => ns.Gemeindeflaeche);
            ReferencesTo(ns => ns.Gemeindetyp);
            MapTo(ns => ns.MittlereHoehenlageSiedlungsgebiete);
            ReferencesTo(ns => ns.OeffentlicheVerkehrsmittel);
            MapTo(ns => ns.Siedlungsflaeche);
            MapTo(ns => ns.Steuerertrag);
            MapTo(ns => ns.NetzLaenge);

            MapTo(ns => ns.IsCompleted);
            MapTo(ns => ns.IsAchsenEditEnabled);

            ReferencesTo(ns => ns.Mandant);
            ReferencesTo(ns => ns.ErfassungsPeriod);
        }

        protected override string TableName
        {
            get { return "MandantDet"; }
        }

        protected override string PrefixTableName
        {
            get { return PrefixTableNameKlasse.InventarobjektNutzdatentabelle; }
        }
    }
}