using ASTRA.EMSG.Business.Infrastructure.Validators;
using ASTRA.EMSG.Business.Models.Katalogs;
using ASTRA.EMSG.Business.Services.Common;
using FluentValidation;

namespace ASTRA.EMSG.Business.Validators.Katalogs
{
    public class MassnahmenvorschlagKatalogKonstenEditModelValidator : AbstractValidatorBase<MassnahmenvorschlagKatalogKonstenEditModel>
    {
        public MassnahmenvorschlagKatalogKonstenEditModelValidator(ILocalizationService localizationService)
            : base(localizationService)
        {
            RuleForNullableDecimal(m => m.DefaultKosten).Must(m => m.HasValue)
                .WithMessage(localizationService.GetLocalizedError(ValidationError.ShouldNotBeEmpty), m => m.BelastungskategorieBezeichnung);
        }
    }
}