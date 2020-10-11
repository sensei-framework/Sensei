using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace Sensei.AspNet.QueryProcessor.QueryFilters.Filters
{
    public class NumericQueryFilter : IQueryFilter
    {
        public Type[] SupportedTypes => new[]
        {
            typeof(decimal), typeof(decimal?),
            typeof(int), typeof(int?),
            typeof(long), typeof(long?),
            typeof(double), typeof(double?),
            typeof(float), typeof(float?)
        };

        public Expression GetCompareExpression(Expression propertyExpression, Type propertyType, string term)
        {
            var value = TypeDescriptor.GetConverter(propertyType).ConvertFromInvariantString(term);
            var valueExpression = Expression.Constant(value, propertyType);
            return Expression.Equal(propertyExpression, valueExpression);
        }

        public Expression GetExistsExpression(Expression propertyExpression, Type propertyType)
        {
            return Expression.NotEqual(propertyExpression, Expression.Constant(null));
        }

        public Expression GetGreaterExpression(Expression propertyExpression, Type propertyType, string term,
            bool inclusive)
        {
            var value = TypeDescriptor.GetConverter(propertyType).ConvertFromInvariantString(term);
            var valueExpression = Expression.Constant(value, propertyType);

            return inclusive
                ? Expression.GreaterThanOrEqual(propertyExpression, valueExpression)
                : Expression.GreaterThan(propertyExpression, valueExpression);
        }

        public Expression GetLessExpression(Expression propertyExpression, Type propertyType, string term,
            bool inclusive)
        {
            var value = TypeDescriptor.GetConverter(propertyType).ConvertFromInvariantString(term);
            var valueExpression = Expression.Constant(value, propertyType);

            return inclusive
                ? Expression.LessThanOrEqual(propertyExpression, valueExpression)
                : Expression.LessThan(propertyExpression, valueExpression);
        }
    }
}