using Sensei.AspNet.Queries;
using Sensei.AspNet.Tests.FakeServer.Entities;

namespace Sensei.AspNet.Tests.FakeServer
{
    public class FluentPermissiveQueryContext : QueryContext
    {
        protected override QueryMapper MapProperties(QueryMapper mapper)
        {
            mapper.Property<Product>(p => p.Name)
                .CanFilter(false)
                .CanSort(false);

            mapper.Property<Product>(p => p.Enabled)
                .CanFilter(false)
                .CanSort(false);

            mapper.Property<Product>(p => p.CategoryAlt)
                .CanInclude(false);

            return mapper;
        }
    }
}