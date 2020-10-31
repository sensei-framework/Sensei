using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Sensei.AspNet.Queries;
using Sensei.AspNet.Queries.Entities;
using Sensei.AspNet.Queries.Exceptions;
using Sensei.AspNet.Queries.QuerySorts;
using Sensei.AspNet.Tests.FakeServer;
using Sensei.AspNet.Tests.FakeServer.Entities;
using Sensei.AspNet.Tests.Utils;
using Xunit;

namespace Sensei.AspNet.Tests.Queries
{
    public class QuerySortsTest
    {
        [Theory]
        [MemberData(nameof(QuerySortsTestCase.TestCases), MemberType = typeof(QuerySortsTestCase))]
        public void Sort(string query, Func<IQueryable<Product>, IQueryable<Product>> func)
        {
            var testServer = TestServerUtils.Create();

            var dbContext = testServer.Services.GetService<FakeServerDbContext>();
            var queryProcessor = testServer.Services.GetService<IQueryProcessor>();

            var expectedResults = func(dbContext.Products).ToList();

            var results = queryProcessor.Start(dbContext.Products)
                .ApplySorting(new Sorting {Sorts = query})
                .Queryable.ToList();

            Assert.True(expectedResults.Compare(results));
        }
        
        [Theory]
        [MemberData(nameof(QuerySortsTestCase.AttributePermissiveTestCases), MemberType = typeof(QuerySortsTestCase))]
        public void SortAttributePermissive(string query, Func<IQueryable<ProductAlt2>, IQueryable<ProductAlt2>> func,
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
                    .ApplySorting(new Sorting {Sorts = query})
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
        [MemberData(nameof(QuerySortsTestCase.AttributeStrictTestCases), MemberType = typeof(QuerySortsTestCase))]
        public void SortAttributeStrict(string query, Func<IQueryable<ProductAlt1>, IQueryable<ProductAlt1>> func,
            bool shouldGenerateException)
        {
            var testServer = TestServerUtils.Create(builder =>
            {
                builder.UseSetting("EnableSortsAsDefault", "false");
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
                    .ApplySorting(new Sorting {Sorts = query})
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
        [MemberData(nameof(QuerySortsTestCase.FluentPermissiveTestCases), MemberType = typeof(QuerySortsTestCase))]
        public void SortFluentPermissive(string query, Func<IQueryable<Product>, IQueryable<Product>> func,
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
                    .ApplySorting(new Sorting {Sorts = query})
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
        [MemberData(nameof(QuerySortsTestCase.FluentPermissiveTestCases), MemberType = typeof(QuerySortsTestCase))]
        public void SortFluentPermissiveCustomContext(string query, Func<IQueryable<Product>, IQueryable<Product>> func,
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
                    .ApplySorting(new Sorting {Sorts = query})
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
        [MemberData(nameof(QuerySortsTestCase.FluentStrictTestCases), MemberType = typeof(QuerySortsTestCase))]
        public void SortFluentStrict(string query, Func<IQueryable<Product>, IQueryable<Product>> func,
            bool shouldGenerateException)
        {
            var testServer = TestServerUtils.Create(builder =>
            {
                builder.UseSetting("FluentStrict", "true");
                builder.UseSetting("EnableSortsAsDefault", "false");
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
                    .ApplySorting(new Sorting {Sorts = query})
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
        [MemberData(nameof(QuerySortsTestCase.FluentStrictTestCases), MemberType = typeof(QuerySortsTestCase))]
        public void SortFluentStrictCustomMapper(string query, Func<IQueryable<Product>, IQueryable<Product>> func,
            bool shouldGenerateException)
        {
            var testServer = TestServerUtils.Create(builder =>
            {
                builder.UseSetting("EnableSortsAsDefault", "false");
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
                    .ApplySorting(new Sorting {Sorts = query})
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
        [MemberData(nameof(QuerySortsTestCase.ExceptionsTestCases), MemberType = typeof(QuerySortsTestCase))]
        public void SortExceptions(string query, Type expectedType)
        {
            var testServer = TestServerUtils.Create();
            var dbContext = testServer.Services.GetService<FakeServerDbContext>();
            var queryProcessor = testServer.Services.GetService<IQueryProcessor>();

            Type exceptionType = null;
            try
            {
                queryProcessor.Start(dbContext.Products)
                    .ApplySorting(new Sorting {Sorts = query});
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