using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;

namespace WSC.Dashboard.API.RateLimiting
{
    /// <summary>
    /// Defines rate limiting policies for Dashboard API endpoints
    /// </summary>
    public static class RateLimitingPolicy
    {
        public const string DefaultPolicy = "DefaultPolicy";

        /// <summary>
        /// Configures rate limiting policies
        /// </summary>
        public static void AddCustomRateLimiting(this IServiceCollection services)
        {
            services.AddRateLimiter(options =>
            {
                // Default policy: 100 requests per minute per IP
                options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
                    RateLimitPartition.GetSlidingWindowLimiter(
                        partitionKey: context.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                        factory: _ => new SlidingWindowRateLimiterOptions
                        {
                            AutoReplenishment = true,
                            PermitLimit = 100,
                            Window = TimeSpan.FromMinutes(1)
                        }));

                // Configure response when rate limit is exceeded
                options.OnRejected = async (context, cancellationToken) =>
                {
                    context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                    context.HttpContext.Response.ContentType = "application/json";

                    var response = new
                    {
                        success = false,
                        message = "Rate limit exceeded. Please try again later.",
                        retryAfter = context.HttpContext.Response.Headers["Retry-After"].ToString()
                    };

                    await context.HttpContext.Response.WriteAsJsonAsync(response, cancellationToken);
                };
            });
        }
    }
}
