using ASTRA.EMSG.Business.Infrastructure.Validators;
using ASTRA.EMSG.Business.Reports.BenchmarkauswertungKennwertenRealisiertenMassnahmen;
using ASTRA.EMSG.Business.Services.Common;
using FluentValidation;

namespace ASTRA.EMSG.Business.Validators.Benchmarking
{
    public class BenchmarkauswertungKennwertenRealisiertenMassnahmenParameterValidator : AbstractValidatorBase<BenchmarkauswertungKennwertenRealisiertenMassnahmenParameter>
    {
        public BenchmarkauswertungKennwertenRealisiertenMassnahmenParameterValidator(ILocalizationService localizationService) : base(localizationService)
        {
            RuleFor(m => m.BenchmarkingGruppenTypList).Must(p => p == null || p.Count <= 3).WithMessage(localizationService.GetLocalizedError(ValidationError.MaximumEigenschaften));
        }
    }
}