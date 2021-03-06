﻿using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace Sensei.AspNet.Queries.QueryFilters.Filters
{
    public class GenericQueryFilter : IQueryFilter
    {
        public Type[] SupportedTypes => new[]
        {
            typeof(Enum),
            typeof(bool),
            typeof(Guid)
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
            // enums doesn't support it
            return null;
        }

        public Expression GetLessExpression(Expression propertyExpression, Type propertyType, string term,
            bool inclusive)
        {
            // enums doesn't support it
            return null;
        }
    }
}