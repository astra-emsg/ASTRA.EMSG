using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Infrastructure.Validators;
using ASTRA.EMSG.Business.Models.Common;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Business.Services.EntityServices.Common;
using FluentValidation;

namespace ASTRA.EMSG.Business.Validators.Common
{
    public class KenngroessenFruehererJahreModelValidator : AbstractValidatorBase<KenngroessenFruehererJahreModel>
    {
        private ErfassungsPeriod oldestClosedErfassungsperiod;

        public KenngroessenFruehererJahreModelValidator(
            ILocalizationService localizationService, 
            IErfassungsPeriodService erfassungsPeriodService,
            IKenngroessenFruehererJahreService kenngroessenFruehererJahreService
            )
            : base(localizationService)
        {
            RuleFor(m => m.Jahr).NotNull()
                .ShouldNotBeNegative(LocalizationService)
                .InclusiveBetween(1000, 9999)
                .Must(jahr => IsValidJahr(erfassungsPeriodService, jahr))
                .WithMessage(localizationService.GetLocalizedError(ValidationError.InvalidKenngroessenFuehererJahre),
                 k =>  erfassungsPeriodService.GetOldestClosedErfassungsperiod() == null ? "" : oldestClosedErfassungsperiod.Erfassungsjahr.Year.ToString())
                .Must((m,jahr) => kenngroessenFruehererJahreService.IsJahrUnique(m))
                .WithMessage(localizationService.GetLocalizedError(ValidationError.ShouldBeUnique));

            RuleForNullableDecimal(m => m.KostenFuerWerterhaltung).NotEmpty();
            RuleFor(m => m.KenngroesseFruehereJahrDetailModels)
                .SetCollectionValidator(new KenngroessenFruehererJahreDetailModelValidator(localizationService));
        }

        private bool IsValidJahr(IErfassungsPeriodService erfassungsPeriodService, int? jahr)
        {
            if (!jahr.HasValue)
                return true;

            oldestClosedErfassungsperiod = erfassungsPeriodService.GetOldestClosedErfassungsperiod();
            if (oldestClosedErfassungsperiod == null)
                return false;

            return jahr < oldestClosedErfassungsperiod.Erfassungsjahr.Year;
        }
    }
}
