using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Sensei.AspNet.QueryProcessor.QueryFilters;
using Sensei.AspNet.QueryProcessor.QueryFilters.Filters;

namespace Sensei.AspNet.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection UseSensei(this IServiceCollection services,
            Action<SenseiOptions> optionsAction = null)
        {
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddTransient<QueryProcessor.QueryProcessor>();
            services.AddTransient<IQueryFilter, StringQueryFilter>();
            services.AddTransient<IQueryFilter, NumericQueryFilter>();
            services.AddTransient<IQueryFilter, EnumQueryFilter>();
            
            var options = new SenseiOptions();
            optionsAction?.Invoke(options);

            services.AddSingleton(options);

            return services;
        }
    }
}