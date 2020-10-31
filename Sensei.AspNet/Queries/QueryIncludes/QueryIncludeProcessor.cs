using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sensei.AspNet.Queries.Entities;
using Sensei.AspNet.Queries.Exceptions;

namespace Sensei.AspNet.Queries.QueryIncludes
{
    public static class QueryIncludeProcessor
    {
        public static Query<TEntity> ApplyIncluding<TEntity>(this Query<TEntity> query, Including including)
            where TEntity : class
        {
            if (string.IsNullOrEmpty(including?.Includes))
                return query;

            var queryable = query.Queryable;
            var logger = query.LoggerFactory.CreateLogger(Const.LoggerName);
            var queryContext = query.QueryContext;
            var serviceProvider = query.ServiceProvider;
            var options = serviceProvider.GetService<SenseiOptions>();

            foreach (var item in including.Includes.Split(','))
            {
                var include = item.Trim();
                
                // we resolve the property expression just to check the permissions
                var queryPropertyInfo =
                    queryContext.Resolve(typeof(TEntity), null, include, QueryType.Includes,
                        serviceProvider.GetService<SenseiOptions>(), true);

                // we skip if we can't find the property
                if (queryPropertyInfo?.PropertyInfo == null)
                {
                    logger.LogError("Property for field {field} not found", include);

                    if (options.ThrowExceptionOnQueryError)
                        throw new MissingPropertyException($"Property for field {include} not found");

                    continue;
                }
                
                queryable = queryable.Include(queryPropertyInfo.FullPath);
            }

            if (query.Queryable != queryable)
                query.Queryable = queryable;
            
            return query;
        }
    }
}