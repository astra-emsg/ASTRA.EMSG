using System;
using System.Linq;
using System.Linq.Expressions;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Common;
using ASTRA.EMSG.Common.Enums;
using Resources;

namespace ASTRA.EMSG.Web.Infrastructure
{
    public class LocalizationService : LocalizationServiceBase, ILocalizationService
    {
        public virtual string GetLocalizedEnum<TEnum>(TEnum enumValue, LocalizationType localizationType = LocalizationType.Default)
        {
            return GetLocalizedEnumByKey(string.Format("{0}_{1}", enumValue.GetType().Name, enumValue), localizationType) ?? enumValue.ToString();
        }

        public virtual string GetLocalizedError(ValidationError validationError)
        {
            return GetLocalizationForKey(typeof(ValidationErrorLocalization).Name, validationError.ToString());
        }

        public TEnum GetEnumFromLocalizedText<TEnum>(string enumValue) where TEnum : struct
        {
            return typeof(TEnum)
                .GetEnumValues()
                .Cast<TEnum>()
                .Single(e => GetLocalizedEnum(e) == enumValue);
        }

        public bool IsEnumExistsForLocalisedText<TEnum>(string enumValue) where TEnum : struct
        {
            return typeof(TEnum).GetEnumValues()
                .Cast<TEnum>()
                .Any(e => GetLocalizedEnum(e) == enumValue);
        }

        public string GetLocalizedBelastungskategorieTyp(string belastungskategorieTyp, LocalizationType localizationType = LocalizationType.Default)
        {
            return GetLocalizedLookupByKey(belastungskategorieTyp, localizationType);
        }

        public string GetLocalizedGemeindeTyp(string gemeindeTyp) { return GetLocalizedLookupByKey(gemeindeTyp); }
        public string GetLocalizedOeffentlicheVerkehrsmittelTyp(string oeffentlicheVerkehrsmittelTyp) { return GetLocalizedLookupByKey(oeffentlicheVerkehrsmittelTyp); }
        public string GetLocalizedMassnahmenvorschlagTyp(string massnahmenvorschlagTyp)
        {
            if (massnahmenvorschlagTyp == null)
            {
                return null;
            }
            return GetLocalizedLookupByKey(massnahmenvorschlagTyp);
        }

        public string GetLocalizedLegendLabel(string label)
        {
            string locLabel = GetLocalizationForKey(typeof(MapLocalization).Name, label);
            if (locLabel == null)
            {
                return "";
            }
            else
            {
                return locLabel;
            }
        }
        public string getNoMeasureLabel { get { return MapLocalization.NoMeasure; } }

        public string GetLocalizedText(string textKey) { return GetLocalizationForKey(typeof(TextLocalization).Name, textKey); }
        public string GetLocalizedTitle(string textKey) { return GetLocalizationForKey(typeof(TitleLocalization).Name, textKey); }

        public string GetLocalizedValue(string resourceName, string resourceKey) { return GetLocalizationForKey(resourceName, resourceKey); }

        public string GetLocalizedReportText(string textKey) { return GetLocalizationForKey(typeof(ReportLocalization).Name, textKey); }

        public virtual string GetLocalizedModelPropertyText<TModel, TProperty>(Expression<Func<TModel, TProperty>> expression) { return GetLocalizedModelPropertyText<TModel>(ExpressionHelper.GetPropertyName(expression)); }
        public virtual string GetLocalizedModelPropertyText<TModel>(string modelPropertyName)
        {
            var result = GetLocalizationForKey(typeof(ModelLocalization).Name, string.Format("{0}_{1}", typeof(TModel).Name, modelPropertyName));
            if (result != null)
                return result;
            return GetLocalizationForKey(typeof(ModelLocalization).Name, modelPropertyName);
        }

        private string GetLocalizedLookupByKey(string lookupKey, LocalizationType localizationType = LocalizationType.Default)
        {
            var localizedLookupByKey = GetLocalizationForKey(typeof(LookupLocalization).Name, lookupKey, localizationType);
            if (string.IsNullOrWhiteSpace(localizedLookupByKey))
                return lookupKey;

            return localizedLookupByKey;
        }

        private string GetLocalizedEnumByKey(string enumKey, LocalizationType localizationType = LocalizationType.Default) { return GetLocalizationForKey(typeof(EnumLocalization).Name, enumKey, localizationType); }

        public virtual string CurrentCultureCode { get { return GetLocalization(() => TextLocalization.CultureCode); } }
    }
}
