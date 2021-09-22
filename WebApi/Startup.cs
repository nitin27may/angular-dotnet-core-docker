using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using WebApi.Data.Helper;
using WebApi.Data.Repository;
using WebApi.Helpers;
using WebApi.Models;

namespace WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebApi", Version = "v1" });
            });

            services.AddDbContext<DataContext>(p => p.UseNpgsql(Configuration.GetSection("AppSettings:ConnectionStrings:DefaultConnection").Value));
            services.AddScoped<IUserRepository, UserRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // app.ConfigureExceptionHandler(logger);
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApi v1"));
            }
            app.UseMiddleware<SerilogRequestLogger>();

            //using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>()
            // .CreateScope())
            //{
            //    serviceScope.ServiceProvider.GetService<DataContext>()
            //        .Database.Migrate();
            //}
            app.UseHttpsRedirection();

            app.UseRouting();

             app.UseCors(corsPolicyBuilder => corsPolicyBuilder
             .WithOrigins(Configuration["AppSettings:CorsAllowdOrigins"].ToString().Split(','))
             .AllowAnyMethod()
             .AllowAnyHeader()
             .AllowCredentials()
             );
        

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
