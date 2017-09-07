using System;
using System.Collections.Generic;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Infrastructure.Validators;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Business.Services.EntityServices.GIS;
using ASTRA.EMSG.Common.DataTransferObjects;
using FluentValidation;

namespace ASTRA.EMSG.Business.Validators.GIS
{
    public class ZustandsabschnittGISDTOValidator : AbstractValidatorBase<ZustandsabschnittGISDTO>
    {
        private readonly IZustandsabschnittGISService zustandsabschnittGISService;
        private readonly IStrassenabschnittGISService strassenabschnittGISService;
        private readonly ILocalizationService localizationService;
        public ZustandsabschnittGISDTOValidator(ILocalizationService localizationService, IZustandsabschnittGISService zustandsabschnittGISService, IStrassenabschnittGISService strassenabschnittGISService)
            : base(localizationService)
        {
            base.RuleForNullableString(m => m.BezeichnungVon);
            base.RuleForNullableString(m => m.BezeichnungBis);
            base.RuleFor(m => m.Aufnahmedatum).NotNull().InclusiveBetween(Defaults.MinDateTime, Defaults.MaxDateTime);

            RuleFor(m => m.Shape).NotNull().NotEmpty().WithMessage(localizationService.GetLocalizedError(ValidationError.GeometryNull));

            RuleFor(m => m.ReferenzGruppeDTO.AchsenReferenzenDTO.Count).GreaterThan(0).WithMessage(localizationService.GetLocalizedError(ValidationError.GeometryNull));

            this.zustandsabschnittGISService = zustandsabschnittGISService;
            this.strassenabschnittGISService = strassenabschnittGISService;
            this.localizationService = localizationService;
            //RuleForNullableDecimal(m => m.Laenge)
            //    .NotNull()
            //    .Must((m, p) => m.Laenge.HasValue && zustandsabschnittGISService.IsZustandsabschnittLaengeValid(m.StrassenabschnittGIS, m.Id, m.Laenge.Value))
            //    .WithMessage(localizationService.StrassenabschnittZustandsabschnittLaengeError);
            
        }
        public FluentValidation.Results.ValidationResult Validate(ZustandsabschnittGISDTO instance, IList<Guid> deletedZustandsabschnitte)
        {

            FluentValidation.Results.ValidationResult result  = base.Validate(instance);
            if (!(zustandsabschnittGISService.IsZustandsabschnittLaengeValid(instance.StrassenabschnittGIS, instance.Id, instance.Laenge.Value, deletedZustandsabschnitte)))
            {
                result.Errors.Add(new FluentValidation.Results.ValidationFailure(instance.Laenge.GetType().ToString(), string.Format(localizationService.GetLocalizedError(ValidationError.StrassenabschnittZustandsabschnittLaengeError), strassenabschnittGISService.GetById(instance.StrassenabschnittGIS).Laenge)));
            }
            return result;

        }
    }
}
