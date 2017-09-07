using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Entities.Mapping;

namespace ASTRA.EMSG.Business.Entities.Katalogs
{
    [TableShortName("GEK")]
    public class GemeindeKatalog : Entity
    {
        public virtual string Typ { get; set; }
    }
}