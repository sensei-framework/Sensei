using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Sensei.AspNet.Queries;
using Sensei.AspNet.Queries.QueryFilters;
using Sensei.AspNet.Queries.QueryFilters.Filters;

namespace Sensei.AspNet.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection UseSensei(this IServiceCollection services,
            Action<SenseiOptions> optionsAction = null)
        {
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddTransient<IQueryProcessor, QueryProcessor>();
            services.AddTransient<IQueryFilter, StringQueryFilter>();
            services.AddTransient<IQueryFilter, ComplexQueryFilter>();
            services.AddTransient<IQueryFilter, GenericQueryFilter>();
            
            var options = new SenseiOptions();
            optionsAction?.Invoke(options);

            services.AddSingleton(options);

            return services;
        }
    }
}