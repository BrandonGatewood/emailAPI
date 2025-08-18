using DotNetEnv;
using emailAPI.Services;
using emailAPI.Middleware;
using emailAPI.Extensions;

Env.Load();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRedis(builder.Configuration);
builder.Services.AddSingleton<IEmailService, EmailService>();

builder.Services.AddControllers();

var app = builder.Build();

app.UseMiddleware<ApiKeyMiddleware>();
app.UseMiddleware<RateLimiterMiddleware>();
app.MapControllers();
app.Run();