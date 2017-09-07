using ASTRA.EMSG.Business.Infrastructure.Validators;
using ASTRA.EMSG.Business.Models.Common;
using ASTRA.EMSG.Business.Services.Common;

namespace ASTRA.EMSG.Business.Validators.Common
{
    public class SchadendetailModelValidator : AbstractValidatorBase<SchadendetailModel>
    {
        public SchadendetailModelValidator(ILocalizationService localizationService) : base(localizationService)
        {
        }
    }
}