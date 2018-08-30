using Microsoft.AspNetCore.Builder;

namespace WebApiRateLimiter
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseApiRateLimitMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ApiRateLimitMiddleware>();
        }
    }
}