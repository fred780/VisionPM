using Microsoft.EntityFrameworkCore;
using VisionPM.Application.Abstractions;
using VisionPM.Domain.DTOs;
using VisionPM.Domain.Entities;

namespace VisionPM.Infrastructure.Services;

public class TaskService : ITaskService
{
    private readonly AppDbContext _db;
    public TaskService(AppDbContext db) => _db = db;

    public async Task<IEnumerable<TaskItem>> ListAsync()
        => await _db.Tasks.ToListAsync();

    public async Task<TaskItem> CreateAsync(TaskCreateDto dto)
    {
        var item = new TaskItem
        {
            Id = Guid.NewGuid(),
            Title = dto.Title,
            Priority = dto.Priority,
            Status = VisionPM.Domain.Entities.TaskStatus.InProgress,
            CreatedAt = DateTime.UtcNow
        };
        _db.Tasks.Add(item);
        await _db.SaveChangesAsync();
        return item;
    }

    public async Task<TaskItem?> UpdateAsync(Guid id, TaskUpdateDto dto)
    {
        var item = await _db.Tasks.FindAsync(id);
        if (item == null) return null;
        item.Title = dto.Title;
        item.Priority = dto.Priority;
        item.Status = dto.Status;
        item.UpdatedAt = dto.UpdatedAt ?? DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return item;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var item = await _db.Tasks.FindAsync(id);
        if (item == null) return false;
        _db.Tasks.Remove(item);
        await _db.SaveChangesAsync();
        return true;
    }
}