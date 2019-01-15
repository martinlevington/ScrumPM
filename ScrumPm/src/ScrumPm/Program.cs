using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Core;
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
                .Enrich.WithThreadId()
           
                .Enrich.WithProperty(new KeyValuePair<string, object>("applicationId", "SCRUM PM"))
                .Enrich.FromLogContext()
               
                 //  .WriteTo.Async(a => a.ApplicationInsightsTraces("<MyApplicationInsightsInstrumentationKey>"))
                     
                //.WriteTo.Async(r => r.RollingFile("logs/log-"+typeof(Program).Assembly.GetName().Name+"-{Date}.txt", retainedFileCountLimit: 10,
                //    outputTemplate:
                //    "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] {Message}{NewLine}{Exception}{NewLine}Memory: {MemoryUsage}{NewLine}",
                //    levelSwitch: new LoggingLevelSwitch(LogEventLevel.Information)
                //        )
                //)
                       
                .WriteTo.RollingFile("logs/log-"+typeof(Program).Assembly.GetName().Name+"-{Date}.txt", retainedFileCountLimit: 10,
                        outputTemplate:
                        "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] {Message}{NewLine}{Exception}{NewLine}Memory: {MemoryUsage}{NewLine}",
                        levelSwitch: new LoggingLevelSwitch(LogEventLevel.Debug)
                    )
                
                .WriteTo.Async(r => r.RollingFile(new JsonFormatter(), "logs/log-"+typeof(Program).Assembly.GetName().Name+"-json-{Date}.txt"))

                .WriteTo.Console()
                .CreateLogger();

       

            try
            {
                Log.Information("Starting web host");
                Log.Information("Application version {Version} starting up",
                    typeof(Program).Assembly.GetName().Version);
                 CreateWebHostBuilder(args).Build().Run();

                return 0;
                
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

          

    
         
        }

        private static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
             //   .UseKestrel()
             //   .UseContentRoot(Directory.GetCurrentDirectory())
               // .UseIISIntegration()
                .UseStartup<Startup>()
                .UseConfiguration(Configuration)
                .UseSerilog(Log.Logger);
    }
}
