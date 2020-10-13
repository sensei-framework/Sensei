using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Foundatio.Parsers.LuceneQueries;
using Foundatio.Parsers.LuceneQueries.Nodes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sensei.AspNet.Models;
using Sensei.AspNet.QueryProcessor.Entities;

namespace Sensei.AspNet.QueryProcessor.QueryFilters
{
    /// <summary>
    ///     Process the filters for a query.
    /// </summary>
    public static class QueryFilterProcessor
    {
        /// <summary>
        ///     Apply filters to a query.
        /// </summary>
        /// <param name="query">The query where the filters need to applied.</param>
        /// <param name="filtering">The filters model.</param>
        /// <typeparam name="TEntity">The entity type associated to the query.</typeparam>
        /// <returns>The updated query.</returns>
        public static Query<TEntity> ApplyFilters<TEntity>(this Query<TEntity> query, Filtering filtering)
        {
            if (string.IsNullOrEmpty(filtering?.Filters))
                return query;

            var queryable = query.Queryable;
            var serviceProvider = query.ServiceProvider;
            
            var logger = query.LoggerFactory.CreateLogger(Const.LoggerName);
            logger.LogDebug("Lucene query: {filters}", filtering.Filters);
            
            var parser = new LuceneQueryParser();
            GroupNode groupNode;
            try
            {
                groupNode = parser.Parse(filtering.Filters);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Cannot parse lucene query");
                return query;
            }

            var parameterExpression = Expression.Parameter(typeof(TEntity), "e");

            var groupNodeExpression =
                BuildNodeExpression<TEntity>(groupNode, parameterExpression, serviceProvider, logger);
            if (groupNodeExpression != null)
            {
                var whereCallExpression = Expression.Call(  
                    typeof(Queryable),  
                    "Where",  
                    new[] { queryable.ElementType },  
                    queryable.Expression,  
                    Expression.Lambda<Func<TEntity, bool>>(groupNodeExpression, parameterExpression));
                
                queryable = queryable.Provider.CreateQuery<TEntity>(whereCallExpression);
            }

            query.Queryable = queryable;
            
            return query;
        }

        private static Expression BuildNodeExpression<TEntity>(GroupNode groupNode,
            Expression parameterExpression, IServiceProvider serviceProvider, ILogger logger)
        {
            var expressions = new List<Expression>();
            
            foreach (var queryNode in groupNode.Children)
            {
                // if it's a child node we resolve it recursively
                if (queryNode is GroupNode childGroupNode)
                {
                    var expression =
                        BuildNodeExpression<TEntity>(childGroupNode, parameterExpression, serviceProvider, logger);
                    if (expression == null)
                    {
                        logger.LogError("Error creating group node expression");
                        continue;
                    }
                    expressions.Add(expression);
                }
                
                // if it's a term node we try to resolve it
                else if (queryNode is TermNode || queryNode is ExistsNode || queryNode is TermRangeNode)
                {
                    
                    var field = (queryNode as TermNode)?.Field ??
                                (queryNode as ExistsNode)?.Field ??
                                (queryNode as TermRangeNode)?.Field;
                    
                    // we skip it if the field name is empty
                    if (string.IsNullOrEmpty(field))
                    {
                        logger.LogError("Missing field name");
                        continue;
                    }

                    // resolve the property expression
                    var (propertyInfo, propertyExpression) =
                        QueryProcessor.ResolveProperty(typeof(TEntity), parameterExpression, field);

                    // we skip if we can't find the property
                    if (propertyInfo == null || propertyExpression == null)
                    {
                        logger.LogError("Property for field {field} not found", field);
                        continue;
                    }

                    // retrieve the right query filter
                    var queryFilters = serviceProvider.GetServices<IQueryFilter>();
                    var queryFilter =
                        queryFilters.FirstOrDefault(f =>
                            f.SupportedTypes.Any(t =>
                                t == propertyInfo.PropertyType || t.IsAssignableFrom(propertyInfo.PropertyType)));

                    // if we can't find the filter, skip it
                    if (queryFilter == null)
                    {
                        logger.LogError("Query filter for type {type} not found", propertyInfo.PropertyType.Name);
                        continue;
                    }

                    // resolve the expression
                    Expression expression = null;
                    try
                    {
                        if (queryNode is TermNode termNode)
                        {
                            expression = queryFilter.GetCompareExpression(propertyExpression, propertyInfo.PropertyType,
                                termNode.Term);

                            if (expression != null && termNode.IsNegated.HasValue && termNode.IsNegated.Value)
                                expression = Expression.Not(expression);
                        }
                        else if (queryNode is ExistsNode existsNode)
                        {
                            expression = queryFilter.GetExistsExpression(propertyExpression, propertyInfo.PropertyType);

                            if (expression != null && existsNode.IsNegated.HasValue && existsNode.IsNegated.Value)
                                expression = Expression.Not(expression);
                        }
                        else if (queryNode is TermRangeNode rangeNode)
                        {
                            Expression first = null;
                            Expression second = null;
                            if (!string.IsNullOrEmpty(rangeNode.Min))
                                first = queryFilter.GetGreaterExpression(propertyExpression, propertyInfo.PropertyType,
                                    rangeNode.Min, rangeNode.MinInclusive == true);
                            if (!string.IsNullOrEmpty(rangeNode.Max))
                                second = queryFilter.GetLessExpression(propertyExpression, propertyInfo.PropertyType,
                                    rangeNode.Max, rangeNode.MaxInclusive == true);

                            if (first != null && second != null)
                                expression = Expression.And(first, second);
                            else
                                expression = first ?? second;

                            if (expression != null && rangeNode.IsNegated.HasValue && rangeNode.IsNegated.Value)
                                expression = Expression.Not(expression);
                        }
                    }
                    catch (Exception e)
                    {
                        logger.LogError(e, "Cannot build expression");
                    }

                    if (expression != null)
                        expressions.Add(expression);
                }
            }

            // no expressions :(
            if (expressions.Count == 0)
                return null;

            // there's not expression to chain
            if (expressions.Count == 1)
                return expressions.FirstOrDefault();

            // chain expression following the group operator
            Expression finalExpression;
            if (groupNode.Operator == GroupOperator.And ||
                groupNode.Operator == GroupOperator.Default)
            {
                var lastExpression = expressions.First();
                for (var i = 1; i < expressions.Count; i++)
                    lastExpression = Expression.And(lastExpression, expressions[i]);
                finalExpression = lastExpression;
            }
            else
            {
                var lastExpression = expressions.First();
                for (var i = 1; i < expressions.Count; i++)
                    lastExpression = Expression.Or(lastExpression, expressions[i]);
                finalExpression = lastExpression;
            }

            return finalExpression;
        }
    }
}