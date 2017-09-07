using System.Collections.Generic;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Entities.Mapping;
using ASTRA.EMSG.Common.Enums;

namespace ASTRA.EMSG.Business.Entities.Katalogs
{
    [TableShortName("BLK")]
    public class Belastungskategorie : Entity
    {
        public Belastungskategorie()
        {
            AllowedBelagList = new List<BelagsTyp>();
        }

        public virtual string Typ { get; set; }
        public virtual int Reihenfolge { get; set; }
        public virtual decimal? DefaultBreiteFahrbahn { get; set; }
        public virtual decimal? DefaultBreiteTrottoirRechts { get; set; }
        public virtual decimal? DefaultBreiteTrottoirLinks { get; set; }

        public virtual IList<BelagsTyp> AllowedBelagList { get; set; }

        public virtual string ColorCode { get; set; }
    }
}
