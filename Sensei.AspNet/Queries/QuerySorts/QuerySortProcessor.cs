﻿using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sensei.AspNet.Queries.Entities;
using Sensei.AspNet.Queries.Exceptions;

namespace Sensei.AspNet.Queries.QuerySorts
{
    public static class QuerySortProcessor
    {
        public static Query<TEntity> ApplySorting<TEntity>(this Query<TEntity> query, Sorting sorting)
        {
            if (string.IsNullOrEmpty(sorting.Sorts))
                return query;
            
            var queryable = query.Queryable;
            var logger = query.LoggerFactory.CreateLogger(Const.LoggerName);
            var queryContext = query.QueryContext;
            var serviceProvider = query.ServiceProvider;
            var options = serviceProvider.GetService<SenseiOptions>();

            var parameterExpression = Expression.Parameter(typeof(TEntity), "e");

            var terms = ParseQuery(sorting.Sorts);
            var currentExpression = queryable.Expression;
            var useThenBy = false;
            
            foreach (var term in terms)
            {
                // resolve the property expression
                var (propertyInfo, propertyExpression) =
                    queryContext.Resolve(typeof(TEntity), parameterExpression, term.Field, QueryType.Sorts,
                        serviceProvider.GetService<SenseiOptions>());

                // we skip if we can't find the property
                if (propertyInfo == null || propertyExpression == null)
                {
                    logger.LogError("Property for field {field} not found", term.Field);

                    if (options.ThrowExceptionOnQueryError)
                        throw new MissingPropertyException($"Property for field {term.Field} not found");

                    continue;
                }
                
                var command = term.Type == SortTypeEnum.Descending ?
                    useThenBy ? "ThenByDescending" : "OrderByDescending" :
                    useThenBy ? "ThenBy" : "OrderBy";
                
                // update the expression
                var memberExpression = Expression.MakeMemberAccess(parameterExpression, propertyInfo);
                var orderByExpression = Expression.Lambda(memberExpression, parameterExpression);
                currentExpression =
                    Expression.Call(
                        typeof(Queryable),
                        command,
                        new[] { typeof(TEntity), propertyInfo.PropertyType },
                        currentExpression, Expression.Quote(orderByExpression));

                useThenBy = true;
            }

            // if the expression is changed we create the new query
            if (currentExpression != queryable.Expression)
                query.Queryable = queryable.Provider.CreateQuery<TEntity>(currentExpression);

            return query;
        }

        private static List<SortTerm> ParseQuery(string query)
        {
            var terms = new List<SortTerm>();
            var parts = query.Split(',');
            foreach (var part in parts)
            {
                var value = part.Trim();
                if (value.StartsWith('-'))
                    terms.Add(new SortTerm
                    {
                        Field = value.TrimStart('-'),
                        Type = SortTypeEnum.Descending
                    });
                else
                    terms.Add(new SortTerm
                    {
                        Field = value.TrimStart('+'),
                        Type = SortTypeEnum.Ascending
                    });
            }

            return terms;
        }
    }
}