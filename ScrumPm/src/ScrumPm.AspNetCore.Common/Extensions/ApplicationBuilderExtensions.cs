using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using ScrumPm.AspNetCore.Common.Middleware;

namespace ScrumPm.AspNetCore.Common.Extensions
{
    /// <summary>
    /// Extensions Methods to help organise building the application during start up
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Configure the application to use Cross-Origin Resource Sharing in a ver 'relaxed way'
        /// This method is useful during development but a tighter CORS specfication should be used for production.
        /// </summary>
        /// <param name="application">The application being configured</param>
        public static void UseCorsCustom(this IApplicationBuilder application)
        {
            application.UseCors(cors => cors.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().AllowCredentials());
        }

        /// <summary>
        /// Configure the application exception handling depending upon the curretn environment
        /// </summary>
        /// <param name="application">The application being configured</param>
        /// <param name="environment">The current development environment</param>
        public static void UseExceptionCustom(this IApplicationBuilder application, IHostingEnvironment environment)
        {
            if (environment.IsDevelopment())
            {
                application.UseDeveloperExceptionPage();
                application.UseDatabaseErrorPage();
            }

            application.UseExceptionMiddleware();
        }

        /// <summary>
        /// Configure the applciation to use the custom exception middleware
        /// </summary>
        /// <param name="application">The application being configured</param>
        public static void UseExceptionMiddleware(this IApplicationBuilder application)
        {
            application.UseMiddleware<ExceptionMiddleware>();
        }

        /// <summary>
        /// Configure the application to use HTTP Strict Transport Security (Hsts) 
        /// </summary>
        /// <param name="application">The application being configured</param>
        /// <param name="environment">The current development environment</param>
        public static void UseHstsCustom(this IApplicationBuilder application, IHostingEnvironment environment)
        {
            if (!environment.IsDevelopment())
            {
                application.UseHsts();
            }
        }

    }
}
