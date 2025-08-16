using DotNetEnv;
using emailAPI.Services;
using emailAPI.Middleware;

var builder = WebApplication.CreateBuilder(args);

Env.Load();
builder.Services.AddSingleton<EmailService>();

builder.Services.AddControllers();

var app = builder.Build();

app.UseMiddleware<ApiKeyMiddleware>();
app.MapControllers();
app.Run();