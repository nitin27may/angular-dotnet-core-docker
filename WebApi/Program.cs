using System;
using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace WebApi
{
    public class Program
    {
        public static IConfiguration Configuration { get; } = new ConfigurationBuilder()
      .SetBasePath(Directory.GetCurrentDirectory())
      .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
      .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true, reloadOnChange: true)
      .AddEnvironmentVariables()
       .Build();


        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
           .ReadFrom.Configuration(Configuration)
           .Enrich.FromLogContext()
           .CreateLogger();

            Serilog.Debugging.SelfLog.Enable(msg =>
            {
                Console.WriteLine(msg);
            });

            try
            {
                Log.Information("Getting the motors running...");

                CreateWebHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");

            }
            finally
            {
                Log.CloseAndFlush();
            }

        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseSerilog(); // <-- Add this line;
        }
    }
}

