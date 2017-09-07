using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Infrastructure.Validators;
using ASTRA.EMSG.Business.Models.Common;
using ASTRA.EMSG.Business.Services.Common;
using FluentValidation;

namespace ASTRA.EMSG.Business.Validators.Common
{
    public static class ZustandsabschnittCommonModelValidator
    {
        public static void ApplyRules<T>(AbstractValidatorBase<T> abstractValidatorBase, ILocalizationService localizationService) where T : ZustandsabschnittModelBase
        {
            abstractValidatorBase.RuleForNullableString(m => m.BezeichnungVon);
            abstractValidatorBase.RuleForNullableString(m => m.BezeichnungBis);
            abstractValidatorBase.RuleForNullableLongString(m => m.Bemerkung);
            abstractValidatorBase.RuleFor(m => m.Aufnahmedatum).NotNull().InclusiveBetween(Defaults.MinDateTime, Defaults.MaxDateTime);
        }
    }
}