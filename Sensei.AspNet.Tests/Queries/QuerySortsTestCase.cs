using System;
using System.Collections.Generic;
using System.Linq;
using Sensei.AspNet.Queries.Exceptions;
using Sensei.AspNet.Tests.FakeServer.Entities;

namespace Sensei.AspNet.Tests.Queries
{
    public class QuerySortsTestCase
    {
        public static IEnumerable<object[]> TestCases
        {
            get
            {
                yield return new object[]
                {
                    "name",
                    (Func<IQueryable<Product>, IQueryable<Product>>) (x =>
                        x.OrderBy(p => p.Name))
                };
                yield return new object[]
                {
                    "-name",
                    (Func<IQueryable<Product>, IQueryable<Product>>) (x =>
                        x.OrderByDescending(p => p.Name))
                };
                yield return new object[]
                {
                    "availableSince",
                    (Func<IQueryable<Product>, IQueryable<Product>>) (x =>
                        x.OrderBy(p => p.AvailableSince))
                };
                yield return new object[]
                {
                    "-availableSince",
                    (Func<IQueryable<Product>, IQueryable<Product>>) (x =>
                        x.OrderByDescending(p => p.AvailableSince))
                };
                yield return new object[]
                {
                    "category.name",
                    (Func<IQueryable<Product>, IQueryable<Product>>) (x =>
                        x.OrderBy(p => p.Category.Name))
                };
                yield return new object[]
                {
                    "-category.name",
                    (Func<IQueryable<Product>, IQueryable<Product>>) (x =>
                        x.OrderByDescending(p => p.Category.Name))
                };
                yield return new object[]
                {
                    "category.name,-availableSince",
                    (Func<IQueryable<Product>, IQueryable<Product>>) (x =>
                        x.OrderBy(p => p.Category.Name)
                            .ThenByDescending(p => p.AvailableSince))
                };
                yield return new object[]
                {
                    "-category.name,availableSince",
                    (Func<IQueryable<Product>, IQueryable<Product>>) (x =>
                        x.OrderByDescending(p => p.Category.Name)
                            .ThenBy(p => p.AvailableSince))
                };
            }
        }

        public static IEnumerable<object[]> AttributePermissiveTestCases
        {
            get
            {
                yield return new object[]
                {
                    "name",
                    (Func<IQueryable<ProductAlt2>, IQueryable<ProductAlt2>>) (x =>
                        x.OrderBy(p => p.Name)),
                    true
                };
                yield return new object[]
                {
                    "enabled",
                    (Func<IQueryable<ProductAlt2>, IQueryable<ProductAlt2>>) (x =>
                        x.OrderBy(p => p.Enabled)),
                    true
                };
                yield return new object[]
                {
                    "price",
                    (Func<IQueryable<ProductAlt2>, IQueryable<ProductAlt2>>) (x =>
                        x.OrderBy(p => p.Price)),
                    false
                };
                yield return new object[]
                {
                    "info",
                    (Func<IQueryable<ProductAlt2>, IQueryable<ProductAlt2>>) (x =>
                        x.OrderBy(p => p.Info)),
                    false
                };
            }
        }

        public static IEnumerable<object[]> AttributeStrictTestCases
        {
            get
            {
                yield return new object[]
                {
                    "name",
                    (Func<IQueryable<ProductAlt1>, IQueryable<ProductAlt1>>) (x =>
                        x.OrderBy(p => p.Name)),
                    false
                };
                yield return new object[]
                {
                    "enabled",
                    (Func<IQueryable<ProductAlt1>, IQueryable<ProductAlt1>>) (x =>
                        x.OrderBy(p => p.Enabled)),
                    false
                };
                yield return new object[]
                {
                    "price",
                    (Func<IQueryable<ProductAlt1>, IQueryable<ProductAlt1>>) (x =>
                        x.OrderBy(p => p.Price)),
                    true
                };
                yield return new object[]
                {
                    "info",
                    (Func<IQueryable<ProductAlt1>, IQueryable<ProductAlt1>>) (x =>
                        x.OrderBy(p => p.Info)),
                    true
                };
            }
        }

        public static IEnumerable<object[]> FluentPermissiveTestCases
        {
            get
            {
                yield return new object[]
                {
                    "name",
                    (Func<IQueryable<Product>, IQueryable<Product>>) (x =>
                        x.OrderBy(p => p.Name)),
                    true
                };
                yield return new object[]
                {
                    "enabled",
                    (Func<IQueryable<Product>, IQueryable<Product>>) (x =>
                        x.OrderBy(p => p.Enabled)),
                    true
                };
                yield return new object[]
                {
                    "price",
                    (Func<IQueryable<Product>, IQueryable<Product>>) (x =>
                        x.OrderBy(p => p.Price)),
                    false
                };
                yield return new object[]
                {
                    "info",
                    (Func<IQueryable<Product>, IQueryable<Product>>) (x =>
                        x.OrderBy(p => p.Info)),
                    false
                };
            }
        }

        public static IEnumerable<object[]> FluentStrictTestCases
        {
            get
            {
                yield return new object[]
                {
                    "name",
                    (Func<IQueryable<Product>, IQueryable<Product>>) (x =>
                        x.OrderBy(p => p.Name)),
                    false
                };
                yield return new object[]
                {
                    "enabled",
                    (Func<IQueryable<Product>, IQueryable<Product>>) (x =>
                        x.OrderBy(p => p.Enabled)),
                    false
                };
                yield return new object[]
                {
                    "price",
                    (Func<IQueryable<Product>, IQueryable<Product>>) (x =>
                        x.OrderBy(p => p.Price)),
                    true
                };
                yield return new object[]
                {
                    "info",
                    (Func<IQueryable<Product>, IQueryable<Product>>) (x =>
                        x.OrderBy(p => p.Info)),
                    true
                };
            }
        }

        public static IEnumerable<object[]> ExceptionsTestCases
        {
            get
            {
                yield return new object[]
                {
                    "test",
                    typeof(MissingPropertyException)
                };
            }
        }
    }
}