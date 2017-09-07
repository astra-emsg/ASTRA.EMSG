using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASTRA.EMSG.Business.Entities.GIS;

namespace ASTRA.EMSG.Business.Entities.Mapping
{
   
    public class AchsenLockMapping : ClassMapBase<AchsenLock>
    {
        public AchsenLockMapping()
        {
            MapTo(alk => alk.IsLocked);
            MapTo(alk => alk.LockStart);
            MapTo(alk => alk.LockEnd);
            MapTo(alk => alk.LockType);
            ReferencesTo(alk => alk.Mandant);
        }
        protected override string TableName
        {
            get { return "AchsLock"; }
        }

        protected override string PrefixTableName
        {
            get { return PrefixTableNameKlasse.InventarobjektNutzdatentabelle; }
        }
    }
}
