using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.Extensions.Logging;

namespace Sensei.AspNet.QueryProcessor
{
    /// <summary>
    ///     Process the queries for the EF framework.
    /// </summary>
    public class QueryProcessor
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILoggerFactory _loggerFactory;
        
        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        public QueryProcessor(IServiceProvider serviceProvider, ILoggerFactory loggerFactory)
        {
            _serviceProvider = serviceProvider;
            _loggerFactory = loggerFactory;
        }

        /// <summary>
        ///     Create a new query container.
        /// </summary>
        /// <param name="queryable">The queryable from a db set.</param>
        /// <typeparam name="TEntity">The entity type associated to the query.</typeparam>
        /// <returns>A query container.</returns>
        public Query<TEntity> Start<TEntity>(IQueryable<TEntity> queryable)
        {
            return new Query<TEntity>(queryable, _serviceProvider, _loggerFactory);
        }
        
        internal static (PropertyInfo, Expression) ResolveProperty(Type containerType, Expression parentExpression,
            string propertyName)
        {
            // we split the property name to get the first property and the rest
            var properties = propertyName.Split('.', 2);
            var property = properties.First();
            var childProperty = properties.Length > 1 ? properties.Last() : null;
            
            // get the property
            var propertyInfo = containerType.GetProperty(property,
                BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (propertyInfo == null)
                return (null, null);
            
            // check the permissions
            // TODO: here we need to check if the user have the permissions to read that property (if don't return null)

            // resolve the expression
            var expression = parentExpression != null ? Expression.Property(parentExpression, propertyInfo) : null;
            
            // we check if continue to go recursively or we just return the expression
            return string.IsNullOrEmpty(childProperty)
                ? (propertyInfo, expression)
                : ResolveProperty(propertyInfo.PropertyType, expression, childProperty);
        }
    }
}