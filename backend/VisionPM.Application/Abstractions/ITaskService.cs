using VisionPM.Domain.DTOs;
using VisionPM.Domain.Entities;

namespace VisionPM.Application.Abstractions;

public interface ITaskService
{
    Task<IEnumerable<TaskItem>> ListAsync();
    Task<TaskItem> CreateAsync(TaskCreateDto dto);
    Task<TaskItem?> UpdateAsync(Guid id, TaskUpdateDto dto);
    Task<bool> DeleteAsync(Guid id);
}