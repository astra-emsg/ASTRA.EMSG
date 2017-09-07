using System;
using System.Linq.Expressions;

namespace ASTRA.EMSG.IntegrationTests.Support
{
    public static class ExpressionExtensions
    {
        public static string GetPropertyName<TEntity, TValue>(this Expression<Func<TEntity, TValue>> property)
        {
            var exp = (LambdaExpression)property;

            var mExp = (exp.Body.NodeType == ExpressionType.MemberAccess)
                ? (MemberExpression)exp.Body
                : (MemberExpression)((UnaryExpression)exp.Body).Operand;
            return mExp.Member.Name;
        }
    }
}