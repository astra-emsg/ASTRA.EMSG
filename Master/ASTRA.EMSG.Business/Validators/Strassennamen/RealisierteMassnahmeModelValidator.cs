using ASTRA.EMSG.Business.Infrastructure.Validators;
using ASTRA.EMSG.Business.Models.Strassennamen;
using ASTRA.EMSG.Business.Services.Common;
using FluentValidation;

namespace ASTRA.EMSG.Business.Validators.Strassennamen
{
    public class RealisierteMassnahmeModelValidator : AbstractValidatorBase<RealisierteMassnahmeModel>
    {
        public RealisierteMassnahmeModelValidator(ILocalizationService localizationService)
            : base(localizationService)
        {
            RuleForNotNullableString(m => m.Projektname);
            RuleForNullableLongString(m => m.Beschreibung);
            RuleForNullableString(m => m.BezeichnungBis);
            RuleForNullableString(m => m.BezeichnungVon);

            RuleForNullableDecimal(m => m.KostenFahrbahn, 0);
            RuleForNullableDecimal(m => m.KostenTrottoirLinks, 0);
            RuleForNullableDecimal(m => m.KostenTrottoirRechts, 0);

            RuleForNullableDecimal(m => m.Laenge, 1).NotNull();


            RuleForNullableDecimal(m => m.BreiteFahrbahn)
                .NotNull()
                .When(m => m.KostenFahrbahn.HasValue);
            RuleForNullableDecimal(m => m.BreiteFahrbahn)
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

            RuleFor(m => m.KostenFahrbahn)
                .ShouldNotBeEmpty(localizationService)
                .When(m => m.BreiteFahrbahn.HasValue || m.MassnahmenvorschlagFahrbahn.HasValue);
            RuleFor(m => m.KostenTrottoirLinks)
                .ShouldNotBeEmpty(localizationService)
                .When(m => m.BreiteTrottoirLinks.HasValue || (!m.KostenTrottoirRechts.HasValue && m.MassnahmenvorschlagTrottoir.HasValue));
            RuleFor(m => m.KostenTrottoirRechts)
                .ShouldNotBeEmpty(localizationService)
                .When(m => m.BreiteTrottoirRechts.HasValue || (!m.KostenTrottoirLinks.HasValue && m.MassnahmenvorschlagTrottoir.HasValue));

         
            RuleFor(m => m.Belastungskategorie).ShouldNotBeEmpty(localizationService);
            RuleFor(m => m.Strasseneigentuemer).NotNull();
        }
    }
}