using ASTRA.EMSG.Business.Infrastructure.Validators;
using ASTRA.EMSG.Business.Models.Common;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Common.Enums;
using FluentValidation;

namespace ASTRA.EMSG.Business.Validators.Common
{
    public class ZustandsabschnittdetailsTrottoirModelValidator : AbstractValidatorBase<ZustandsabschnittdetailsTrottoirModel>
    {
        public ZustandsabschnittdetailsTrottoirModelValidator(ILocalizationService localizationService)
            : base(localizationService)
        {
            RuleFor(x => x.LinkeTrottoirDringlichkeit).Must((m, p) => HasLinkeTrottoir(m.Trottoir) || p == DringlichkeitTyp.Unbekannt);
            RuleFor(x => x.LinkeTrottoirGesamtKosten).Must((m, p) => HasLinkeTrottoir(m.Trottoir) || !p.HasValue);
            RuleFor(x => x.LinkeTrottoirKosten).Must((m, p) => HasLinkeTrottoir(m.Trottoir) || !p.HasValue);
            RuleFor(x => x.LinkeTrottoirMassnahmenvorschlagKatalogId).Must((m, p) => HasLinkeTrottoir(m.Trottoir) || !p.HasValue);
            RuleFor(x => x.LinkeTrottoirZustandsindex).Must((m, p) => HasLinkeTrottoir(m.Trottoir) || p == ZustandsindexTyp.Unbekannt);

            RuleFor(x => x.RechteTrottoirDringlichkeit).Must((m, p) => HasRechteTrottoir(m.Trottoir) || p == DringlichkeitTyp.Unbekannt);
            RuleFor(x => x.RechteTrottoirGesamtKosten).Must((m, p) => HasRechteTrottoir(m.Trottoir) || !p.HasValue);
            RuleFor(x => x.RechteTrottoirKosten).Must((m, p) => HasRechteTrottoir(m.Trottoir) || !p.HasValue);
            RuleFor(x => x.RechteTrottoirMassnahmenvorschlagKatalogId).Must((m, p) => HasRechteTrottoir(m.Trottoir) || !p.HasValue);
            RuleFor(x => x.RechteTrottoirZustandsindex).Must((m, p) => HasRechteTrottoir(m.Trottoir) || p == ZustandsindexTyp.Unbekannt);
        }

        private bool HasLinkeTrottoir(TrottoirTyp trottoirTyp)
        {
            return trottoirTyp == TrottoirTyp.BeideSeiten || trottoirTyp == TrottoirTyp.Links;
        }

        private bool HasRechteTrottoir(TrottoirTyp trottoirTyp)
        {
            return trottoirTyp == TrottoirTyp.BeideSeiten || trottoirTyp == TrottoirTyp.Rechts;
        }
    }
}