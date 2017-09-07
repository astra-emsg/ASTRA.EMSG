using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Infrastructure.Validators;
using ASTRA.EMSG.Business.Models.GIS;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Business.Services.EntityServices.GIS;
using ASTRA.EMSG.Business.Validators.Common;
using FluentValidation;
using ASTRA.EMSG.Common.Utils;
using ASTRA.EMSG.Business.Services.GIS;

namespace ASTRA.EMSG.Business.Validators.GIS
{
    public class StrassenabschnittGISModelValidator : AbstractValidatorBase<StrassenabschnittGISModel>
    {
        public StrassenabschnittGISModelValidator(IGeoJSONParseService geoJSONParseService, ILocalizationService localizationService, IStrassenabschnittGISService strassenabschnittGISService)
            : base(localizationService)
        {
            StrassenabschnittCommonModelValidator.ApplyRules(this, localizationService);
            RuleFor(m => m.FeatureGeoJSONString).Must(json => json.HasText())
                .WithMessage(localizationService.GetLocalizedError(ValidationError.GeometryNull)).NotNull().NotEmpty();
            RuleForNullableInt(m => m.Abschnittsnummer);
            RuleFor(m => m.Laenge)
                .NotNull()
                .When(m => m.FeatureGeoJSONString.HasText())
                .InclusiveBetween(1, 50000)
                .When(m => m.FeatureGeoJSONString.HasText())
                .IsValidDecimalWithDecimalPlaces(1, LocalizationService)
                .When(m => m.FeatureGeoJSONString.HasText())
                .Must((m, p) => m.Laenge.HasValue && (m.Laenge.Value >= strassenabschnittGISService.GetSumOfZustandsabschnittLaenge(m.Id)))
                .WithMessage(localizationService.GetLocalizedError(ValidationError.StrassenabschnittZustandsabschnittLaengeError), m => m.Laenge)
                .When(m => m.FeatureGeoJSONString.HasText());
            RuleFor(m => m.FeatureGeoJSONString).Must(geoJSONParseService.isAbschnittGISModelBaseValid).When(m => m.FeatureGeoJSONString.HasText()).WithMessage(localizationService.GetLocalizedError(ValidationError.InvalidGeometry));
        }
    }
}
