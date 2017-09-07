using System;
using ASTRA.EMSG.Business.Infrastructure.Validators;
using ASTRA.EMSG.Business.Models.GIS;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Common.Utils;
using FluentValidation;
using ASTRA.EMSG.Business.Services.EntityServices.GIS;
using ASTRA.EMSG.Business.Services.GIS;

namespace ASTRA.EMSG.Business.Validators.GIS
{
    public class RealisierteMassnahmeGISModelValidator : AbstractValidatorBase<RealisierteMassnahmeGISModel>
    {
        public RealisierteMassnahmeGISModelValidator(IGeoJSONParseService geoJSONParseService, ILocalizationService localizationService, IRealisierteMassnahmeGISModelService realisierteMassnahmeGISModelService)
            : base(localizationService)
        {
            RuleForNotNullableString(k => k.Projektname);
            RuleForNullableString(k => k.BezeichnungVon);
            RuleForNullableString(k => k.BezeichnungBis);
            RuleForNullableDecimal(k => k.Laenge).NotEmpty();
            
            RuleForNullableLongString(k => k.Beschreibung);
            RuleForNullableString(k => k.LeitendeOrganisation);
            RuleFor(m => m.Strasseneigentuemer).NotNull();
            RuleFor(m => m.Belastungskategorie).ShouldNotBeEmpty(localizationService);

            RuleForNullableDecimal(s => s.KostenFahrbahn, 0);
            RuleForNullableDecimal(s => s.KostenTrottoirLinks, 0);
            RuleForNullableDecimal(s => s.KostenTrottoirRechts, 0);

            RuleForNullableDecimal(m => m.BreiteFahrbahn, 2)
                .NotNull()
                .When(m => m.KostenFahrbahn.HasValue);
            RuleForNullableDecimal(m => m.BreiteFahrbahn, 2)
                .NotNull()
                .When(m => (!m.BreiteTrottoirLinks.HasValue && !m.BreiteTrottoirRechts.HasValue) || m.MassnahmenvorschlagFahrbahn.HasValue)
                .WithMessage(localizationService.GetLocalizedError(ValidationError.RealisierteMassnahmeBreiteEmpty));
            RuleForNullableDecimal(m => m.BreiteTrottoirLinks, 2)
                .NotNull()
                .When(m => m.KostenTrottoirLinks.HasValue || (!m.BreiteTrottoirRechts.HasValue && m.MassnahmenvorschlagTrottoir.HasValue));
            RuleForNullableDecimal(m => m.BreiteTrottoirRechts, 2)
                .NotNull()
                .When(m => m.KostenTrottoirRechts.HasValue || (!m.BreiteTrottoirLinks.HasValue && m.MassnahmenvorschlagTrottoir.HasValue));

            RuleFor(m => m.MassnahmenvorschlagFahrbahn)
                .ShouldNotBeEmpty(localizationService)
                .When(m => m.BreiteFahrbahn.HasValue || m.KostenFahrbahn.HasValue);
            RuleFor(m => m.MassnahmenvorschlagTrottoir)
                .ShouldNotBeEmpty(localizationService)
                .When(m => 
                    (m.BreiteTrottoirLinks.HasValue || m.BreiteTrottoirRechts.HasValue) || 
                    (m.KostenTrottoirLinks.HasValue || m.KostenTrottoirRechts.HasValue));
            
            RuleForNullableDecimal(s => s.KostenGesamtprojekt, 0)
                .Must((model, value) => !value.HasValue || value >= model.SumKosten)
                .WithMessage(localizationService.GetLocalizedError(ValidationError.KostenGesamtprojektGraterThanSum));

            RuleFor(m => m.KostenFahrbahn)
                .ShouldNotBeEmpty(localizationService)
                .When(m => m.BreiteFahrbahn.HasValue || m.MassnahmenvorschlagFahrbahn.HasValue);
            RuleFor(m => m.KostenTrottoirLinks)
                .ShouldNotBeEmpty(localizationService)
                .When(m => m.BreiteTrottoirLinks.HasValue || (!m.KostenTrottoirRechts.HasValue && m.MassnahmenvorschlagTrottoir.HasValue));
            RuleFor(m => m.KostenTrottoirRechts)
                .ShouldNotBeEmpty(localizationService)
                .When(m => m.BreiteTrottoirRechts.HasValue || (!m.KostenTrottoirLinks.HasValue && m.MassnahmenvorschlagTrottoir.HasValue));

            RuleFor(m => m.FeatureGeoJSONString).Must(json => json.HasText()).WithMessage(localizationService.GetLocalizedError(ValidationError.GeometryNull)).NotNull().NotEmpty();
            RuleFor(m => m.FeatureGeoJSONString).Must(geoJSONParseService.isAbschnittGISModelBaseValid).When(m => m.FeatureGeoJSONString.HasText()).WithMessage(localizationService.GetLocalizedError(ValidationError.InvalidGeometry));
            
        }
    }
}