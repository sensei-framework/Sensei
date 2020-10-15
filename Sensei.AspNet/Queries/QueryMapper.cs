using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Sensei.AspNet.Queries
{
    public class QueryMapper
    {
        private readonly Dictionary<Type, QueryProperty> _map
            = new Dictionary<Type, QueryProperty>();

        public QueryProperty Property<TEntity>(Expression<Func<TEntity, object>> expression)
        {
            if (_map.ContainsKey(typeof(TEntity)))
                return _map[typeof(TEntity)];
            
            var queryProperty = new QueryProperty();
            _map.Add(typeof(TEntity), queryProperty);
            return queryProperty;
        }

        internal QueryProperty GetQueryProperty(Type type)
        {
            return _map.ContainsKey(type) ? _map[type] : null;
        }
    }
}