using ASTRA.EMSG.Business.Infrastructure.Validators;
using ASTRA.EMSG.Business.Models.Katalogs;
using ASTRA.EMSG.Business.Services.Common;
using FluentValidation;

namespace ASTRA.EMSG.Business.Validators.Katalogs
{
    public class WiederbeschaffungswertKatalogEditModelValidator : AbstractValidatorBase<WiederbeschaffungswertKatalogEditModel>
    {
        public WiederbeschaffungswertKatalogEditModelValidator(ILocalizationService localizationService)
            : base(localizationService)
        {
            RuleForNotNullableDecimal(m => m.FlaecheFahrbahn, 2);
            RuleForNotNullableDecimal(m => m.FlaecheTrottoir, 2);
            RuleForNotNullableDecimal(m => m.GesamtflaecheFahrbahn, 2);

            RuleForNotNullableDecimal(m => m.AlterungsbeiwertI, 2);
            RuleForNotNullableDecimal(m => m.AlterungsbeiwertII, 2);

            RuleFor(m => m.Belastungskategorie).NotNull();
        }
    }
}