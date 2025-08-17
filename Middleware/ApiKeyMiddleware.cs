using System.Security.Cryptography;
using System.Text;

namespace emailAPI.Middleware
{
    public class ApiKeyMiddleware(RequestDelegate next, IConfiguration config)
    {
        private readonly RequestDelegate _next = next;
        private readonly byte[] _apiKeyBytes = Encoding.UTF8.GetBytes(
            config["API_KEY"] ?? throw new InvalidOperationException("API key is not set in environment variables.")
        );
        public async Task InvokeAsync(HttpContext context)
        {
            // check if header has api key or compare every character in constant time
            if (!context.Request.Headers.TryGetValue("x-api-key", out var key) ||
                !CryptographicOperations.FixedTimeEquals(
                    Encoding.UTF8.GetBytes(key.ToString()), _apiKeyBytes))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }

            await _next(context);
        }
    }
}