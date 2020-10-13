using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sensei.AspNet.Models;
using Sensei.AspNet.QueryProcessor.Entities;

namespace Sensei.AspNet.QueryProcessor.QueryIncludes
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
            
            foreach (var item in including.Includes.Split(','))
            {
                var include = item.Trim();
                
                // we resolve the property expression just to check the permissions
                var (propertyInfo, _) =
                    QueryProcessor.ResolveProperty(typeof(TEntity), null, include);

                // we skip if we can't find the property
                if (propertyInfo == null)
                {
                    logger.LogError("Property for field {field} not found", include);
                    continue;
                }
                
                queryable = queryable.Include(include);
            }

            if (query.Queryable != queryable)
                query.Queryable = queryable;
            
            return query;
        }
    }
}