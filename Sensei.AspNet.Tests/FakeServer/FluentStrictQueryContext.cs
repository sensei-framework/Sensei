using Sensei.AspNet.Queries;
using Sensei.AspNet.Tests.FakeServer.Entities;

namespace Sensei.AspNet.Tests.FakeServer
{
    public class FluentStrictQueryContext : QueryContext
    {
        protected override QueryMapper MapProperties(QueryMapper mapper)
        {
            mapper.Property<Product>(p => p.Name)
                .CanFilter();

            mapper.Property<Product>(p => p.Enabled)
                .CanFilter();

            return mapper;
        }
    }
}