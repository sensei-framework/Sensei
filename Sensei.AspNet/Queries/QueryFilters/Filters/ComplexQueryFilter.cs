using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace Sensei.AspNet.Queries.QueryFilters.Filters
{
    public class ComplexQueryFilter : IQueryFilter
    {
        public Type[] SupportedTypes => new[]
        {
            typeof(decimal),
            typeof(int),
            typeof(long),
            typeof(double),
            typeof(float),
            typeof(DateTime),
            typeof(DateTimeOffset),
            typeof(TimeSpan)
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