using StackExchange.Redis;

namespace emailAPI.Middleware
{
    public class RateLimiterMiddleware(RequestDelegate next, IConnectionMultiplexer redis, IConfiguration config)
    {
        private readonly RequestDelegate _next = next;
        private readonly IConnectionMultiplexer _redis = redis;
        private readonly int _limit = int.Parse(config["RATE_LIMIT"]
            ?? throw new InvalidOperationException("RATE_LIMIT is not set in environment variables.")); 
        private readonly int _windowSeconds = int.Parse(config["WINDOW_SEC"]
            ?? throw new InvalidOperationException("WINDOW_SEC is not set in environment variables.")); 

        public async Task InvokeAsync(HttpContext context)
        {
            var ip = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            var db = _redis.GetDatabase();
            string key = $"rate_limit:{ip}";
            var count = await db.StringIncrementAsync(key);

            if (count == 1)
            {
                await db.KeyExpireAsync(key, TimeSpan.FromSeconds(_windowSeconds));
            }

            if (count > _limit)
            {
                context.Response.StatusCode = StatusCodes.Status429TooManyRequests;

                var ttl = await db.KeyTimeToLiveAsync(key);
                var retryAfter = ttl?.TotalSeconds ?? _windowSeconds;

                context.Response.Headers.RetryAfter = retryAfter.ToString("F0");
                await context.Response.WriteAsync("Too many requests. Try again later.");
                return;
            }

            await _next(context);
        }
    }
}