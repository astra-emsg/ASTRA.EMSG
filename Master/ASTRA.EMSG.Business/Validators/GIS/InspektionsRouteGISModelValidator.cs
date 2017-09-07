using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Infrastructure.Validators;
using ASTRA.EMSG.Business.Models.GIS;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Business.Services.EntityServices.GIS;
using FluentValidation;
using System.Linq;

namespace ASTRA.EMSG.Business.Validators.GIS
{
    public class InspektionsRouteGISModelValidator : AbstractValidatorBase<InspektionsRouteGISModel>
    {
        public InspektionsRouteGISModelValidator(ILocalizationService localizationService, IInspektionsRtStrAbschnitteService inspektionsRtStrAbschnitteService)
            : base(localizationService)
        {
            RuleForNotNullableString(m => m.Bezeichnung);
            RuleForNullableLongString(m => m.Bemerkungen);
            RuleForNullableLongString(m => m.Beschreibung);
            RuleForNullableString(m => m.InInspektionBei);
            RuleFor(m => m.InInspektionBis).InclusiveBetween(Defaults.MinDateTime, Defaults.MaxDateTime);

            RuleFor(ir => ir.InspektionsRtStrAbschnitteModelList)
                .Must(inspektionsRtStrAbschnitteService.HasInspektionsRouteGISJustUniqueStraasenabschnitten)
                .WithMessage(localizationService.GetLocalizedError(ValidationError.HasNotJustUniqueStraasenabschnittenInspektionsRouteGIS))
                .Must((m, p) => m.InspektionsRtStrAbschnitteModelList.Any())
                .WithMessage(localizationService.GetLocalizedError(ValidationError.GeometryNull));
        }
    }
}
