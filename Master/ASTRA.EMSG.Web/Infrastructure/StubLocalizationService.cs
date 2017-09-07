using System;
using System.Linq.Expressions;
using ASTRA.EMSG.Common;
using ASTRA.EMSG.Common.Enums;

namespace ASTRA.EMSG.Web.Infrastructure
{
    public class StubLocalizationService : LocalizationService
    {
        public override string GetLocalizedEnum<TEnum>(TEnum enumValue, LocalizationType localizationType = LocalizationType.Default)
        {
            return enumValue.ToString();
        }
        
        protected override string GetLocalization(Expression<Func<string>> resourceAccessor)
        {
            return ExpressionHelper.GetPropertyName(resourceAccessor);
        }

        protected override string GetLocalizationForKey(string classKey, string resourceEntryKey)
        {
            return resourceEntryKey;
        }

        public override string CurrentCultureCode { get { return "de-CH"; } }
    }

    public class StubReportLocalizationService : ReportLocalizationService
    {
        protected override string GetLocalization(Expression<Func<string>> resourceAccessor)
        {
            return ExpressionHelper.GetPropertyName(resourceAccessor);
        }

        protected override string GetLocalizationForKey(string classKey, string resourceEntryKey)
        {
            return resourceEntryKey;
        }
    }
}