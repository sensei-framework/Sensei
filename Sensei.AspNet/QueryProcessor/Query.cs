using System;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace Sensei.AspNet.QueryProcessor
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

        /// <summary>
        ///     The logger factory.
        /// </summary>
        public ILoggerFactory LoggerFactory { get; }

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="queryable">The queryable expression.</param>
        /// <param name="serviceProvider">The service provider.</param>
        public Query(IQueryable<TEntity> queryable, IServiceProvider serviceProvider, ILoggerFactory loggerFactory)
        {
            Queryable = queryable;
            ServiceProvider = serviceProvider;
            LoggerFactory = loggerFactory;
        }
    }
}