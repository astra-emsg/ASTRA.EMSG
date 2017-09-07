using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Infrastructure.Validators;
using ASTRA.EMSG.Business.Models.Strassennamen;
using ASTRA.EMSG.Business.Services.Common;
using FluentValidation;

namespace ASTRA.EMSG.Business.Validators.Strassennamen
{
    public class ZustandsabschnittImportModelValidator : AbstractValidatorBase<ZustandsabschnittImportModel>
    {
        public ZustandsabschnittImportModelValidator(ILocalizationService localizationService)
            : base(localizationService)
        {           
            RuleForNotNullableDecimal(m => m.Laenge);
            RuleFor(m => m.Aufnahmedatum).NotNull().InclusiveBetween(Defaults.MinDateTime, Defaults.MaxDateTime);
            RuleFor(m => m.Zustandsindex).NotNull().InclusiveBetween(0, 5).IsValidDecimalWithDecimalPlaces(1, LocalizationService);
            RuleForNullableInt(m => m.Abschnittsnummer);
            RuleForNullableString(m => m.BezeichnungVon);
            RuleForNullableString(m => m.BezeichnungBis);
            RuleForNullableString(m => m.Bemerkung);
            RuleForNullableString(m => m.Aufnahmeteam);
        }
    }
}