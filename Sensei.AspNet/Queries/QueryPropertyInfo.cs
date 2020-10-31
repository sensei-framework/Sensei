using System.Linq.Expressions;
using System.Reflection;

namespace Sensei.AspNet.Queries
{
    internal class QueryPropertyInfo
    {
        public QueryPropertyInfo(PropertyInfo propertyInfo, Expression expression, string fullPath)
        {
            PropertyInfo = propertyInfo;
            Expression = expression;
            FullPath = fullPath;
        }
        
        public PropertyInfo PropertyInfo { get; }
        public Expression Expression { get; }
        public string FullPath { get; }
    }
}