using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASTRA.EMSG.Business.Infrastructure.Validators;
using ASTRA.EMSG.Business.Models.GIS;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Business.Services.GIS;
using ASTRA.EMSG.Common.Utils;
using FluentValidation;

namespace ASTRA.EMSG.Business.Validators.GIS
{
    public class AchsenSegmentModelValidator : AbstractValidatorBase<AchsenSegmentModel>
    {
        public AchsenSegmentModelValidator(IGeoJSONParseService geoJSONParseService, ILocalizationService localizationService) 
            : base(localizationService)
        {
            RuleForNotNullableString(m => m.Name);

            RuleFor(m => m.FeatureGeoJSONString).Must(json => json.HasText())
                .WithMessage(localizationService.GetLocalizedError(ValidationError.GeometryNull)).NotNull().NotEmpty();

            RuleFor(m => m.FeatureGeoJSONString).Must(geoJSONParseService.isAchsenSegmentModelValid)
                .When(m => m.FeatureGeoJSONString.HasText())
                .WithMessage(localizationService.GetLocalizedError(ValidationError.GeometryNotSimple));
            
        }
    }
}
