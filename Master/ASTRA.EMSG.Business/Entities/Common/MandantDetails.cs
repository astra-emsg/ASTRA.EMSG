using ASTRA.EMSG.Business.Entities.Katalogs;
using ASTRA.EMSG.Business.Entities.Mapping;

namespace ASTRA.EMSG.Business.Entities.Common
{
    [TableShortName("MAD")]
    public class MandantDetails : Entity, IErfassungsPeriodDependentEntity
    {
        public virtual int? Einwohner { get; set; }
        public virtual int? Siedlungsflaeche { get; set; }
        public virtual int? Gemeindeflaeche { get; set; }
        public virtual GemeindeKatalog Gemeindetyp { get; set; }
        public virtual int? MittlereHoehenlageSiedlungsgebiete { get; set; }
        public virtual int? DifferenzHoehenlageSiedlungsgebiete { get; set; }
        public virtual int? Steuerertrag { get; set; }
        public virtual OeffentlicheVerkehrsmittelKatalog OeffentlicheVerkehrsmittel { get; set; }

        //Unit: km
        public virtual decimal NetzLaenge { get; set; }

        public virtual bool IsCompleted { get; set; }

        public virtual string MandantName { get { return Mandant.MandantName; } }

        public virtual bool IsAchsenEditEnabled { get; set; }

        public virtual Mandant Mandant { get; set; }
        public virtual ErfassungsPeriod ErfassungsPeriod { get; set; }
    }
}