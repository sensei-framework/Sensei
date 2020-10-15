using System.Linq;

namespace Sensei.AspNet.Queries
{
    public interface IQueryProcessor
    {
        Query<TEntity> Start<TEntity>(IQueryable<TEntity> queryable, IQueryContext queryContext = null);
    }
}