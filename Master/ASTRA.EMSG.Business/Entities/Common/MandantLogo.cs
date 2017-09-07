using ASTRA.EMSG.Business.Entities.Mapping;

namespace ASTRA.EMSG.Business.Entities.Common
{
    [TableShortName("MAL")]
    public class MandantLogo : Entity, IMandantDependentEntity
    {
        public virtual byte[] Logo { get; set; }
        public virtual int Width { get; set; }
        public virtual int Height { get; set; }

        public virtual Mandant Mandant { get; set; }
    }
}