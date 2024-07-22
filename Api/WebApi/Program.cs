using Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Threading.Tasks;

namespace WebApi;

public class Program
{
    public static async Task Main(string[] args)
    {
        //Read Configuration from appSettings
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        //Initialize Logger
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(config)
            .CreateLogger();
        var host = CreateHostBuilder(args).Build();
        host.Run();
       
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
        .UseSerilog() //Uses Serilog instead of default .NET Logger
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
}