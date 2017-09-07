using ASTRA.EMSG.Business.Infrastructure.Validators;
using ASTRA.EMSG.Business.Models.Common;
using ASTRA.EMSG.Business.Services.Common;
using FluentValidation;

namespace ASTRA.EMSG.Business.Validators.Common
{
    public class KenngroessenFruehererJahreDetailModelValidator : AbstractValidatorBase<KenngroessenFruehererJahreDetailModel>
    {
        public KenngroessenFruehererJahreDetailModelValidator(ILocalizationService localizationService)
            : base(localizationService)
        {
            RuleForNotNullableDecimal(m => m.Fahrbahnlaenge, 1);
            RuleForNotNullableInt(m => m.Fahrbahnflaeche);
            RuleFor(m => m.MittlererZustand)
                .ShouldNotBeNegative(localizationService)
                .InclusiveBetween(0, 5)
                .IsValidDecimalWithDecimalPlaces(1, localizationService);
        }
    }
}