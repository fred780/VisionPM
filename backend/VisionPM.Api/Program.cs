using Microsoft.EntityFrameworkCore;
using VisionPM.Application.Abstractions;
using VisionPM.Domain.Entities;
using VisionPM.Infrastructure;
using VisionPM.Infrastructure.Services;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;
using Serilog;


var builder = WebApplication.CreateBuilder(args);
var cfg = builder.Configuration;
var services = builder.Services;
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();
builder.Host.UseSerilog();

var useSqlite = cfg.GetValue<bool>("UseSqlite");
if (useSqlite)
    services.AddDbContext<AppDbContext>(o => o.UseSqlite(cfg.GetConnectionString("sqlite")));
else
    services.AddDbContext<AppDbContext>(o => o.UseInMemoryDatabase("visionPM"));

services.AddScoped<ITaskService, TaskService>();
services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();
services.AddCors(o => o.AddPolicy("web", p => p
    .WithOrigins("http://localhost:5173")
    .AllowAnyHeader().AllowAnyMethod()));

services.AddRateLimiter(options =>
{
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "global",
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 20, // Max 20 requests
                Window = TimeSpan.FromMinutes(1),
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                QueueLimit = 0
            }));
    options.RejectionStatusCode = 429;
});

var app = builder.Build();
app.UseCors("web");
app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();
app.MapGet("/health", () => new { ok = true });

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

app.Run();
