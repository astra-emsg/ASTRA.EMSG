using System;
using System.Linq.Expressions;
using ASTRA.EMSG.Common.Enums;

namespace ASTRA.EMSG.Business.Services.Common
{
    public interface ILocalizationService
    {
        string GetLocalizedEnum<TEnum>(TEnum e, LocalizationType localizationType = LocalizationType.Default);
        string GetLocalizedError(ValidationError validationError);
        TEnum GetEnumFromLocalizedText<TEnum>(string enumValue) where TEnum : struct;
        bool IsEnumExistsForLocalisedText<TEnum>(string enumValue) where TEnum : struct;
        string GetLocalizedBelastungskategorieTyp(string belastungskategorieTyp, LocalizationType localizationType = LocalizationType.Default);
        string GetLocalizedMassnahmenvorschlagTyp(string massnahmenvorschlagTyp);
        string GetLocalizedText(string textKey);
        string GetLocalizedReportText(string textKey);
        string CurrentCultureCode { get; }
        string GetLocalizedValue(string resourceName, string resourceKey);
        string GetLocalizedModelPropertyText<TModel, TProperty>(Expression<Func<TModel, TProperty>> expression);
        string GetLocalizedModelPropertyText<TModel>(string modelPropertyName);
        string GetLocalizedGemeindeTyp(string gemeindeTyp);
        string GetLocalizedOeffentlicheVerkehrsmittelTyp(string oeffentlicheVerkehrsmittelTyp);
        string GetLocalizedLegendLabel(string label);
        string GetLocalizedTitle(string textKey);
        string getNoMeasureLabel { get; }
    }
}

    