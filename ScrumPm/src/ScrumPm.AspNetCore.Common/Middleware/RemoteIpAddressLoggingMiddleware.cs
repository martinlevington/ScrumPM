using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Serilog.Context;

namespace ScrumPm.AspNetCore.Common.Middleware
{
    public class RemoteIpAddressLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public RemoteIpAddressLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            using (LogContext.PushProperty("RemoteIpAddress", context.Connection.RemoteIpAddress))
            {
                await _next.Invoke(context);
            }
        }
    }
}
