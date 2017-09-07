using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Entities.Mapping;

namespace ASTRA.EMSG.Business.Entities.Katalogs
{
    [TableShortName("OVG")]
    public class OeffentlicheVerkehrsmittelKatalog : Entity
    {
        public virtual string Typ { get; set; }
    }
}