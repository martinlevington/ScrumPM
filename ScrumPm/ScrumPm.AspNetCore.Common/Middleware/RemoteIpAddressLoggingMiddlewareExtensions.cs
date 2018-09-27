using Microsoft.AspNetCore.Builder;

namespace ScrumPm.AspNetCore.Common.Middleware
{
    public static class RemoteIpAddressLoggingMiddlewareExtensions
    {
        public static IApplicationBuilder UseRemoteIpAddressLoggingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RemoteIpAddressLoggingMiddleware>();
        }
    }
}
