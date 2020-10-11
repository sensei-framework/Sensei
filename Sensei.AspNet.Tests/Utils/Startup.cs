using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Sensei.AspNet.Extensions;

namespace Sensei.AspNet.Tests.Utils
{
    public class Startup
    {
        private SqliteConnection _inMemorySqlite;
        
        public void ConfigureServices(IServiceCollection services)
        {
            //_inMemorySqlite = new SqliteConnection("Data Source=C:\\Users\\skaman\\Desktop\\test.db");
            _inMemorySqlite = new SqliteConnection("Data Source=:memory:");
            _inMemorySqlite.Open();

            services.AddDbContext<TestDbContext>(options =>
                options.UseSqlite(_inMemorySqlite));
            
            services.AddControllers();

            services.AddAuthentication();
            
            services.UseSensei();

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
            using var context = serviceScope.ServiceProvider.GetService<TestDbContext>();
            context.Database.Migrate();
        }
        
        private class FakeUserFilter : IAsyncActionFilter
        {
            public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
            {
                context.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, "12345678-1234-1234-1234-123456789012"),
                    new Claim(ClaimTypes.Name, "TestUser"),
                    new Claim(ClaimTypes.Email, "test.user@example.com"), // add as many claims as you need
                }));

                await next();
            }
        }
    }
}