using DotNetEnv;
using emailAPI.Services;

var builder = WebApplication.CreateBuilder(args);

Env.Load();

builder.Services.AddSingleton<EmailService>();

builder.Services.AddControllers();

var app = builder.Build();

app.MapControllers();
app.Run();