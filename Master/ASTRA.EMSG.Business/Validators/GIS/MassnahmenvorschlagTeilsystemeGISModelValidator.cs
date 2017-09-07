using ASTRA.EMSG.Business.Infrastructure.Validators;
using ASTRA.EMSG.Business.Models.GIS;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Common.Utils;
using FluentValidation;
using ASTRA.EMSG.Business.Services.GIS;

namespace ASTRA.EMSG.Business.Validators.GIS
{
    public class MassnahmenvorschlagTeilsystemeGISModelValidator : AbstractValidatorBase<MassnahmenvorschlagTeilsystemeGISModel>
    {
        public MassnahmenvorschlagTeilsystemeGISModelValidator(IGeoJSONParseService geoJSONParseService, ILocalizationService localizationService)
            : base(localizationService)
        {
            RuleForNotNullableString(s => s.Projektname);
            RuleForNullableString(s => s.BezeichnungVon);
            RuleForNullableString(s => s.BezeichnungBis);
            RuleForNullableDecimal(k => k.Laenge).NotEmpty();
            RuleFor(s => s.Teilsystem).NotEmpty();
            RuleForNullableLongString(s => s.Beschreibung);
            RuleForNullableDecimal(s => s.Kosten, 0);

            RuleFor(m => m.FeatureGeoJSONString).Must(json => json.HasText()).WithMessage(localizationService.GetLocalizedError(ValidationError.GeometryNull)).NotNull().NotEmpty();
            RuleFor(m => m.FeatureGeoJSONString).Must(geoJSONParseService.isAbschnittGISModelBaseValid).When(m => m.FeatureGeoJSONString.HasText()).WithMessage(localizationService.GetLocalizedError(ValidationError.InvalidGeometry));
        }
    }
}