using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;

namespace ScrumPm
{
    public class Program
    {
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
            
   
            try
            {
                Log.Information("Starting web host");
                BuildWebHost(args).Run();
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

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .UseSerilog() 
                .Build();
    }
}
