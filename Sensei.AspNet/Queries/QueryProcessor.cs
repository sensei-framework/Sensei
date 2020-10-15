using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Sensei.AspNet.Queries
{
    /// <summary>
    ///     Process the queries for the EF framework.
    /// </summary>
    public class QueryProcessor : IQueryProcessor
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILoggerFactory _loggerFactory;

        private QueryMapper _queryMapper;
        
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
        public Query<TEntity> Start<TEntity>(IQueryable<TEntity> queryable, IQueryContext queryContext = null)
        {
            queryContext ??= _serviceProvider.GetService<IQueryContext>();
            queryContext ??= new QueryContext();
            
            if (!(queryContext is QueryContext))
                throw new NotSupportedException();

            return new Query<TEntity>(queryable, _serviceProvider, _loggerFactory, (QueryContext) queryContext);
        }
    }
}