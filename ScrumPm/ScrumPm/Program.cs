using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ScrumPm.Migrations;
using ScrumPm.Persistence.Database;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;

namespace ScrumPm
{
    public class Program
    {

        public static IConfiguration Configuration { get; } = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        public static int Main(string[] args)
        {
            
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.WithMachineName()
                .Enrich.WithMemoryUsage()
                .Enrich.WithEnvironment("OS")
                .Enrich.WithProperty(new KeyValuePair<string, object>("applicationId", "SCRUM PM"))
                 //  .WriteTo.Async(a => a.ApplicationInsightsEvents("<MyApplicationInsightsInstrumentationKey>"))
                     
                .WriteTo.Async(r => r.RollingFile("logs/log-{Date}.txt", retainedFileCountLimit: 10,
                    outputTemplate:
                    "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] {Message}{NewLine}{Exception}{NewLine}Memory: {MemoryUsage}{NewLine}"))
                .WriteTo.Async(r => r.RollingFile(new JsonFormatter(), "logs/log-json-{Date}.txt"))

                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();

            IWebHost host;

            try
            {
                Log.Information("Starting web host");
                host = BuildWebHost(args);
                
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<ScrumPMContext>();
                try
                {
                    context.Database.EnsureCreated();
                    SeedData.Initialize(services);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred seeding the DB.");
                }
            }

            host.Run();
            return 0;
        }

        private static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .UseConfiguration(Configuration)
                .UseSerilog() 
                .Build();
    }
}
