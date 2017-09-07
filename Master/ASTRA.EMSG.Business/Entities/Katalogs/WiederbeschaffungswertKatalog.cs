using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Entities.Mapping;

namespace ASTRA.EMSG.Business.Entities.Katalogs
{
    [TableShortName("WBK")]
    public class WiederbeschaffungswertKatalog : WiederbeschaffungswertKatalogBase, IMandantDependentEntity, IErfassungsPeriodDependentEntity
    {
        public virtual bool IsCustomized { get; set; }

        public virtual Mandant Mandant { get; set; }
        public virtual ErfassungsPeriod ErfassungsPeriod { get; set; }
    }
}
