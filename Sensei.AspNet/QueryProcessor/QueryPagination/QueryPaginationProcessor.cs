﻿using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Sensei.AspNet.Models;

namespace Sensei.AspNet.QueryProcessor.QueryPagination
{
    public static class QueryPaginationProcessor
    {
        public static Paginator<TEntity> ApplyPagination<TEntity>(this Query<TEntity> query, Pagination pagination)
            where TEntity : class
        {
            var queryable = query.Queryable;
            var options = query.ServiceProvider.GetService<SenseiOptions>();

            var page = pagination.Page ?? 1;
            var pageSize = pagination.PageSize ?? options.PaginationDefaultPageSize;
            var maxPageSize = options.PaginationMaxPageSize > 0 ? options.PaginationMaxPageSize : pageSize;
            var count = queryable.Count();

            if (pageSize > 0)
            {
                queryable = queryable.Skip((page - 1) * pageSize);
                queryable = queryable.Take(Math.Min(pageSize, maxPageSize));
            }

            return new Paginator<TEntity>
            {
                Page = page,
                PageSize = pageSize,
                Items = queryable,
                Total = count,
                HavePrev = page > 1,
                HaveNext = count > page * pageSize
            };
        }
    }
}