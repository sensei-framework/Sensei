using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Sensei.AspNet.Queries;
using Sensei.AspNet.Queries.Entities;
using Sensei.AspNet.Queries.QueryPagination;
using Sensei.AspNet.Tests.FakeServer;
using Sensei.AspNet.Tests.FakeServer.Entities;
using Sensei.AspNet.Tests.Utils;
using Xunit;

namespace Sensei.AspNet.Tests.Queries
{
    public class QueryPaginationTest
    {
        [Theory]
        [MemberData(nameof(QueryPaginationTestCase.TestCases), MemberType = typeof(QueryPaginationTestCase))]
        public void Pagination(int? page, int? pageSize, Func<IQueryable<Product>, Paginator<Product>> func)
        {
            var testServer = TestServerUtils.Create();

            var dbContext = testServer.Services.GetService<FakeServerDbContext>();
            var queryProcessor = testServer.Services.GetService<IQueryProcessor>();

            var expectedResults = func(dbContext.Products);

            var paginator = queryProcessor.Start(dbContext.Products)
                .ApplyPagination(new Pagination {Page = page, PageSize = pageSize});

            Assert.True(expectedResults.Compare(paginator));
        }
    }
}