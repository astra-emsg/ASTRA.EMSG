using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Infrastructure.Validators;
using ASTRA.EMSG.Business.Models.Summarisch;
using ASTRA.EMSG.Business.Services.Common;
using FluentValidation;

namespace ASTRA.EMSG.Business.Validators.Summarisch
{
    public class NetzSummarischModelValidator : AbstractValidatorBase<NetzSummarischModel>
    {
        public NetzSummarischModelValidator(ILocalizationService localizationService)
            : base(localizationService)
        {
            RuleFor(m => m.MittleresErhebungsJahr).InclusiveBetween(Defaults.MinDateTime, Defaults.MaxDateTime);
        }
    }
}