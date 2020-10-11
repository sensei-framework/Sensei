using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Logging;
using Sensei.AspNet.Extensions;

namespace CatalogApplication
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // ReSharper disable once MemberCanBePrivate.Global
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
#if  DEBUG
            IdentityModelEventSource.ShowPII = true;
#endif
            
            services.AddDbContext<CatalogDbContext>(options =>
                options.UseSqlite(
                    Configuration.GetConnectionString("DefaultConnection")));
            
            services.AddControllers();

            services
                .AddMvcCore(options =>
                {
                    // require scope1 or scope2
                    var policy = ScopePolicy.Create("catalog.read", "catalog.create", "catalog.update", "catalog.delete");
                    options.Filters.Add(new AuthorizeFilter(policy));
                });

            //services.AddAuthorization();
            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = "https://localhost:5050";
                    options.ApiName = "catalog";
                });/*.AddPolicyScheme(IdentityServerAuthenticationDefaults.AuthenticationScheme, "", options =>
                {
                } );*/
            
            services.AddAuthorization(options =>
            {
                options.AddPolicy("read", builder =>
                {
                    builder.RequireScope("catalog.read");
                });
                options.AddPolicy("create", builder =>
                {
                    builder.RequireScope("catalog.create");
                });
                options.AddPolicy("update", builder =>
                {
                    builder.RequireScope("catalog.update");
                });
                options.AddPolicy("delete", builder =>
                {
                    builder.RequireScope("catalog.delete");
                });
            });

            services.UseSensei();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}