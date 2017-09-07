using System;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;

namespace ASTRA.EMSG.Common
{
    public class ExpressionHelper
    {
        [DebuggerStepThrough]
        public static string GetPropertyName<T, TProperty>(Expression<Func<T, TProperty>> property)
        {
            return GetMember(property).Name;
        }

        [DebuggerStepThrough]
        public static Type GetPropertyType<T, TProperty>(Expression<Func<T, TProperty>> property)
        {
            return GetMember(property).PropertyType;
        }

        [DebuggerStepThrough]
        public static string GetPropertyName<TProperty>(Expression<Func<TProperty>> property)
        {
            return GetMember(property).Name;
        }

        [DebuggerStepThrough]
        public static Type GetPropertyType<T, TProperty>(Expression<Func<TProperty>> property)
        {
            return GetMember(property).PropertyType;
        }

        private static PropertyInfo GetMember(LambdaExpression lambda)
        {
            MemberExpression memberExpression;
            if (lambda.Body is UnaryExpression)
            {
                var unaryExpression = (UnaryExpression)lambda.Body;
                memberExpression = (MemberExpression)unaryExpression.Operand;
            }
            else memberExpression = (MemberExpression)lambda.Body;

            return memberExpression.Member as PropertyInfo;
        }
    }
}