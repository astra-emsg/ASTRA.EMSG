using System;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Infrastructure.Validators;
using ASTRA.EMSG.Business.Models.Common;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Common.Enums;
using FluentValidation;

namespace ASTRA.EMSG.Business.Validators.Common
{
    public static class StrassenabschnittCommonModelValidator
    {
        public static void ApplyRules<T>(AbstractValidatorBase<T> abstractValidatorBase, ILocalizationService localizationService) where T : StrassenabschnittModelBase
        {
            abstractValidatorBase.RuleForNotNullableString(m => m.Strassenname);
            abstractValidatorBase.RuleFor(m => m.Belastungskategorie).ShouldNotBeEmpty(localizationService);
            abstractValidatorBase.RuleFor(m => m.Belag).NotNull();
            abstractValidatorBase.RuleFor(m => m.Trottoir).NotNull();
            abstractValidatorBase.RuleFor(m => m.Strasseneigentuemer).NotNull();
            abstractValidatorBase.RuleFor(m => m.BreiteFahrbahn).NotNull().InclusiveBetween(1, 50).IsValidDecimalWithDecimalPlaces(2, localizationService);
            abstractValidatorBase.RuleForNullableString(m => m.Ortsbezeichnung);
            abstractValidatorBase.RuleForNullableString(m => m.BezeichnungVon);
            abstractValidatorBase.RuleForNullableString(m => m.BezeichnungBis);
            abstractValidatorBase.RuleForNullableDecimal(m => m.BreiteTrottoirLinks);
            abstractValidatorBase.RuleForNullableDecimal(m => m.BreiteTrottoirRechts);

            abstractValidatorBase.RuleFor(m => m.BreiteTrottoirLinks)
                .InclusiveBetween(0, 30)
                .When(s => s.Trottoir == TrottoirTyp.BeideSeiten || s.Trottoir == TrottoirTyp.Links)
                .ShouldBeEmpty(localizationService)
                .When(s => s.Trottoir == TrottoirTyp.NochNichtErfasst || s.Trottoir == TrottoirTyp.KeinTrottoir || s.Trottoir == TrottoirTyp.Rechts);
            abstractValidatorBase.RuleFor(m => m.BreiteTrottoirRechts)
                .InclusiveBetween(0, 30)
                .When(s => s.Trottoir == TrottoirTyp.BeideSeiten || s.Trottoir == TrottoirTyp.Rechts)
                .ShouldBeEmpty(localizationService)
                .When(s => s.Trottoir == TrottoirTyp.NochNichtErfasst || s.Trottoir == TrottoirTyp.KeinTrottoir || s.Trottoir == TrottoirTyp.Links);
            abstractValidatorBase.RuleFor(m => m.BreiteTrottoirLinks)
                .InclusiveBetween(0, 30)
                .When(s => s.Trottoir == TrottoirTyp.BeideSeiten || s.Trottoir == TrottoirTyp.Links)
                .NotEmpty()
                .When(s => s.Trottoir == TrottoirTyp.BeideSeiten || s.Trottoir == TrottoirTyp.Links);
            abstractValidatorBase.RuleFor(m => m.BreiteTrottoirRechts)
                .InclusiveBetween(0, 30)
                .When(s => s.Trottoir == TrottoirTyp.BeideSeiten || s.Trottoir == TrottoirTyp.Rechts)
                .NotEmpty()
                .When(s => s.Trottoir == TrottoirTyp.BeideSeiten || s.Trottoir == TrottoirTyp.Rechts);
        }
    }
}