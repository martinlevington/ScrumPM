using Microsoft.AspNetCore.Builder;

namespace ScrumPm.Middleware
{
    public static class RemoteIpAddressLoggingMiddlewareExtensions
    {
        public static IApplicationBuilder UseRemoteIpAddressLoggingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RemoteIpAddressLoggingMiddleware>();
        }
    }
}
