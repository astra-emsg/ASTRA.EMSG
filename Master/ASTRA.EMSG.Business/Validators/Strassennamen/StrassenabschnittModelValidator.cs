using System;
using ASTRA.EMSG.Business.Infrastructure.Validators;
using ASTRA.EMSG.Business.Models.Strassennamen;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Business.Services.EntityServices.Strassennamen;
using ASTRA.EMSG.Business.Validators.Common;
using FluentValidation;

namespace ASTRA.EMSG.Business.Validators.Strassennamen
{
    public class StrassenabschnittModelValidator : AbstractValidatorBase<StrassenabschnittModel>
    {
        public StrassenabschnittModelValidator(ILocalizationService localizationService, IStrassenabschnittService strassenabschnittService) : base(localizationService)
        {
            StrassenabschnittCommonModelValidator.ApplyRules(this, localizationService);
            RuleForNullableInt(m => m.Abschnittsnummer);
            RuleFor(m => m.ExternalId).Must((model, propertyValue) => strassenabschnittService.IsUniqueId(propertyValue, model.Id))
               .When(m => !string.IsNullOrEmpty(m.ExternalId))
               .WithMessage(localizationService.GetLocalizedError(ValidationError.ShouldBeUnique));
            RuleFor(m => m.Laenge)
                .NotNull()
                .InclusiveBetween(1, 50000)
                .IsValidDecimalWithDecimalPlaces(1, LocalizationService)
                .Must((m, p) => m.Laenge.HasValue && (m.Laenge.Value >= strassenabschnittService.GetSumOfZustandsabschnittLaenge(m.Id)))
                .WithMessage(localizationService.GetLocalizedError(ValidationError.StrassenabschnittZustandsabschnittLaengeError), m => m.Laenge);
        }
    }
}