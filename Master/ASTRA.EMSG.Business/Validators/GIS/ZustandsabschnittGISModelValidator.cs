using ASTRA.EMSG.Business.Infrastructure.Validators;
using ASTRA.EMSG.Business.Models.GIS;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Business.Services.EntityServices.GIS;
using ASTRA.EMSG.Business.Validators.Common;
using FluentValidation;
using GeoAPI.Geometries;
using System;
using ASTRA.EMSG.Business.Services.GIS;
using ASTRA.EMSG.Common.Utils;

namespace ASTRA.EMSG.Business.Validators.GIS
{
    public class ZustandsabschnittGISModelValidator : AbstractValidatorBase<ZustandsabschnittGISModel>
    {
        
        public ZustandsabschnittGISModelValidator(IGeoJSONParseService geoJSONParseService, ILocalizationService localizationService, IZustandsabschnittGISService zustandsabschnittGISService, IStrassenabschnittGISService strassenabschnittGISService)
            : base(localizationService)
        {
            ZustandsabschnittCommonModelValidator.ApplyRules(this, localizationService);
            
            RuleFor(m => m.FeatureGeoJSONString).Must(json => json.HasText()).WithMessage(localizationService.GetLocalizedError(ValidationError.GeometryNull)).NotNull().NotEmpty();
            RuleForNullableInt(m => m.Abschnittsnummer);
            RuleForNullableDecimal(m => m.Laenge)
                .NotNull()
                .Must((m, p) => m.Laenge.HasValue && zustandsabschnittGISService.IsZustandsabschnittLaengeValid(m.StrassenabschnittGIS, m.Id, m.Laenge.Value))
                .When(m => m.FeatureGeoJSONString.HasText())
                .WithMessage(localizationService.GetLocalizedError(ValidationError.StrassenabschnittZustandsabschnittLaengeError), m => strassenabschnittGISService.GetById(m.StrassenabschnittGIS).Laenge);
            RuleFor(m => m.FeatureGeoJSONString).Must(geoJSONParseService.isAbschnittGISModelBaseValid).When(m => m.FeatureGeoJSONString.HasText()).WithMessage(localizationService.GetLocalizedError(ValidationError.InvalidGeometry));
        }

       
        
    }
}