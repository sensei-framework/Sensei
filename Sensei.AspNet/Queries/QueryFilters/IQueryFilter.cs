using System;
using System.Linq.Expressions;

namespace Sensei.AspNet.Queries.QueryFilters
{
    public interface IQueryFilter
    {
        Type[] SupportedTypes { get; }
        
        Expression GetCompareExpression(Expression propertyExpression, Type propertyType, string term);
        
        Expression GetExistsExpression(Expression propertyExpression, Type propertyType);

        Expression GetGreaterExpression(Expression propertyExpression, Type propertyType, string term, bool inclusive);
        
        Expression GetLessExpression(Expression propertyExpression, Type propertyType, string term, bool inclusive);
    }
}