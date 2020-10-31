using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Foundatio.Parsers.LuceneQueries;
using Foundatio.Parsers.LuceneQueries.Extensions;
using Foundatio.Parsers.LuceneQueries.Nodes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sensei.AspNet.Queries.Entities;
using Sensei.AspNet.Queries.Exceptions;

namespace Sensei.AspNet.Queries.QueryFilters
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
        /// <exception cref="LuceneParserException">Raise if the lucene query have syntax errors.</exception>
        /// <exception cref="MissingPropertyException">Raise if the property is missing or doesnt' have permissions.</exception>
        /// <exception cref="MissingFilterException">Raise if there is no filter for the property type.</exception>
        /// <exception cref="EvaluateFilterException">Raise if there is an error when evaluating a filter.</exception>
        /// <returns>The updated query.</returns>
        public static Query<TEntity> ApplyFilters<TEntity>(this Query<TEntity> query, Filtering filtering)
        {
            if (string.IsNullOrEmpty(filtering?.Filters))
                return query;

            var queryable = query.Queryable;
            var serviceProvider = query.ServiceProvider;
            var queryContext = query.QueryContext;
            var options = serviceProvider.GetService<SenseiOptions>();
            
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
                
                if (options.ThrowExceptionOnQueryError)
                    throw new LuceneParserException("Cannot parse lucene query", e);
                
                return query;
            }

            var parameterExpression = Expression.Parameter(typeof(TEntity), "e");

            var groupNodeExpression =
                BuildNodeExpression<TEntity>(groupNode, parameterExpression, serviceProvider, logger, queryContext,
                    options);
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
            Expression parameterExpression, IServiceProvider serviceProvider, ILogger logger,
            QueryContext queryContext, SenseiOptions options)
        {
            var expressions = new List<Expression>();
            
            foreach (var queryNode in groupNode.Children)
            {
                // if it's a child node we resolve it recursively
                if (queryNode is GroupNode childGroupNode)
                {
                    var expression =
                        BuildNodeExpression<TEntity>(childGroupNode, parameterExpression, serviceProvider, logger,
                            queryContext, options);
                    if (expression == null)
                        continue;
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

                        if (options.ThrowExceptionOnQueryError)
                            throw new MissingPropertyException("Missing field name");
                        
                        continue;
                    }

                    // resolve the property expression
                    var queryPropertyInfo =
                        queryContext.Resolve(typeof(TEntity), parameterExpression, field,
                            QueryType.Filters, options);

                    // we skip if we can't find the property
                    if (queryPropertyInfo?.PropertyInfo == null || queryPropertyInfo.Expression == null)
                    {
                        logger.LogError("Property for field {field} not found", field);

                        if (options.ThrowExceptionOnQueryError)
                            throw new MissingPropertyException($"Property for field {field} not found");
                        
                        continue;
                    }

                    var propertyType = queryPropertyInfo.PropertyInfo.PropertyType;
                    var underlyingType = Nullable.GetUnderlyingType(propertyType);
                    if (underlyingType != null)
                        propertyType = underlyingType;

                    // retrieve the right query filter
                    var queryFilters = serviceProvider.GetServices<IQueryFilter>();
                    var queryFilter =
                        queryFilters.FirstOrDefault(f =>
                            f.SupportedTypes.Any(t => t.IsAssignableFrom(propertyType)));

                    // if we can't find the filter, skip it
                    if (queryFilter == null)
                    {
                        logger.LogError("Query filter for type {type} not found",
                            queryPropertyInfo.PropertyInfo.PropertyType.Name);

                        if (options.ThrowExceptionOnQueryError)
                            throw new MissingFilterException(
                                $"Query filter for type {queryPropertyInfo.PropertyInfo.PropertyType.Name} not found");
                        
                        continue;
                    }

                    // resolve the expression
                    Expression expression = null;
                    try
                    {
                        if (queryNode is TermNode termNode)
                        {
                            expression = queryFilter.GetCompareExpression(queryPropertyInfo.Expression,
                                queryPropertyInfo.PropertyInfo.PropertyType,
                                termNode.Term.Unescape());

                            if (expression != null && termNode.IsNegated.HasValue && termNode.IsNegated.Value)
                                expression = Expression.Not(expression);
                        }
                        else if (queryNode is ExistsNode existsNode)
                        {
                            expression = queryFilter.GetExistsExpression(queryPropertyInfo.Expression,
                                queryPropertyInfo.PropertyInfo.PropertyType);

                            if (expression != null && existsNode.IsNegated.HasValue && existsNode.IsNegated.Value)
                                expression = Expression.Not(expression);
                        }
                        else if (queryNode is TermRangeNode rangeNode)
                        {
                            Expression first = null;
                            Expression second = null;
                            if (!string.IsNullOrEmpty(rangeNode.Min))
                                first = queryFilter.GetGreaterExpression(queryPropertyInfo.Expression,
                                    queryPropertyInfo.PropertyInfo.PropertyType,
                                    rangeNode.Min.Unescape(), rangeNode.MinInclusive == true);
                            if (!string.IsNullOrEmpty(rangeNode.Max))
                                second = queryFilter.GetLessExpression(queryPropertyInfo.Expression,
                                    queryPropertyInfo.PropertyInfo.PropertyType,
                                    rangeNode.Max.Unescape(), rangeNode.MaxInclusive == true);

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
                        
                        if (options.ThrowExceptionOnQueryError)
                            throw new EvaluateFilterException("Cannot build expression");
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
                    lastExpression = Expression.AndAlso(lastExpression, expressions[i]);
                finalExpression = lastExpression;
            }
            else
            {
                var lastExpression = expressions.First();
                for (var i = 1; i < expressions.Count; i++)
                    lastExpression = Expression.OrElse(lastExpression, expressions[i]);
                finalExpression = lastExpression;
            }

            return finalExpression;
        }
    }
}