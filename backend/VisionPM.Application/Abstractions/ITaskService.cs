using VisionPM.Domain.DTOs;
using VisionPM.Domain.Entities;

namespace VisionPM.Application.Abstractions;

public interface ITaskService
{
    Task<IEnumerable<TaskItem>> ListAsync();
    Task<TaskItem> CreateAsync(TaskCreateDto dto);
}