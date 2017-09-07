using System.Linq;
using ASTRA.EMSG.Business.Infrastructure.Validators;
using ASTRA.EMSG.Business.Models.Administration;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Business.Services.EntityServices.Common;
using ASTRA.EMSG.Business.Services.Historization;
using FluentValidation;
using ASTRA.EMSG.Business.Services.EntityServices.GIS;

namespace ASTRA.EMSG.Business.Validators.Administration
{
    public class ErfassungsabschlussModelValidator : AbstractValidatorBase<ErfassungsabschlussModel>
    {
        public ErfassungsabschlussModelValidator(
            ILocalizationService localizationService, 
            IHistorizationService historizationService, 
            IStrassenabschnittGISService strassenabschnittGISService,
            IMandantDetailsService mandantDetailsService)
            : base(localizationService)
        {
            var availabledDates = historizationService.GetAvailableErfassungsabschlussen().Select(x => x.AbschlussDate).ToList();
            RuleFor(m => m.AbschlussDate).ExclusiveBetween(availabledDates.First().AddDays(-1), availabledDates.Last().AddDays(1));
            RuleFor(m => m.AbschlussDate).Must((m, p) => !strassenabschnittGISService.AreThereLockedStrassenabschnitte()).WithMessage(localizationService.GetLocalizedError(ValidationError.JahresAbschlussStrassenabschnittLocked));
            RuleFor(m => m.AbschlussDate).Must((m, p) => mandantDetailsService.GetCurrentMandantDetails().IsCompleted);
        }
    }
}