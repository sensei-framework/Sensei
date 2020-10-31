using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sensei.AspNet.Extensions;
using Sensei.AspNet.Queries;
using Sensei.AspNet.Tests.FakeServer.Entities;
using Sensei.AspNet.Utils.Database;

namespace Sensei.AspNet.Tests.FakeServer
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private SqliteConnection _inMemorySqlite;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            //_inMemorySqlite = new SqliteConnection("Data Source=/Users/skaman/Desktop/test.db");
            _inMemorySqlite = new SqliteConnection("Data Source=:memory:");
            _inMemorySqlite.Open();

            services.AddDbContext<FakeServerDbContext>(options =>
                options.UseSqlite(_inMemorySqlite));

            services.AddControllers();

            services.AddAuthentication();

            services.UseSensei(options =>
            {
                options.EnableFiltersAsDefault =
                    _configuration.GetValue<bool?>("EnableFiltersAsDefault") ?? options.EnableFiltersAsDefault;
                options.EnableSortsAsDefault =
                    _configuration.GetValue<bool?>("EnableSortsAsDefault") ?? options.EnableSortsAsDefault;
                options.EnableIncludesAsDefault =
                    _configuration.GetValue<bool?>("EnableIncludesAsDefault") ?? options.EnableIncludesAsDefault;
            });

            if (_configuration.GetValue<bool>("FluentPermissive"))
                services.AddTransient<IQueryContext, FluentPermissiveQueryContext>();

            if (_configuration.GetValue<bool>("FluentStrict"))
                services.AddTransient<IQueryContext, FluentStrictQueryContext>();

            services.AddMvc(
                options =>
                {
                    options.Filters.Add(new AllowAnonymousFilter());
                    options.Filters.Add(new FakeUserFilter());
                });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            UpdateDatabase(app);

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }

        private static void UpdateDatabase(IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope();
            using var context = serviceScope.ServiceProvider.GetService<FakeServerDbContext>();
            context.Database.Migrate();

            context.Seed<Category>("FakeServer/Data/categories.json");
            context.Seed<Product>("FakeServer/Data/products.json");
            context.Seed<ProductAlt1>("FakeServer/Data/products.json");
            context.Seed<ProductAlt2>("FakeServer/Data/products.json");
            context.Seed<TimeSlot>("FakeServer/Data/timeSlots.json");
            context.Seed<CategoryTimeSlot>("FakeServer/Data/categoryTimeSlots.json");
        }

        private class FakeUserFilter : IAsyncActionFilter
        {
            public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
            {
                context.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, "12345678-1234-1234-1234-123456789012"),
                    new Claim(ClaimTypes.Name, "TestUser"),
                    new Claim(ClaimTypes.Email, "test.user@example.com") // add as many claims as you need
                }));

                await next();
            }
        }
    }
}