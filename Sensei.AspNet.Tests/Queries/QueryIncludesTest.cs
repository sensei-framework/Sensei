using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Sensei.AspNet.Queries;
using Sensei.AspNet.Queries.Entities;
using Sensei.AspNet.Queries.Exceptions;
using Sensei.AspNet.Queries.QueryIncludes;
using Sensei.AspNet.Tests.FakeServer;
using Sensei.AspNet.Tests.FakeServer.Entities;
using Sensei.AspNet.Tests.Utils;
using Xunit;

namespace Sensei.AspNet.Tests.Queries
{
    public class QueryIncludesTest
    {
        [Theory]
        [MemberData(nameof(QueryIncludesTestCase.TestCases), MemberType = typeof(QueryIncludesTestCase))]
        public void Include(string query, Func<IQueryable<Product>, IQueryable<Product>> func)
        {
            var testServer = TestServerUtils.Create();

            var dbContext = testServer.Services.GetService<FakeServerDbContext>();
            var queryProcessor = testServer.Services.GetService<IQueryProcessor>();

            var expectedResults = func(dbContext.Products).ToList();

            var results = queryProcessor.Start(dbContext.Products)
                .ApplyIncluding(new Including {Includes = query})
                .Queryable.ToList();

            Assert.True(expectedResults.Compare(results));
        }

        [Theory]
        [MemberData(nameof(QueryIncludesTestCase.AttributePermissiveTestCases),
            MemberType = typeof(QueryIncludesTestCase))]
        public void IncludeAttributePermissive(string query,
            Func<IQueryable<ProductAlt2>, IQueryable<ProductAlt2>> func,
            bool shouldGenerateException)
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
                    .ApplyIncluding(new Including {Includes = query})
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
        [MemberData(nameof(QueryIncludesTestCase.AttributeStrictTestCases), MemberType = typeof(QueryIncludesTestCase))]
        public void IncludeAttributeStrict(string query, Func<IQueryable<ProductAlt1>, IQueryable<ProductAlt1>> func,
            bool shouldGenerateException)
        {
            var testServer = TestServerUtils.Create(builder =>
            {
                builder.UseSetting("EnableIncludesAsDefault", "false");
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
                    .ApplyIncluding(new Including {Includes = query})
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
        [MemberData(nameof(QueryIncludesTestCase.FluentPermissiveTestCases),
            MemberType = typeof(QueryIncludesTestCase))]
        public void IncludeFluentPermissive(string query, Func<IQueryable<Product>, IQueryable<Product>> func,
            bool shouldGenerateException)
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
                    .ApplyIncluding(new Including {Includes = query})
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
        [MemberData(nameof(QueryIncludesTestCase.FluentPermissiveTestCases),
            MemberType = typeof(QueryIncludesTestCase))]
        public void IncludeFluentPermissiveCustomContext(string query,
            Func<IQueryable<Product>, IQueryable<Product>> func,
            bool shouldGenerateException)
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
                    .ApplyIncluding(new Including {Includes = query})
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
        [MemberData(nameof(QueryIncludesTestCase.FluentStrictTestCases), MemberType = typeof(QueryIncludesTestCase))]
        public void IncludeFluentStrict(string query, Func<IQueryable<Product>, IQueryable<Product>> func,
            bool shouldGenerateException)
        {
            var testServer = TestServerUtils.Create(builder =>
            {
                builder.UseSetting("FluentStrict", "true");
                builder.UseSetting("EnableIncludesAsDefault", "false");
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
                    .ApplyIncluding(new Including {Includes = query})
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
        [MemberData(nameof(QueryIncludesTestCase.FluentStrictTestCases), MemberType = typeof(QueryIncludesTestCase))]
        public void IncludeFluentStrictCustomMapper(string query, Func<IQueryable<Product>, IQueryable<Product>> func,
            bool shouldGenerateException)
        {
            var testServer = TestServerUtils.Create(builder =>
            {
                builder.UseSetting("EnableIncludesAsDefault", "false");
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
                    .ApplyIncluding(new Including {Includes = query})
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
        [MemberData(nameof(QueryIncludesTestCase.ExceptionsTestCases), MemberType = typeof(QueryIncludesTestCase))]
        public void IncludeExceptions(string query, Type expectedType)
        {
            var testServer = TestServerUtils.Create();
            var dbContext = testServer.Services.GetService<FakeServerDbContext>();
            var queryProcessor = testServer.Services.GetService<IQueryProcessor>();

            Type exceptionType = null;
            try
            {
                queryProcessor.Start(dbContext.Products)
                    .ApplyIncluding(new Including {Includes = query});
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