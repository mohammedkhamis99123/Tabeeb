using Microsoft.AspNetCore.RateLimiting;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.RateLimiting;

public static class RateLimitConfig
{
    public static IServiceCollection AddRateLimitConfig(this IServiceCollection services)
    {
        services.AddRateLimiter(options =>
        {
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

            options.OnRejected = async (context, cancellationToken) =>
            {
                context.HttpContext.Response.ContentType = "application/json";

                var response = new
                {
                    success = false,
                    statusCode = 429,
                    message = "Too many requests. Please try again later after 1 Minute",
                };

                await context.HttpContext.Response.WriteAsync(
                    JsonSerializer.Serialize(response),
                    cancellationToken);
            };

            options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(
                context =>
                {
                    // 1. معرّف العميل (User ID أو IP)
                    var userIdentifier = context.User.Identity?.IsAuthenticated == true
                        ? context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value!
                        : context.Connection.RemoteIpAddress?.ToString() ?? "anonymous";

                    // 2. معرّف الـ Endpoint (اسم المسار)
                    var endpointIdentifier = context.GetEndpoint()?.DisplayName
                        ?? context.Request.Path.ToString();

                    // 3. دمجهم مع بعض كـ Key فريد لكل Endpoint وبورزر مختلف
                    // الناتج هيكون مثلاً: "User123_POST /api/login"
                    var partitionKey = $"{userIdentifier}_{endpointIdentifier}";

                    return RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey,
                        _ => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit =100,
                            Window = TimeSpan.FromMinutes(1),
                            QueueLimit = 0
                        });
                });
        });

        return services;
    }
}