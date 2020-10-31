using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Sensei.AspNet.Queries.Exceptions;
using Sensei.AspNet.Tests.FakeServer.Entities;

namespace Sensei.AspNet.Tests.Queries
{
    public class QueryIncludesTestCase
    {
        public static IEnumerable<object[]> TestCases
        {
            get
            {
                yield return new object[]
                {
                    "category",
                    (Func<IQueryable<Product>, IQueryable<Product>>) (x =>
                        x.Include(p => p.Category))
                };
                yield return new object[]
                {
                    "category.categoryTimeSlots",
                    (Func<IQueryable<Product>, IQueryable<Product>>) (x =>
                        x.Include(p => p.Category.CategoryTimeSlots))
                };
                yield return new object[]
                {
                    "category.categoryTimeSlots.category,category.categoryTimeSlots.timeSlot",
                    (Func<IQueryable<Product>, IQueryable<Product>>) (x =>
                        x.Include(p => p.Category.CategoryTimeSlots)
                            .ThenInclude(c => c.Category)
                            .Include(p => p.Category.CategoryTimeSlots)
                            .ThenInclude(c => c.TimeSlot))
                };
            }
        }

        public static IEnumerable<object[]> AttributePermissiveTestCases
        {
            get
            {
                yield return new object[]
                {
                    "category",
                    (Func<IQueryable<ProductAlt2>, IQueryable<ProductAlt2>>) (x =>
                        x.Include(p => p.Category)),
                    false
                };
                yield return new object[]
                {
                    "categoryAlt",
                    (Func<IQueryable<ProductAlt2>, IQueryable<ProductAlt2>>) (x =>
                        x.Include(p => p.CategoryAlt)),
                    true
                };
            }
        }

        public static IEnumerable<object[]> AttributeStrictTestCases
        {
            get
            {
                yield return new object[]
                {
                    "category",
                    (Func<IQueryable<ProductAlt1>, IQueryable<ProductAlt1>>) (x =>
                        x.Include(p => p.Category)),
                    false
                };
                yield return new object[]
                {
                    "categoryAlt",
                    (Func<IQueryable<ProductAlt1>, IQueryable<ProductAlt1>>) (x =>
                        x.Include(p => p.CategoryAlt)),
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
                    "category",
                    (Func<IQueryable<Product>, IQueryable<Product>>) (x =>
                        x.Include(p => p.Category)),
                    false
                };
                yield return new object[]
                {
                    "categoryAlt",
                    (Func<IQueryable<Product>, IQueryable<Product>>) (x =>
                        x.Include(p => p.CategoryAlt)),
                    true
                };
            }
        }

        public static IEnumerable<object[]> FluentStrictTestCases
        {
            get
            {
                yield return new object[]
                {
                    "category",
                    (Func<IQueryable<Product>, IQueryable<Product>>) (x =>
                        x.Include(p => p.Category)),
                    false
                };
                yield return new object[]
                {
                    "categoryAlt",
                    (Func<IQueryable<Product>, IQueryable<Product>>) (x =>
                        x.Include(p => p.CategoryAlt)),
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