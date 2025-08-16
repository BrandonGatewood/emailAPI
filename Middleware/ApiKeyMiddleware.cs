namespace emailAPI.Middleware
{
    public class ApiKeyMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;
        private readonly string _apiKey = Environment.GetEnvironmentVariable("API_KEY")
            ?? throw new InvalidOperationException("API_KEY is not set in environment variables.");

        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Request.Headers.TryGetValue("x-api-key", out var key) || key != _apiKey)
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Unauthorized");
                return;
            }

            await _next(context);
        }
    }
}