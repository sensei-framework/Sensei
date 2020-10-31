using Sensei.AspNet.Queries;
using Sensei.AspNet.Tests.FakeServer.Entities;

namespace Sensei.AspNet.Tests.FakeServer
{
    public class FluentStrictQueryContext : QueryContext
    {
        protected override QueryMapper MapProperties(QueryMapper mapper)
        {
            mapper.Property<Product>(p => p.Name)
                .CanFilter()
                .CanSort();

            mapper.Property<Product>(p => p.Enabled)
                .CanFilter()
                .CanSort();

            mapper.Property<Product>(p => p.Category)
                .CanInclude();

            return mapper;
        }
    }
}