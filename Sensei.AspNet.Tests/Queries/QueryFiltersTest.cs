using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Sensei.AspNet.Queries;
using Sensei.AspNet.Queries.Entities;
using Sensei.AspNet.Queries.Exceptions;
using Sensei.AspNet.Queries.QueryFilters;
using Sensei.AspNet.Tests.FakeServer;
using Sensei.AspNet.Tests.FakeServer.Entities;
using Sensei.AspNet.Tests.Utils;
using Xunit;

namespace Sensei.AspNet.Tests.Queries
{
    public class QueryFiltersTest
    {
        [Theory]
        [MemberData(nameof(QueryFiltersTestCase.TestCases), MemberType = typeof(QueryFiltersTestCase))]
        public void Filters(string query, Func<IQueryable<Product>, IQueryable<Product>> func)
        {
            var testServer = TestServerUtils.Create();

            var dbContext = testServer.Services.GetService<FakeServerDbContext>();
            var queryProcessor = testServer.Services.GetService<IQueryProcessor>();

            var expectedResults = func(dbContext.Products).ToList();

            // if expected result is empty, maybe this test is wrong
            Assert.NotEmpty(expectedResults);

            var results = queryProcessor.Start(dbContext.Products)
                .ApplyFilters(new Filtering {Filters = query})
                .Queryable.ToList();

            Assert.True(expectedResults.Compare(results));
        }

        [Theory]
        [MemberData(nameof(QueryFiltersTestCase.AttributePermissiveTestCases),
            MemberType = typeof(QueryFiltersTestCase))]
        public void FilterAttributePermissive(string query, Func<IQueryable<ProductAlt2>,
            IQueryable<ProductAlt2>> func, bool shouldGenerateException)
        {
            var testServer = TestServerUtils.Create();

            var dbContext = testServer.Services.GetService<FakeServerDbContext>();
            var queryProcessor = testServer.Services.GetService<IQueryProcessor>();

            var expectedResults = func(dbContext.ProductsAlt2).ToList();

            // if expected result is empty, maybe this test is wrong
            Assert.NotEmpty(expectedResults);

            List<ProductAlt2> results = null;
            var haveException = false;
            try
            {
                results = queryProcessor.Start(dbContext.ProductsAlt2)
                    .ApplyFilters(new Filtering {Filters = query})
                    .Queryable.ToList();
            }
            catch (MissingPropertyException)
            {
                haveException = true;
            }

            Assert.Equal(shouldGenerateException, haveException);

            if (!shouldGenerateException)
                Assert.True(expectedResults.Compare(results));
        }

        [Theory]
        [MemberData(nameof(QueryFiltersTestCase.AttributeStrictTestCases), MemberType = typeof(QueryFiltersTestCase))]
        public void FilterAttributeStrict(string query, Func<IQueryable<ProductAlt1>,
            IQueryable<ProductAlt1>> func, bool shouldGenerateException)
        {
            var testServer = TestServerUtils.Create(builder =>
            {
                builder.UseSetting("EnableFiltersAsDefault", "false");
                return builder;
            });

            var dbContext = testServer.Services.GetService<FakeServerDbContext>();
            var queryProcessor = testServer.Services.GetService<IQueryProcessor>();

            var expectedResults = func(dbContext.ProductsAlt1).ToList();

            // if expected result is empty, maybe this test is wrong
            Assert.NotEmpty(expectedResults);

            List<ProductAlt1> results = null;
            var haveException = false;
            try
            {
                results = queryProcessor.Start(dbContext.ProductsAlt1)
                    .ApplyFilters(new Filtering {Filters = query})
                    .Queryable.ToList();
            }
            catch (MissingPropertyException)
            {
                haveException = true;
            }

            Assert.Equal(shouldGenerateException, haveException);

            if (!shouldGenerateException)
                Assert.True(expectedResults.Compare(results));
        }

        [Theory]
        [MemberData(nameof(QueryFiltersTestCase.FluentPermissiveTestCases), MemberType = typeof(QueryFiltersTestCase))]
        public void FilterFluentPermissive(string query, Func<IQueryable<Product>,
            IQueryable<Product>> func, bool shouldGenerateException)
        {
            var testServer = TestServerUtils.Create(builder =>
            {
                builder.UseSetting("FluentPermissive", "true");
                return builder;
            });
            var dbContext = testServer.Services.GetService<FakeServerDbContext>();
            var queryProcessor = testServer.Services.GetService<IQueryProcessor>();

            var expectedResults = func(dbContext.Products).ToList();

            // if expected result is empty, maybe this test is wrong
            Assert.NotEmpty(expectedResults);

            List<Product> results = null;
            var haveException = false;
            try
            {
                results = queryProcessor.Start(dbContext.Products)
                    .ApplyFilters(new Filtering {Filters = query})
                    .Queryable.ToList();
            }
            catch (MissingPropertyException)
            {
                haveException = true;
            }

            Assert.Equal(shouldGenerateException, haveException);

            if (!shouldGenerateException)
                Assert.True(expectedResults.Compare(results));
        }

        [Theory]
        [MemberData(nameof(QueryFiltersTestCase.FluentPermissiveTestCases), MemberType = typeof(QueryFiltersTestCase))]
        public void FilterFluentPermissiveCustomMapper(string query, Func<IQueryable<Product>,
            IQueryable<Product>> func, bool shouldGenerateException)
        {
            var testServer = TestServerUtils.Create();
            var dbContext = testServer.Services.GetService<FakeServerDbContext>();
            var queryProcessor = testServer.Services.GetService<IQueryProcessor>();

            var expectedResults = func(dbContext.Products).ToList();

            // if expected result is empty, maybe this test is wrong
            Assert.NotEmpty(expectedResults);

            List<Product> results = null;
            var haveException = false;
            try
            {
                results = queryProcessor.Start(dbContext.Products, new FluentPermissiveQueryContext())
                    .ApplyFilters(new Filtering {Filters = query})
                    .Queryable.ToList();
            }
            catch (MissingPropertyException)
            {
                haveException = true;
            }

            Assert.Equal(shouldGenerateException, haveException);

            if (!shouldGenerateException)
                Assert.True(expectedResults.Compare(results));
        }

        [Theory]
        [MemberData(nameof(QueryFiltersTestCase.FluentStrictTestCases), MemberType = typeof(QueryFiltersTestCase))]
        public void FilterFluentStrict(string query, Func<IQueryable<Product>,
            IQueryable<Product>> func, bool shouldGenerateException)
        {
            var testServer = TestServerUtils.Create(builder =>
            {
                builder.UseSetting("EnableFiltersAsDefault", "false");
                builder.UseSetting("FluentStrict", "true");
                return builder;
            });
            var dbContext = testServer.Services.GetService<FakeServerDbContext>();
            var queryProcessor = testServer.Services.GetService<IQueryProcessor>();

            var expectedResults = func(dbContext.Products).ToList();

            // if expected result is empty, maybe this test is wrong
            Assert.NotEmpty(expectedResults);

            List<Product> results = null;
            var haveException = false;
            try
            {
                results = queryProcessor.Start(dbContext.Products)
                    .ApplyFilters(new Filtering {Filters = query})
                    .Queryable.ToList();
            }
            catch (MissingPropertyException)
            {
                haveException = true;
            }

            Assert.Equal(shouldGenerateException, haveException);

            if (!shouldGenerateException)
                Assert.True(expectedResults.Compare(results));
        }

        [Theory]
        [MemberData(nameof(QueryFiltersTestCase.FluentStrictTestCases), MemberType = typeof(QueryFiltersTestCase))]
        public void FilterFluentStrictCustomMapper(string query, Func<IQueryable<Product>,
            IQueryable<Product>> func, bool shouldGenerateException)
        {
            var testServer = TestServerUtils.Create(builder =>
            {
                builder.UseSetting("EnableFiltersAsDefault", "false");
                return builder;
            });
            var dbContext = testServer.Services.GetService<FakeServerDbContext>();
            var queryProcessor = testServer.Services.GetService<IQueryProcessor>();

            var expectedResults = func(dbContext.Products).ToList();

            // if expected result is empty, maybe this test is wrong
            Assert.NotEmpty(expectedResults);

            List<Product> results = null;
            var haveException = false;
            try
            {
                results = queryProcessor.Start(dbContext.Products, new FluentStrictQueryContext())
                    .ApplyFilters(new Filtering {Filters = query})
                    .Queryable.ToList();
            }
            catch (MissingPropertyException)
            {
                haveException = true;
            }

            Assert.Equal(shouldGenerateException, haveException);

            if (!shouldGenerateException)
                Assert.True(expectedResults.Compare(results));
        }

        [Theory]
        [MemberData(nameof(QueryFiltersTestCase.ExceptionsTestCases), MemberType = typeof(QueryFiltersTestCase))]
        public void FilterExceptions(string query, Type expectedType)
        {
            var testServer = TestServerUtils.Create();
            var dbContext = testServer.Services.GetService<FakeServerDbContext>();
            var queryProcessor = testServer.Services.GetService<IQueryProcessor>();

            Type exceptionType = null;
            try
            {
                queryProcessor.Start(dbContext.Products)
                    .ApplyFilters(new Filtering {Filters = query});
            }
            catch (Exception e)
            {
                exceptionType = e.GetType();
            }

            Assert.NotNull(exceptionType);
            Assert.Equal(expectedType, exceptionType);
        }
    }
}