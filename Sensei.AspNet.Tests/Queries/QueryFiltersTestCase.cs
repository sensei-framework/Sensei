using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Sensei.AspNet.Queries.Exceptions;
using Sensei.AspNet.Tests.FakeServer.Entities;

namespace Sensei.AspNet.Tests.Queries
{
    public class QueryFiltersTestCase
    {
        public static IEnumerable<object[]> TestCases
        {
            get
            {
                yield return new object[]
                {
                    "name:\"tomato salad\"",
                    (Func<IQueryable<Product>, IQueryable<Product>>) (x =>
                        x.Where(p => EF.Functions.Like(p.Name, "tomato salad")))
                };
                yield return new object[]
                {
                    "name:tomato%",
                    (Func<IQueryable<Product>, IQueryable<Product>>) (x =>
                        x.Where(p => EF.Functions.Like(p.Name, "tomato%")))
                };
                yield return new object[]
                {
                    "NOT name:tomato%",
                    (Func<IQueryable<Product>, IQueryable<Product>>) (x =>
                        x.Where(p => !EF.Functions.Like(p.Name, "tomato%")))
                };
                yield return new object[]
                {
                    "name:%potatoes",
                    (Func<IQueryable<Product>, IQueryable<Product>>) (x =>
                        x.Where(p => EF.Functions.Like(p.Name, "%potatoes")))
                };
                yield return new object[]
                {
                    "name:%tomato%",
                    (Func<IQueryable<Product>, IQueryable<Product>>) (x =>
                        x.Where(p => EF.Functions.Like(p.Name, "%tomato%")))
                };
                yield return new object[]
                {
                    "_exists_:info",
                    (Func<IQueryable<Product>, IQueryable<Product>>) (x =>
                        x.Where(p => p.Info != null))
                };
                yield return new object[]
                {
                    "NOT _exists_:info",
                    (Func<IQueryable<Product>, IQueryable<Product>>) (x =>
                        x.Where(p => p.Info == null))
                };
                yield return new object[]
                {
                    "id:dcd8b3d5-fad8-49cc-ae80-481847aca50e",
                    (Func<IQueryable<Product>, IQueryable<Product>>) (x =>
                        x.Where(p => p.Id == Guid.Parse("dcd8b3d5-fad8-49cc-ae80-481847aca50e")))
                };
                yield return new object[]
                {
                    "fileId:b5ede0ef-d3f8-4006-84c9-304d46a5267d",
                    (Func<IQueryable<Product>, IQueryable<Product>>) (x =>
                        x.Where(p => p.FileId == Guid.Parse("b5ede0ef-d3f8-4006-84c9-304d46a5267d")))
                };
                yield return new object[]
                {
                    "_exists_:fileId",
                    (Func<IQueryable<Product>, IQueryable<Product>>) (x =>
                        x.Where(p => p.FileId != null))
                };
                yield return new object[]
                {
                    "status:promotion",
                    (Func<IQueryable<Product>, IQueryable<Product>>) (x =>
                        x.Where(p => p.Status == Product.StatusEnum.Promotion))
                };
                yield return new object[]
                {
                    "status:1",
                    (Func<IQueryable<Product>, IQueryable<Product>>) (x =>
                        x.Where(p => p.Status == Product.StatusEnum.Promotion))
                };
                yield return new object[]
                {
                    "NOT status:promotion",
                    (Func<IQueryable<Product>, IQueryable<Product>>) (x =>
                        x.Where(p => p.Status != Product.StatusEnum.Promotion))
                };
                yield return new object[]
                {
                    "availability:outOfStock",
                    (Func<IQueryable<Product>, IQueryable<Product>>) (x =>
                        x.Where(p => p.Availability == Product.AvailabilityEnum.OutOfStock))
                };
                yield return new object[]
                {
                    "_exists_:availability",
                    (Func<IQueryable<Product>, IQueryable<Product>>) (x =>
                        x.Where(p => p.Availability != null))
                };
                yield return new object[]
                {
                    "NOT _exists_:availability",
                    (Func<IQueryable<Product>, IQueryable<Product>>) (x =>
                        x.Where(p => p.Availability == null))
                };
                yield return new object[]
                {
                    "price:2.5",
                    (Func<IQueryable<Product>, IQueryable<Product>>) (x =>
                        // ReSharper disable once CompareOfFloatsByEqualityOperator
                        x.Where(p => p.Price == 2.5))
                };
                yield return new object[]
                {
                    "price:<2.5",
                    (Func<IQueryable<Product>, IQueryable<Product>>) (x =>
                        x.Where(p => p.Price < 2.5))
                };
                yield return new object[]
                {
                    "price:<=2.5",
                    (Func<IQueryable<Product>, IQueryable<Product>>) (x =>
                        x.Where(p => p.Price <= 2.5))
                };
                yield return new object[]
                {
                    "price:>2.5",
                    (Func<IQueryable<Product>, IQueryable<Product>>) (x =>
                        x.Where(p => p.Price > 2.5))
                };
                yield return new object[]
                {
                    "price:>=2.5",
                    (Func<IQueryable<Product>, IQueryable<Product>>) (x =>
                        x.Where(p => p.Price >= 2.5))
                };
                yield return new object[]
                {
                    "price:[2.5 TO 3.5]",
                    (Func<IQueryable<Product>, IQueryable<Product>>) (x =>
                        x.Where(p => p.Price >= 2.5 && p.Price <= 3.5))
                };
                yield return new object[]
                {
                    "discountAfter:19\\:00",
                    (Func<IQueryable<Product>, IQueryable<Product>>) (x =>
                        x.Where(p => p.DiscountAfter == TimeSpan.Parse("19:00")))
                };
                // Disabled because SQLITE doesn't support it
#if false
                yield return new object []
                {
                    "discountAfter:>19\\:00",
                    (Func<IQueryable<Product>, IQueryable<Product>>)(x =>
                        x.Where(p => p.DiscountAfter > TimeSpan.Parse("19:00")))
                };
                yield return new object []
                {
                    "discountAfter:>=19\\:00",
                    (Func<IQueryable<Product>, IQueryable<Product>>)(x =>
                        x.Where(p => p.DiscountAfter >= TimeSpan.Parse("19:00")))
                };
                yield return new object []
                {
                    "discountAfter:<20\\:00",
                    (Func<IQueryable<Product>, IQueryable<Product>>)(x =>
                        x.Where(p => p.DiscountAfter < TimeSpan.Parse("20:00")))
                };
                yield return new object []
                {
                    "discountAfter:<=20\\:00",
                    (Func<IQueryable<Product>, IQueryable<Product>>)(x =>
                        x.Where(p => p.DiscountAfter <= TimeSpan.Parse("20:00")))
                };
#endif
                yield return new object[]
                {
                    "availableSince:2020-12-02T08\\:00\\:00Z",
                    (Func<IQueryable<Product>, IQueryable<Product>>) (x =>
                        x.Where(p => p.AvailableSince == DateTimeOffset.Parse("2020-12-02T08:00:00Z")))
                };
                yield return new object[]
                {
                    "availableSince:>2020-12-02T08\\:00\\:00Z",
                    (Func<IQueryable<Product>, IQueryable<Product>>) (x =>
                        x.Where(p => p.AvailableSince > DateTimeOffset.Parse("2020-12-02T08:00:00Z")))
                };
                yield return new object[]
                {
                    "availableSince:>=2020-12-02T08\\:00\\:00Z",
                    (Func<IQueryable<Product>, IQueryable<Product>>) (x =>
                        x.Where(p => p.AvailableSince >= DateTimeOffset.Parse("2020-12-02T08:00:00Z")))
                };
                yield return new object[]
                {
                    "availableSince:<2020-12-02T08\\:00\\:00Z",
                    (Func<IQueryable<Product>, IQueryable<Product>>) (x =>
                        x.Where(p => p.AvailableSince < DateTimeOffset.Parse("2020-12-02T08:00:00Z")))
                };
                yield return new object[]
                {
                    "availableSince:<=2020-12-02T08\\:00\\:00Z",
                    (Func<IQueryable<Product>, IQueryable<Product>>) (x =>
                        x.Where(p => p.AvailableSince <= DateTimeOffset.Parse("2020-12-02T08:00:00Z")))
                };
                yield return new object[]
                {
                    "availableSince:[2020-12-02T08\\:00\\:00Z TO 2020-12-05T08\\:00\\:00Z]",
                    (Func<IQueryable<Product>, IQueryable<Product>>) (x =>
                        x.Where(p => p.AvailableSince >= DateTimeOffset.Parse("2020-12-02T08:00:00Z") &&
                                     p.AvailableSince <= DateTimeOffset.Parse("2020-12-05T08:00:00Z")))
                };
                yield return new object[]
                {
                    "NOT _exists_:info AND (price:<=2.5 OR price:>=3.5)",
                    (Func<IQueryable<Product>, IQueryable<Product>>) (x =>
                        x.Where(p => p.Info == null && (p.Price <= 2.5 || p.Price >= 3.5)))
                };
                yield return new object[]
                {
                    "category.enabled:true",
                    (Func<IQueryable<Product>, IQueryable<Product>>) (x =>
                        x.Where(p => p.Category.Enabled))
                };
            }
        }

        public static IEnumerable<object[]> AttributePermissiveTestCases
        {
            get
            {
                yield return new object[]
                {
                    "name:\"tomato salad\"",
                    (Func<IQueryable<ProductAlt2>, IQueryable<ProductAlt2>>) (x =>
                        x.Where(p => EF.Functions.Like(p.Name, "tomato salad"))),
                    true
                };
                yield return new object[]
                {
                    "enabled:true",
                    (Func<IQueryable<ProductAlt2>, IQueryable<ProductAlt2>>) (x =>
                        x.Where(p => p.Enabled)),
                    true
                };
                yield return new object[]
                {
                    "price:<3",
                    (Func<IQueryable<ProductAlt2>, IQueryable<ProductAlt2>>) (x =>
                        x.Where(p => p.Price < 3)),
                    false
                };
                yield return new object[]
                {
                    "_exists_:fileId",
                    (Func<IQueryable<ProductAlt2>, IQueryable<ProductAlt2>>) (x =>
                        x.Where(p => p.FileId != null)),
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
                    "name:\"tomato salad\"",
                    (Func<IQueryable<ProductAlt1>, IQueryable<ProductAlt1>>) (x =>
                        x.Where(p => EF.Functions.Like(p.Name, "tomato salad"))),
                    false
                };
                yield return new object[]
                {
                    "enabled:true",
                    (Func<IQueryable<ProductAlt1>, IQueryable<ProductAlt1>>) (x =>
                        x.Where(p => p.Enabled)),
                    false
                };
                yield return new object[]
                {
                    "price:<3",
                    (Func<IQueryable<ProductAlt1>, IQueryable<ProductAlt1>>) (x =>
                        x.Where(p => p.Price < 3)),
                    true
                };
                yield return new object[]
                {
                    "_exists_:fileId",
                    (Func<IQueryable<ProductAlt1>, IQueryable<ProductAlt1>>) (x =>
                        x.Where(p => p.FileId != null)),
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
                    "name:\"tomato salad\"",
                    (Func<IQueryable<Product>, IQueryable<Product>>) (x =>
                        x.Where(p => EF.Functions.Like(p.Name, "tomato salad"))),
                    true
                };
                yield return new object[]
                {
                    "enabled:true",
                    (Func<IQueryable<Product>, IQueryable<Product>>) (x =>
                        x.Where(p => p.Enabled)),
                    true
                };
                yield return new object[]
                {
                    "price:<3",
                    (Func<IQueryable<Product>, IQueryable<Product>>) (x =>
                        x.Where(p => p.Price < 3)),
                    false
                };
                yield return new object[]
                {
                    "_exists_:fileId",
                    (Func<IQueryable<Product>, IQueryable<Product>>) (x =>
                        x.Where(p => p.FileId != null)),
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
                    "name:\"tomato salad\"",
                    (Func<IQueryable<Product>, IQueryable<Product>>) (x =>
                        x.Where(p => EF.Functions.Like(p.Name, "tomato salad"))),
                    false
                };
                yield return new object[]
                {
                    "enabled:true",
                    (Func<IQueryable<Product>, IQueryable<Product>>) (x =>
                        x.Where(p => p.Enabled)),
                    false
                };
                yield return new object[]
                {
                    "price:<3",
                    (Func<IQueryable<Product>, IQueryable<Product>>) (x =>
                        x.Where(p => p.Price < 3)),
                    true
                };
                yield return new object[]
                {
                    "_exists_:fileId",
                    (Func<IQueryable<Product>, IQueryable<Product>>) (x =>
                        x.Where(p => p.FileId != null)),
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
                    "a:[ :",
                    typeof(LuceneParserException)
                };
                yield return new object[]
                {
                    "notexist:test",
                    typeof(MissingPropertyException)
                };
                yield return new object[]
                {
                    "category:test",
                    typeof(MissingFilterException)
                };
                yield return new object[]
                {
                    "availableSince:invaliddate",
                    typeof(EvaluateFilterException)
                };
            }
        }
    }
}