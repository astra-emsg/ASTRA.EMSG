using System;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Services.Common;
using FluentValidation;

namespace ASTRA.EMSG.Business.Infrastructure.Validators
{
    public abstract class AbstractValidatorBase<TModel> : AbstractValidator<TModel>
    {
        public ILocalizationService LocalizationService { get; private set; }

        protected Regex stringRegex = new Regex(@"</?\w+((\s+\w+(\s*=\s*(?:"".*?""|'.*?'|[^'"">\s]+))?)+\s*|\s*)/?>",RegexOptions.Singleline);

        protected AbstractValidatorBase(ILocalizationService localizationService)
        {
            LocalizationService = localizationService;
        }

        protected internal IRuleBuilderOptions<TModel, string> RuleForNullableString(Expression<Func<TModel, string>> expression)
        {
            return RuleFor(expression).Length(0, Defaults.MaxStringLength).Must(s => string.IsNullOrWhiteSpace(s) || !stringRegex.IsMatch(s))
                .WithMessage(LocalizationService.GetLocalizedError(ValidationError.HtmlTagsNotAllowed)); ;
        }

        protected internal IRuleBuilderOptions<TModel, string> RuleForNotNullableString(Expression<Func<TModel, string>> expression)
        {
            return RuleFor(expression).NotEmpty().Length(1, Defaults.MaxStringLength).Must(s => string.IsNullOrWhiteSpace(s) || !stringRegex.IsMatch(s))
                .WithMessage(LocalizationService.GetLocalizedError(ValidationError.HtmlTagsNotAllowed)); ;
        }

        protected internal IRuleBuilderOptions<TModel, string> RuleForNullableLongString(Expression<Func<TModel, string>> expression)
        {
            return RuleFor(expression).Length(0, Defaults.MaxTextBlockLength).Must(s => string.IsNullOrWhiteSpace(s) || !stringRegex.IsMatch(s))
                .WithMessage(LocalizationService.GetLocalizedError(ValidationError.HtmlTagsNotAllowed)); ;
        }

        protected internal IRuleBuilderOptions<TModel, string> RuleForNotNullableLongString(Expression<Func<TModel, string>> expression)
        {
            return RuleFor(expression).NotEmpty().Length(0, Defaults.MaxTextBlockLength).Must(s => string.IsNullOrWhiteSpace(s) || !stringRegex.IsMatch(s))
                .WithMessage(LocalizationService.GetLocalizedError(ValidationError.HtmlTagsNotAllowed)); ;
        }

        protected IRuleBuilderOptions<TModel, int> RuleForNotNullableInt(Expression<Func<TModel, int>> expression)
        {
            return RuleFor(expression).NotNull().ShouldNotBeNegative(LocalizationService).InclusiveBetween(0, Defaults.MaxInt);
        }
        
        protected IRuleBuilderOptions<TModel, int?> RuleForNotNullableInt(Expression<Func<TModel, int?>> expression)
        {
            return RuleFor(expression).NotNull().ShouldNotBeNegative(LocalizationService).InclusiveBetween(0, Defaults.MaxInt);
        }

        protected internal IRuleBuilderOptions<TModel, int?> RuleForNullableInt(Expression<Func<TModel, int?>> expression)
        {
            return RuleFor(expression).ShouldNotBeNegative(LocalizationService).InclusiveBetween(0, Defaults.MaxInt);
        }

        protected internal IRuleBuilderOptions<TModel, decimal?> RuleForNullableDecimal(Expression<Func<TModel, decimal?>> expression)
        {
            return RuleForNullableDecimal(expression, 2);
        }

        protected IRuleBuilderOptions<TModel, decimal?> RuleForNullableDecimal(Expression<Func<TModel, decimal?>> expression, int decimalPlaces)
        {
            return RuleFor(expression).ShouldNotBeNegative(LocalizationService).InclusiveBetween(0, Defaults.MaxDecimal).IsValidDecimalWithDecimalPlaces(decimalPlaces, LocalizationService);
        }

        protected IRuleBuilderOptions<TModel, decimal> RuleForNotNullableDecimal(Expression<Func<TModel, decimal>> expression)
        {
            return RuleForNotNullableDecimal(expression, 2);
        }

        protected IRuleBuilderOptions<TModel, decimal?> RuleForNotNullableDecimal(Expression<Func<TModel, decimal?>> expression, int decimalPlaces)
        {
            return RuleFor(expression).NotNull().ShouldNotBeNegative(LocalizationService).InclusiveBetween(0, Defaults.MaxDecimal).IsValidDecimalWithDecimalPlaces(decimalPlaces, LocalizationService);
        }

        protected IRuleBuilderOptions<TModel, decimal> RuleForNotNullableDecimal(Expression<Func<TModel, decimal>> expression, int decimalPlaces)
        {
            return RuleFor(expression).NotNull().ShouldNotBeNegative(LocalizationService).InclusiveBetween(0, Defaults.MaxDecimal).IsValidDecimalWithDecimalPlaces(decimalPlaces, LocalizationService);
        }
    }
}