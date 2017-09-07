using ASTRA.EMSG.Business.Infrastructure.Validators;
using ASTRA.EMSG.Business.Reports.BenchmarkauswertungZustandskennwerten;
using ASTRA.EMSG.Business.Services.Common;
using FluentValidation;

namespace ASTRA.EMSG.Business.Validators.Benchmarking
{
    public class BenchmarkauswertungZustandskennwertenParameterValidator : AbstractValidatorBase<BenchmarkauswertungZustandskennwertenParameter>
    {
        public BenchmarkauswertungZustandskennwertenParameterValidator(ILocalizationService localizationService) : base(localizationService)
        {
            RuleFor(m => m.BenchmarkingGruppenTypList).Must(p => p == null || p.Count <= 3).WithMessage(localizationService.GetLocalizedError(ValidationError.MaximumEigenschaften));
        }
    }
}