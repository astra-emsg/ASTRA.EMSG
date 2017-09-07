using System;
using ASTRA.EMSG.Business.Infrastructure.Validators;
using ASTRA.EMSG.Business.Models.Strassennamen;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Business.Services.EntityServices.Strassennamen;
using ASTRA.EMSG.Business.Validators.Common;
using FluentValidation;

namespace ASTRA.EMSG.Business.Validators.Strassennamen
{
    public class ZustandsabschnittModelValidator : AbstractValidatorBase<ZustandsabschnittModel>
    {
        public ZustandsabschnittModelValidator(ILocalizationService localizationService, IZustandsabschnittService zustandsabschnittService, IStrassenabschnittService strassenabschnittService)
            : base(localizationService)
        {
            ZustandsabschnittCommonModelValidator.ApplyRules(this, localizationService);
            RuleForNullableInt(m => m.Abschnittsnummer);
            RuleForNullableDecimal(m => m.Laenge)
                .NotNull()
                .Must((m, p) => m.Laenge.HasValue && zustandsabschnittService.IsZustandsabschnittLaengeValid(m.Strassenabschnitt, m.Id, m.Laenge.Value))
                .WithMessage(localizationService.GetLocalizedError(
                    ValidationError.StrassenabschnittZustandsabschnittLaengeError), 
                    m => strassenabschnittService.GetById(m.Strassenabschnitt).Laenge);
        }
    }
}