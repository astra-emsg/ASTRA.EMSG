using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASTRA.EMSG.Business.Entities.GIS;
using NHibernate.Spatial.Type;
using ASTRA.EMSG.Business.Utils;

namespace ASTRA.EMSG.Business.Entities.Mapping
{
    public class AchsenUpdateConflictMapping : ClassMapBase<AchsenUpdateConflict>
    {
        public AchsenUpdateConflictMapping()
        {
            MapTo(ar => ar.Name);
            MapTo(ar => ar.ConflictType);
            MapTo(ar => ar.ItemType);
            MapTo(ar => ar.ItemId);
            MapTo(ar => ar.SegmentId);
            MapTo(ar => ar.Shape);

            ReferencesTo(ar => ar.Mandant);
            ReferencesTo(ar => ar.ErfassungsPeriod);
        }

        protected override string TableName
        {
            get { return "AchsUpdCon"; }
        }

        protected override string PrefixTableName
        {
            get { return PrefixTableNameKlasse.InventarobjektNutzdatentabelle; }
        }
    }
}
