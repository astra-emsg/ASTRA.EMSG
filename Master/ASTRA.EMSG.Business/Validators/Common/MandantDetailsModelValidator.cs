using ASTRA.EMSG.Business.Infrastructure.Validators;
using ASTRA.EMSG.Business.Models.Administration;
using ASTRA.EMSG.Business.Services.Common;

namespace ASTRA.EMSG.Business.Validators.Common
{
    public class MandantDetailsModelValidator : AbstractValidatorBase<MandantDetailsModel>
    {
        public MandantDetailsModelValidator(ILocalizationService localizationService)
            : base(localizationService)
        {
            RuleForNotNullableInt(m => m.DifferenzHoehenlageSiedlungsgebiete);
            RuleForNotNullableInt(m => m.Einwohner);
            RuleForNotNullableInt(m => m.Gemeindeflaeche);
            RuleForNotNullableInt(m => m.MittlereHoehenlageSiedlungsgebiete);
            RuleForNotNullableInt(m => m.Siedlungsflaeche);
            RuleForNotNullableInt(m => m.Steuerertrag);

            RuleFor(m => m.OeffentlicheVerkehrsmittel).ShouldNotBeEmpty(localizationService);
            RuleFor(m => m.Gemeindetyp).ShouldNotBeEmpty(localizationService);
        }
    }
}