using System;
using System.Collections.Generic;
using System.Linq;
using Sensei.AspNet.Queries.Entities;
using Sensei.AspNet.Tests.FakeServer.Entities;

namespace Sensei.AspNet.Tests.Queries
{
    public class QueryPaginationTestCase
    {
        public static IEnumerable<object[]> TestCases
        {
            get
            {
                yield return new object[]
                {
                    null,
                    null,
                    (Func<IQueryable<Product>, Paginator<Product>>) (x => new Paginator<Product>
                    {
                        Page = 1,
                        PageSize = 20,
                        Items = x.Take(20).ToList(),
                        Total = x.Count(),
                        HavePrev = false,
                        HaveNext = x.Count() > 20
                    })
                };

                yield return new object[]
                {
                    null,
                    10,
                    (Func<IQueryable<Product>, Paginator<Product>>) (x => new Paginator<Product>
                    {
                        Page = 1,
                        PageSize = 10,
                        Items = x.Take(10).ToList(),
                        Total = x.Count(),
                        HavePrev = false,
                        HaveNext = x.Count() > 20
                    })
                };

                yield return new object[]
                {
                    null,
                    20,
                    (Func<IQueryable<Product>, Paginator<Product>>) (x => new Paginator<Product>
                    {
                        Page = 1,
                        PageSize = 20,
                        Items = x.Take(20).ToList(),
                        Total = x.Count(),
                        HavePrev = false,
                        HaveNext = x.Count() > 20
                    })
                };

                yield return new object[]
                {
                    null,
                    500,
                    (Func<IQueryable<Product>, Paginator<Product>>) (x => new Paginator<Product>
                    {
                        Page = 1,
                        PageSize = 100,
                        Items = x.Take(100).ToList(),
                        Total = x.Count(),
                        HavePrev = false,
                        HaveNext = x.Count() > 100
                    })
                };

                yield return new object[]
                {
                    null,
                    0,
                    (Func<IQueryable<Product>, Paginator<Product>>) (x => new Paginator<Product>
                    {
                        Page = 1,
                        PageSize = 100,
                        Items = x.Take(100).ToList(),
                        Total = x.Count(),
                        HavePrev = false,
                        HaveNext = x.Count() > 100
                    })
                };

                yield return new object[]
                {
                    1,
                    5,
                    (Func<IQueryable<Product>, Paginator<Product>>) (x => new Paginator<Product>
                    {
                        Page = 1,
                        PageSize = 5,
                        Items = x.Take(5).ToList(),
                        Total = x.Count(),
                        HavePrev = false,
                        HaveNext = x.Count() > 5
                    })
                };
                yield return new object[]
                {
                    2,
                    5,
                    (Func<IQueryable<Product>, Paginator<Product>>) (x => new Paginator<Product>
                    {
                        Page = 2,
                        PageSize = 5,
                        Items = x.Skip(5).Take(5).ToList(),
                        Total = x.Count(),
                        HavePrev = true,
                        HaveNext = x.Count() > 10
                    })
                };
                yield return new object[]
                {
                    6,
                    5,
                    (Func<IQueryable<Product>, Paginator<Product>>) (x => new Paginator<Product>
                    {
                        Page = 6,
                        PageSize = 5,
                        Items = x.Skip(5 * 5).Take(5).ToList(),
                        Total = x.Count(),
                        HavePrev = true,
                        HaveNext = false
                    })
                };
            }
        }
    }
}