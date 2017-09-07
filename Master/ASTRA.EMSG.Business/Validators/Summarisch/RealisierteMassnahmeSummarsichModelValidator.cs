using ASTRA.EMSG.Business.Infrastructure.Validators;
using ASTRA.EMSG.Business.Models.Summarisch;
using ASTRA.EMSG.Business.Services.Common;
using FluentValidation;

namespace ASTRA.EMSG.Business.Validators.Summarisch
{
    public class RealisierteMassnahmeSummarsichModelValidator : AbstractValidatorBase<RealisierteMassnahmeSummarsichModel>
    {
        public RealisierteMassnahmeSummarsichModelValidator(ILocalizationService localizationService)
            : base(localizationService)
        {
            RuleForNotNullableString(m => m.Projektname);
            RuleForNullableLongString(m => m.Beschreibung);

            RuleForNullableInt(m => m.KostenFahrbahn);
            RuleForNotNullableInt(m => m.Fahrbahnflaeche);
            RuleFor(m => m.Belastungskategorie).ShouldNotBeEmpty(localizationService);
            RuleFor(m => m.Strasseneigentuemer).NotNull();
        }
    }
}