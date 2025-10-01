using Microsoft.EntityFrameworkCore;
using VisionPM.Domain.Entities;

namespace VisionPM.Infrastructure;

public class AppDbContext : DbContext
{
    public DbSet<TaskItem> Tasks => Set<TaskItem>();
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var e in ChangeTracker.Entries<TaskItem>())
            if (e.State == EntityState.Modified) e.Entity.UpdatedAt = DateTime.UtcNow;
        return base.SaveChangesAsync(cancellationToken);
    }
}