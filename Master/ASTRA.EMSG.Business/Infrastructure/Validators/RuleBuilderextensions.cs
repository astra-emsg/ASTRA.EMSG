using System;
using ASTRA.EMSG.Business.Services.Common;
using FluentValidation;
using FluentValidation.Internal;

namespace ASTRA.EMSG.Business.Infrastructure.Validators
{
    public static class RuleBuilderextensions
    {
        public static IRuleBuilderOptions<TModel, TProperty?> ShouldBeEmpty<TModel, TProperty>(this IRuleBuilder<TModel, TProperty?> ruleBuilderOptions, ILocalizationService localizationService)
            where TProperty: struct 
        {
            return ruleBuilderOptions
                .Must(property => !property.HasValue)
                .WithMessage(string.Format(localizationService.GetLocalizedError(ValidationError.ShouldBeEmpty), GetLocalizedModelPropertyText(ruleBuilderOptions, localizationService)));
        }

        public static IRuleBuilderOptions<TModel, TProperty?> ShouldNotBeEmpty<TModel, TProperty>(this IRuleBuilder<TModel, TProperty?> ruleBuilderOptions, ILocalizationService localizationService)
            where TProperty: struct 
        {
            return ruleBuilderOptions
                .Must(property => property.HasValue)
                .WithMessage(string.Format(localizationService.GetLocalizedError(ValidationError.ShouldNotBeEmpty), GetLocalizedModelPropertyText(ruleBuilderOptions, localizationService)));
        }

        private static string GetLocalizedModelPropertyText<TModel, TProperty>(IRuleBuilder<TModel, TProperty?> ruleBuilderOptions, ILocalizationService localizationService)
            where TProperty: struct
        {
            var ruleBuilder = ruleBuilderOptions as RuleBuilder<TModel, TProperty?>;
            if (ruleBuilder == null)
                return "-";

            return localizationService.GetLocalizedModelPropertyText<TModel>(ruleBuilder.Rule.PropertyName);
        }

        public static IRuleBuilderOptions<TModel, decimal?> ShouldNotBeNegative<TModel>(this IRuleBuilder<TModel, decimal?> ruleBuilderOptions, ILocalizationService localizationService)
        {
            return ruleBuilderOptions.Must(property => !property.HasValue || property >= 0).WithMessage(localizationService.GetLocalizedError(ValidationError.ShouldNotBeNegative));
        }

        public static IRuleBuilderOptions<TModel, decimal> ShouldNotBeNegative<TModel>(this IRuleBuilder<TModel, decimal> ruleBuilderOptions, ILocalizationService localizationService)
        {
            return ruleBuilderOptions.Must(property => property >= 0).WithMessage(localizationService.GetLocalizedError(ValidationError.ShouldNotBeNegative));
        }

        public static IRuleBuilderOptions<TModel, int?> ShouldNotBeNegative<TModel>(this IRuleBuilder<TModel, int?> ruleBuilderOptions, ILocalizationService localizationService)
        {
            return ruleBuilderOptions.Must(property => !property.HasValue || property >= 0).WithMessage(localizationService.GetLocalizedError(ValidationError.ShouldNotBeNegative));
        }

        public static IRuleBuilderOptions<TModel, int> ShouldNotBeNegative<TModel>(this IRuleBuilder<TModel, int> ruleBuilderOptions, ILocalizationService localizationService)
        {
            return ruleBuilderOptions.Must(property => property >= 0).WithMessage(localizationService.GetLocalizedError(ValidationError.ShouldNotBeNegative));
        }

        public static IRuleBuilderOptions<TModel, decimal?> IsValidDecimalWithDecimalPlaces<TModel>(this IRuleBuilderOptions<TModel, decimal?> ruleBuilderOptions, int decimalPlaces, ILocalizationService localizationService)
        {
            return ruleBuilderOptions
                .Must(property => IsValidDecimalWithDecimalPlaces(property, decimalPlaces))
                .WithMessage(AllowedDecimalPlacesValidationMessage(decimalPlaces, localizationService));
        }

        public static IRuleBuilderOptions<TModel, decimal> IsValidDecimalWithDecimalPlaces<TModel>(this IRuleBuilderOptions<TModel, decimal> ruleBuilderOptions, int decimalPlaces, ILocalizationService localizationService)
        {
            return ruleBuilderOptions
                .Must(property => IsValidDecimalWithDecimalPlaces(property, decimalPlaces))
                .WithMessage(AllowedDecimalPlacesValidationMessage(decimalPlaces, localizationService));
        }

        public static IRuleBuilderOptions<TModel, decimal?> IsValidDecimalWithDecimalPlaces<TModel>(this IRuleBuilderInitial<TModel, decimal?> ruleBuilderInitial, int decimalPlaces, ILocalizationService localizationService)
        {
            return ruleBuilderInitial
                .Must(property => IsValidDecimalWithDecimalPlaces(property, decimalPlaces))
                .WithMessage(AllowedDecimalPlacesValidationMessage(decimalPlaces, localizationService));
        }

        private static string AllowedDecimalPlacesValidationMessage(int decimalPlaces, ILocalizationService localizationService)
        {
            return string.Format(localizationService.GetLocalizedError(ValidationError.AllowedDecimalPlaces), decimalPlaces);
        }

        private static bool IsValidDecimalWithDecimalPlaces(decimal? property, int decimalPlaces)
        {
            if(!property.HasValue)
                return true;

            decimal value = property.Value * Convert.ToInt64(Math.Pow(10, decimalPlaces));

            if(value > Int64.MaxValue)
                return false;

            return Convert.ToInt64(value) == value;
        }
    }
}