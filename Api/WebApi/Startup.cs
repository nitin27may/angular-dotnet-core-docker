using System;
using Application;
using Application.Interfaces;
using Infrastructure.Identity;
using Infrastructure.Identity.Contexts;
using Infrastructure.Identity.Models;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Shared;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WebApi.Extensions;
using WebApi.Services;

namespace WebApi;

public class Startup
{
    public IConfiguration _config { get; }
    public Startup(IConfiguration configuration)
    {
        _config = configuration;
    }
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddApplicationLayer();
        services.AddIdentityInfrastructure(_config);
        services.AddPersistenceInfrastructure(_config);
        services.AddSharedInfrastructure(_config);
        services.AddSwaggerExtension();
        services.AddControllers();
        services.AddApiVersioningExtension();
        services.AddHealthChecks();
        services.AddScoped<IAuthenticatedUserService, AuthenticatedUserService>();
    }

    public async void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseSwaggerExtension();
        app.UseErrorHandlingMiddleware();
        app.UseHealthChecks("/health");

        app.UseEndpoints(endpoints =>
         {
             endpoints.MapControllers();
         });
        using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
        {
            var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
            context.Database.Migrate();
        }

        using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
        {
            var context = serviceScope.ServiceProvider.GetService<IdentityContext>();
            context.Database.Migrate();
        }

        using (var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
        {
            var services = scope.ServiceProvider;

            var loggerFactory = services.GetRequiredService<ILoggerFactory>();
            try
            {
                var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

                 await Infrastructure.Identity.Seeds.DefaultRoles.SeedAsync(userManager, roleManager);
                 await Infrastructure.Identity.Seeds.DefaultSuperAdmin.SeedAsync(userManager, roleManager);
                 await Infrastructure.Identity.Seeds.DefaultBasicUser.SeedAsync(userManager, roleManager);
              
            }
            catch (Exception ex)
            {
                
            }
            finally
            {
                
            }
        }
    }
}
