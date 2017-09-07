using System;
using System.Linq;
using ASTRA.EMSG.Business.Infrastructure.Validators;
using ASTRA.EMSG.Business.Models.Common;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Common.Enums;
using FluentValidation;

namespace ASTRA.EMSG.Business.Validators.Common
{
    public class ZustandsabschnittdetailsModelValidator : AbstractValidatorBase<ZustandsabschnittdetailsModel>
    {
        public ZustandsabschnittdetailsModelValidator(ILocalizationService localizationService)
            : base(localizationService)
        {
            RuleFor(m => m.Zustandsindex).NotNull()
                .Unless(m => m.SchadengruppeModelList.Any())
                .ShouldNotBeNegative(LocalizationService)
                .InclusiveBetween(0, 5)
                .IsValidDecimalWithDecimalPlaces(1, LocalizationService);

            RuleFor(m => m.IsGrobInitializiert).Must((m, _) => m.Erfassungsmodus != ZustandsErfassungsmodus.Grob || m.IsGrobInitializiert).WithMessage(localizationService.GetLocalizedError(ValidationError.GrobFormIsNotInitialized));
            RuleFor(m => m.IsDetailInitializiert).Must((m, _) => m.Erfassungsmodus != ZustandsErfassungsmodus.Detail || m.IsDetailInitializiert).WithMessage(localizationService.GetLocalizedError(ValidationError.DetailFormIsNotInitialized));
        }
    }
}