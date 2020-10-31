using System;
using System.Collections;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Newtonsoft.Json;
using Sensei.AspNet.Queries.Attributes;

namespace Sensei.AspNet.Queries
{
    public class QueryContext : IQueryContext
    {
        private QueryMapper _queryMapper;
        private SenseiOptions _senseiOptions;
        
        protected virtual QueryMapper MapProperties(QueryMapper mapper)
        {
            return mapper;
        }

        internal QueryPropertyInfo Resolve(Type containerType, Expression parentExpression, string propertyName,
            QueryType queryType, SenseiOptions senseiOptions, bool handleEnumerable = false, string fullPath = null)
        {
            while (true)
            {
                _senseiOptions ??= senseiOptions;
                _queryMapper ??= MapProperties(new QueryMapper());

                // we split the property name to get the first property and the rest
                var properties = propertyName.Split('.', 2);
                var property = properties.First();
                var childProperty = properties.Length > 1 ? properties.Last() : null;

                // get the property
                var propertyInfo = GetProperty(containerType, property, queryType);
                if (propertyInfo == null) return null;

                // check the permissions
                // TODO: here we need to check if the user have the permissions to read that property (if don't return null)

                // set the path
                if (fullPath == null)
                    fullPath = propertyInfo.Name;
                else
                    fullPath += $".{propertyInfo.Name}";

                // resolve the expression
                var expression = parentExpression != null ? Expression.Property(parentExpression, propertyInfo) : null;

                // we check if continue or just return the expression
                if (string.IsNullOrEmpty(childProperty))
                    return new QueryPropertyInfo(propertyInfo, expression, fullPath);
                
                // prepare data for next cycle
                if (handleEnumerable && typeof(IEnumerable).IsAssignableFrom(propertyInfo.PropertyType))
                    containerType = propertyInfo.PropertyType.GetGenericArguments()[0];
                else
                    containerType = propertyInfo.PropertyType;
                parentExpression = expression;
                propertyName = childProperty;
            }
        }

        private bool CheckPropertyEnabled(PropertyInfo propertyInfo, QueryType queryType)
        {
            var queryProperty = _queryMapper.GetQueryProperty(propertyInfo.ReflectedType, propertyInfo.Name);
            var queryValue = queryProperty?.GetValue(queryType);
            if (queryValue.HasValue)
                return queryValue.Value;

            switch (queryType)
            {
                case QueryType.Filters:
                    var filterAttribute = propertyInfo.GetCustomAttribute<CanFilterAttribute>();
                    return filterAttribute?.Enabled ?? _senseiOptions.EnableFiltersAsDefault;
                case QueryType.Sorts:
                    var sortAttribute = propertyInfo.GetCustomAttribute<CanSortAttribute>();
                    return sortAttribute?.Enabled ?? _senseiOptions.EnableSortsAsDefault;
                case QueryType.Includes:
                    var includeAttribute = propertyInfo.GetCustomAttribute<CanIncludeAttribute>();
                    return includeAttribute?.Enabled ?? _senseiOptions.EnableIncludesAsDefault;
                default:
                    throw new ArgumentOutOfRangeException(nameof(queryType), queryType, null);
            }
        }

        private PropertyInfo GetProperty(Type containerType, string propertyName, QueryType queryType)
        {
            var properties = containerType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            return (from propertyInfo in properties
                let jsonPropertyAttribute = propertyInfo.GetCustomAttribute<JsonPropertyAttribute>()
                where jsonPropertyAttribute?.PropertyName == propertyName ||
                      propertyInfo.Name.Equals(propertyName, StringComparison.InvariantCultureIgnoreCase)
                select CheckPropertyEnabled(propertyInfo, queryType) ? propertyInfo : null).FirstOrDefault();
        }
    }
}