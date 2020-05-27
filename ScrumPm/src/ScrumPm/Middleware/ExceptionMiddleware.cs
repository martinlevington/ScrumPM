

using System;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ScrumPm.Domain.Common.Exceptions;

namespace ScrumPm.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger _logger;
        private readonly RequestDelegate _request;

        public ExceptionMiddleware(IWebHostEnvironment environment, RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _environment = environment;
            _logger = loggerFactory.CreateLogger<ExceptionMiddleware>();
            _request = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _request(context).ConfigureAwait(false);
            }
            catch (DomainException exception)
            {
                context.Response.Clear();
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(exception.Message).ConfigureAwait(false);
            }
            catch (Exception exception)
            {
                if (_environment.IsDevelopment())
                {
                    throw;
                }

                _logger.Log(LogLevel.Error, exception, exception.Message);
                context.Response.Clear();
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = MediaTypeNames.Text.Plain;
                await context.Response.WriteAsync(string.Empty).ConfigureAwait(false);
            }
        }
    }
}
 