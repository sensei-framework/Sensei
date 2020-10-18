using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Sensei.AspNet.Queries
{
    public class QueryMapper
    {
        private readonly Dictionary<Type, Dictionary<string, QueryProperty>> _map
            = new Dictionary<Type, Dictionary<string, QueryProperty>>();

        public QueryProperty Property<TEntity>(Expression<Func<TEntity, object>> expression)
        {
            var propertyName = GetPropertyName(expression);
            if (propertyName == null)
                throw new NullReferenceException();
            
            //var propertyInfo = GetPropertyInfo(expression);
            //var propertyType = propertyInfo.PropertyType;
            if (!_map.ContainsKey(typeof(TEntity)))
                _map[typeof(TEntity)] = new Dictionary<string, QueryProperty>();

            var modelMap = _map[typeof(TEntity)];
            
            if (modelMap.ContainsKey(propertyName))
                return modelMap[propertyName];
            
            var queryProperty = new QueryProperty();
            modelMap[propertyName] = queryProperty;
            return queryProperty;
        }

        internal QueryProperty GetQueryProperty(Type type, string propertyName)
        {
            if (!_map.ContainsKey(type))
                return null;

            var modelMap = _map[type];
            return modelMap.ContainsKey(propertyName) ? modelMap[propertyName] : null;
        }
        
        private static string GetPropertyName<TEntity>(Expression<Func<TEntity, object>> exp)
        {
            if (exp.Body is MemberExpression body) return body.Member.Name;
            var ubody = (UnaryExpression)exp.Body;
            body = ubody.Operand as MemberExpression;

            return body?.Member.Name;
            /*
            var member = body?.Member as PropertyInfo;
            
            var stack = new Stack<string>();
            while (body != null)
            {
                stack.Push(body.Member.Name);
                body = body.Expression as MemberExpression;
            }

            return (string.Join(".", stack.ToArray()), member);
            */
        }
    }
}