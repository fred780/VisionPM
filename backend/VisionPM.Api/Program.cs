using Microsoft.EntityFrameworkCore;
using VisionPM.Application.Abstractions;
using VisionPM.Domain.Entities;
using VisionPM.Infrastructure;
using VisionPM.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);
var cfg = builder.Configuration;
var services = builder.Services;

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
