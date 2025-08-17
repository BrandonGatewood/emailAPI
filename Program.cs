using DotNetEnv;
using emailAPI.Services;
using emailAPI.Middleware;

Env.Load();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<EmailService>();

builder.Services.AddControllers();

var app = builder.Build();

app.UseMiddleware<ApiKeyMiddleware>();
app.MapControllers();
app.Run();