using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Sensei.AspNet.Queries.QueryFilters
{
    internal static class ExpressionExt
    {
        public static Expression Like(Expression propertyExpression, Expression constantExpression)
        {
            var method = typeof(DbFunctionsExtensions).GetMethod("Like",
                new[] {typeof(DbFunctions), typeof(string), typeof(string)});
            if (method == null)
                return null;
            
            var dbFunctionsExpression = Expression.Constant(EF.Functions, typeof(DbFunctions));
            return Expression.Call(method, dbFunctionsExpression, propertyExpression, constantExpression);
        }
    }
}