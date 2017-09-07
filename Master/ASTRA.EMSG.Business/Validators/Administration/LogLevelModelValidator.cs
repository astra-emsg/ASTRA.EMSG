using ASTRA.EMSG.Business.Infrastructure.Validators;
using ASTRA.EMSG.Business.Models.Administration;
using ASTRA.EMSG.Business.Services.Common;
using FluentValidation;

namespace ASTRA.EMSG.Business.Validators.Administration
{
    public class LogLevelModelValidator : AbstractValidatorBase<LogLevelModel>
    {
        public LogLevelModelValidator(ILocalizationService localizationService, ILogHandlerService logHandlerService)
            : base(localizationService)
        {
            RuleFor(m => m.LogLevel).Must(logHandlerService.IsLogLevelValid).WithMessage(localizationService.GetLocalizedError(ValidationError.InvalidLogLevel));
        }
    }
}