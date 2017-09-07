using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using ExpressionHelper = ASTRA.EMSG.Common.ExpressionHelper;

namespace ASTRA.EMSG.Web.Infrastructure
{
    public static class ModellStateDictionaryExtensions
    {
        public static void AddModelError<T, TProperty>(this ModelStateDictionary modelStateDictionary, Expression<Func<T, TProperty>> property, string errorMessage)
        {
            modelStateDictionary.AddModelError(ExpressionHelper.GetPropertyName(property), errorMessage);
        }

        public static void AddModelError<TProperty>(this ModelStateDictionary modelStateDictionary, Expression<Func<TProperty>> property, string errorMessage)
        {
            modelStateDictionary.AddModelError(ExpressionHelper.GetPropertyName(property), errorMessage);
        }
    }
}