using ASTRA.EMSG.Business.Entities.Mapping;

namespace ASTRA.EMSG.Business.Entities.Common
{
    [TableShortName("MAN")]
    public class Mandant : Entity
    {

        public virtual string MandantName { get; set; }

        public virtual string MandantBezeichnung { get; set; }

        public virtual string OwnerId { get; set; }
        
        public virtual string MandantDisplayName
        {
            get { return string.Format("{0} ({1})", MandantBezeichnung, MandantName); }
        }
    }
}