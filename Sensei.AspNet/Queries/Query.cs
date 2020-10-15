using System;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace Sensei.AspNet.Queries
{
    /// <summary>
    ///     This is a container for an IQueryable expression that also store an IServiceProvider inside.
    /// </summary>
    /// <typeparam name="TEntity">The entity type associated to the query.</typeparam>
    public class Query<TEntity>
    {
        /// <summary>
        ///     The queryable expression.
        /// </summary>
        public IQueryable<TEntity> Queryable { get; set; }
        
        /// <summary>
        ///     The service provider.
        /// </summary>
        public IServiceProvider ServiceProvider { get; }

        internal ILoggerFactory LoggerFactory { get; }
        
        internal QueryContext QueryContext { get; }

        internal Query(IQueryable<TEntity> queryable, IServiceProvider serviceProvider,
            ILoggerFactory loggerFactory, QueryContext queryContext)
        {
            Queryable = queryable;
            ServiceProvider = serviceProvider;
            LoggerFactory = loggerFactory;
            QueryContext = queryContext;
        }
    }
}