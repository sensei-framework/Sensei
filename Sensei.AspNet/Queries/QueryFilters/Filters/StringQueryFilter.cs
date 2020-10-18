using System;
using System.Linq.Expressions;

namespace Sensei.AspNet.Queries.QueryFilters.Filters
{
    public class StringQueryFilter : IQueryFilter
    {
        public Type[] SupportedTypes => new[]
        {
            typeof(string)
        };

        public Expression GetCompareExpression(Expression propertyExpression, Type propertyType, string term)
        {
            var termExpression = Expression.Constant(term, typeof(string));
            return ExpressionExt.Like(propertyExpression, termExpression);
        }

        public Expression GetExistsExpression(Expression propertyExpression, Type propertyType)
        {
            return Expression.NotEqual(propertyExpression, Expression.Constant(null));
        }

        public Expression GetGreaterExpression(Expression propertyExpression, Type propertyType, string term,
            bool inclusive)
        {
            // strings doesn't support it
            return null;
        }

        public Expression GetLessExpression(Expression propertyExpression, Type propertyType, string term,
            bool inclusive)
        {
            // strings doesn't support it
            return null;
        }
    }
}