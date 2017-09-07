using ASTRA.EMSG.Business.Infrastructure.Validators;
using ASTRA.EMSG.Business.Models.Summarisch;
using ASTRA.EMSG.Business.Services.Common;
using FluentValidation;

namespace ASTRA.EMSG.Business.Validators.Summarisch
{
    public class NetzSummarischDetailModelValidator : AbstractValidatorBase<NetzSummarischDetailModel>
    {
        public NetzSummarischDetailModelValidator(ILocalizationService localizationService): base(localizationService)
        {
            RuleFor(m => m.MittlererZustand).InclusiveBetween(0, 5).IsValidDecimalWithDecimalPlaces(1, LocalizationService);

            RuleForNotNullableDecimal(m => m.Fahrbahnlaenge, 1);
            RuleForNotNullableInt(m => m.Fahrbahnflaeche);
        }
    }
}