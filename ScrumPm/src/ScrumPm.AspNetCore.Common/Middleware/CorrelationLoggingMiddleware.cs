using System.Threading.Tasks;
using CorrelationId;
using CorrelationId.Abstractions;
using Microsoft.AspNetCore.Http;
using Serilog.Context;

namespace ScrumPm.AspNetCore.Common.Middleware
{
    public class CorrelationLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ICorrelationContextAccessor _correlationContext;

        public CorrelationLoggingMiddleware(RequestDelegate next, ICorrelationContextAccessor correlationContext)
        {
            _next = next;
            _correlationContext = correlationContext;
        }

        public async Task Invoke(HttpContext context)
        {
                using (LogContext.PushProperty("CorrelationId", _correlationContext.CorrelationContext.CorrelationId))
                {
                    await _next.Invoke(context);
                }
           
        }
    }
}
