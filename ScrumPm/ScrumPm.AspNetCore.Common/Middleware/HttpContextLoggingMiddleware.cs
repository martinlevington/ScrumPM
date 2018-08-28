using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Serilog;
using Serilog.Events;

namespace ScrumPm.Middleware
{
    public class HttpContextLoggingMiddleware
    {

        private const string MessageTemplate =
            "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";

        private static readonly ILogger Log = Serilog.Log.ForContext<HttpContextLoggingMiddleware>();

        private readonly RequestDelegate _next;

        public HttpContextLoggingMiddleware(RequestDelegate next)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
        }

        public async Task Invoke(HttpContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var sw = Stopwatch.StartNew();
            try
            {
                await _next(context);
                sw.Stop();

                var statusCode = context.Response?.StatusCode;
                var level = statusCode > 499 ? LogEventLevel.Error : LogEventLevel.Information;

                LogForErrorContext(context).Write(level, MessageTemplate, context.Request.Method, context.Request.Path,
                    statusCode, sw.Elapsed.TotalMilliseconds);
            }
            
            catch (Exception ex) when (LogException(context, sw, ex))
            {
                // Never caught, because `LogException()` returns false.
            }
        }


        private static bool LogException(HttpContext httpContext, Stopwatch sw, Exception ex)
        {
            sw.Stop();

            LogForErrorContext(httpContext)
                .Error(ex, MessageTemplate, httpContext.Request.Method, httpContext.Request.Path, 500,
                    sw.Elapsed.TotalMilliseconds);

            return false;
        }


        private static ILogger LogForErrorContext(HttpContext httpContext)
        {
            var request = httpContext.Request;

            var result = Log
                    .ForContext("RequestHeaders", request.Headers.ToDictionary(h => h.Key, h => h.Value.ToString()),
                        true)
                    .ForContext("RequestHost", request.Host)
                    .ForContext("RequestContentType", request.ContentType)
                    .ForContext("RequestHttpContext", request.HttpContext)
                    .ForContext("RequestMethod", request.Method)
                    .ForContext("RequestIsHttps", request.IsHttps)
                    .ForContext("RequestPath", request.Path)
                    .ForContext("RequestProtocol", request.Protocol)
                    .ForContext("RequestPathBase", request.PathBase)
                    .ForContext("RequestQuery", request.Query.ToDictionary(h => h.Key, h => h.Value.ToString()),
                        true)
                ;

            if (request.HasFormContentType)
            {
                result = result.ForContext("RequestForm",
                    request.Form.ToDictionary(v => v.Key, v => v.Value.ToString()));
            }

            return result;
        }
    }
}
