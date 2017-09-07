using ASTRA.EMSG.Business.Infrastructure.Validators;
using ASTRA.EMSG.Business.Models.Strassennamen;
using ASTRA.EMSG.Business.Services.Common;
using FluentValidation;

namespace ASTRA.EMSG.Business.Validators.Strassennamen
{
    public class SplitStrassenabschnittModelValidator : AbstractValidatorBase<SplitStrassenabschnittModel>
    {
        public SplitStrassenabschnittModelValidator(ILocalizationService localizationService) : base(localizationService)
        {
            RuleFor(m => m.Count).NotNull().InclusiveBetween(2, 10);
        }
    }
}