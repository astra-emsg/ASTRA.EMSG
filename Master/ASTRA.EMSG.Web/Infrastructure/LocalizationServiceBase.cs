using System;
using System.Linq.Expressions;
using System.Web;
using ASTRA.EMSG.Common.Enums;

namespace ASTRA.EMSG.Web.Infrastructure
{
    public abstract class LocalizationServiceBase
    {
        protected virtual string GetLocalization(Expression<Func<string>> resourceAccessor)
        {
            MemberExpression memberExpression;
            if (resourceAccessor.Body is UnaryExpression)
            {
                var unaryExpression = (UnaryExpression)resourceAccessor.Body;
                memberExpression = (MemberExpression)unaryExpression.Operand;
            }
            else memberExpression = (MemberExpression)resourceAccessor.Body;


            string classKey = memberExpression.Member.DeclaringType.Name;
            string resourceKey = memberExpression.Member.Name;

            return GetLocalizationForKey(classKey, resourceKey);
        }

        protected string GetLocalizationForKey(string classKey, string resourceEntryKey, LocalizationType localizationType)
        {
            var localizedValue = GetLocalizationForKey(classKey, string.Format("{0}_{1}", localizationType, resourceEntryKey));
            if (localizationType == LocalizationType.Default || string.IsNullOrEmpty(localizedValue))
                return GetLocalizationForKey(classKey, resourceEntryKey);

            return localizedValue;
        }

        protected virtual string GetLocalizationForKey(string classKey, string resourceEntryKey)
        {
            return (string)HttpContext.GetGlobalResourceObject(classKey, resourceEntryKey);
        }
    }
}