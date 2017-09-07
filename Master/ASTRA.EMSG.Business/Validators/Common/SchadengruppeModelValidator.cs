using ASTRA.EMSG.Business.Infrastructure.Validators;
using ASTRA.EMSG.Business.Models.Common;
using ASTRA.EMSG.Business.Services.Common;

namespace ASTRA.EMSG.Business.Validators.Common
{
    public class SchadengruppeModelValidator : AbstractValidatorBase<SchadengruppeModel>
    {
        public SchadengruppeModelValidator(ILocalizationService localizationService) : base(localizationService)
        {
        }
    }
}