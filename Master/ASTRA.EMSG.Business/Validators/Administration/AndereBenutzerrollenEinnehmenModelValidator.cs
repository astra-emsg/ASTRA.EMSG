using ASTRA.EMSG.Business.Infrastructure.Validators;
using ASTRA.EMSG.Business.Models.Administration;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Business.Services.Security;
using FluentValidation;

namespace ASTRA.EMSG.Business.Validators.Administration
{
    public class AndereBenutzerrollenEinnehmenModelValidator : AbstractValidatorBase<AndereBenutzerrollenEinnehmenModel>
    {
        public AndereBenutzerrollenEinnehmenModelValidator(ILocalizationService localizationService, IApplicationSupporterService applicationSupporterService)
            : base(localizationService)
        {
            RuleFor(m => m.UserName).Must(applicationSupporterService.IsUserNameValid).WithMessage(localizationService.GetLocalizedError(ValidationError.InvalidUserName));
        }
    }
}