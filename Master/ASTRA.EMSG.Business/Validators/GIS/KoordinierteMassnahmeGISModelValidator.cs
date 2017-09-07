using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Infrastructure.Validators;
using ASTRA.EMSG.Business.Models.GIS;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Common.Utils;
using FluentValidation;
using ASTRA.EMSG.Business.Services.GIS;

namespace ASTRA.EMSG.Business.Validators.GIS
{
    public class KoordinierteMassnahmeGISModelValidator : AbstractValidatorBase<KoordinierteMassnahmeGISModel>
    {
        public KoordinierteMassnahmeGISModelValidator(ILocalizationService localizationService, IGeoJSONParseService geoJSONParseService)
            : base(localizationService)
        {
            RuleForNotNullableString(k => k.Projektname);
            RuleForNullableString(k => k.BezeichnungVon);
            RuleForNullableString(k => k.BezeichnungBis);
            RuleForNullableDecimal(k => k.Laenge).NotEmpty();
            RuleForNotNullableDecimal(s => s.BreiteFahrbahn, 2);
            RuleForNullableDecimal(s => s.BreiteTrottoirLinks, 2);
            RuleForNullableDecimal(s => s.BreiteTrottoirRechts, 2);
            RuleFor(s => s.BeteiligteSysteme).NotEmpty().WithMessage(localizationService.GetLocalizedError(ValidationError.BeteiligteSystemeEmpty));
            RuleForNullableDecimal(s => s.KostenGesamtprojekt, 0)
                .Must((model, value) => (value ?? 0) >= model.SumKosten)
                .When(m => m.KostenGesamtprojekt.HasValue)
                .WithMessage(localizationService.GetLocalizedError(ValidationError.KostenGesamtprojektGraterThanSum));
            RuleForNullableDecimal(s => s.KostenFahrbahn, 0);
            RuleForNullableDecimal(s => s.KostenTrottoirLinks, 0);
            RuleForNullableDecimal(s => s.KostenTrottoirRechts, 0);
            RuleForNullableLongString(k => k.Beschreibung);
            RuleForNullableString(k => k.LeitendeOrganisation);

            RuleFor(k => k.AusfuehrungsAnfang)
                .InclusiveBetween(Defaults.MinDateTime, Defaults.MaxDateTime)
                .Must((m, aa) => ValidateAusfuehrungsDatum(m))
                .WithMessage(LocalizationService.GetLocalizedError(ValidationError.AusfuehrungsAnfangAusfuehrungsEndeError));

            RuleFor(k => k.AusfuehrungsEnde)
                .InclusiveBetween(Defaults.MinDateTime, Defaults.MaxDateTime)
                .Must((m, ae) => ValidateAusfuehrungsDatum(m))
                .WithMessage(LocalizationService.GetLocalizedError(ValidationError.AusfuehrungsAnfangAusfuehrungsEndeError));

            RuleFor(m => m.FeatureGeoJSONString).Must(json => json.HasText()).WithMessage(localizationService.GetLocalizedError(ValidationError.GeometryNull)).NotNull().NotEmpty();
            RuleFor(m => m.FeatureGeoJSONString).Must(geoJSONParseService.isAbschnittGISModelBaseValid).When(m => m.FeatureGeoJSONString.HasText()).WithMessage(localizationService.GetLocalizedError(ValidationError.InvalidGeometry));
        }

        private static bool ValidateAusfuehrungsDatum(KoordinierteMassnahmeGISModel m)
        {
            return !m.AusfuehrungsAnfang.HasValue || !m.AusfuehrungsEnde.HasValue || m.AusfuehrungsAnfang <= m.AusfuehrungsEnde;
        }
    }
}