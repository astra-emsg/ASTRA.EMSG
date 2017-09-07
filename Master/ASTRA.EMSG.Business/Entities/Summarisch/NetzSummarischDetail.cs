using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Entities.Katalogs;
using ASTRA.EMSG.Business.Entities.Mapping;

namespace ASTRA.EMSG.Business.Entities.Summarisch
{
    [TableShortName("NSD")]
    public class NetzSummarischDetail : Entity, IErfassungsPeriodDependentEntity, IBelastungskategorieHolder, IFlaecheFahrbahnUndTrottoirHolder
    {
        public virtual decimal? MittlererZustand { get; set; }
        public virtual decimal Fahrbahnlaenge { get; set; }
        public virtual int Fahrbahnflaeche { get; set; }

        public virtual decimal FlaecheFahrbahn { get { return Fahrbahnflaeche; } }
        public virtual decimal FlaecheTrottoir { get { return 0; } }
        public virtual bool HasTrottoirInformation { get { return false; } }

        public virtual NetzSummarisch NetzSummarisch { get; set; }

        public virtual Belastungskategorie Belastungskategorie { get; set; }

        public virtual string BelastungskategorieTyp { get { return Belastungskategorie == null ? null : Belastungskategorie.Typ; } }

        public virtual Mandant Mandant
        {
            get { return NetzSummarisch.Mandant; }
            set { /*NOP */ }
        }

        public virtual ErfassungsPeriod ErfassungsPeriod
        {
            get { return NetzSummarisch.ErfassungsPeriod; }
            set { /*NOP */ }
        }
    }
}
