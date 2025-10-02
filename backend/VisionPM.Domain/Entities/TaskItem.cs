using System.ComponentModel.DataAnnotations;

namespace VisionPM.Domain.Entities;

public enum TaskStatus { Todo = 0, InProgress = 1, Done = 2 }
public enum TaskPriority { Low = 0, Medium = 1, High = 2 }

public class TaskItem
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public TaskStatus Status { get; set; }
    public TaskPriority Priority { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime CreatedAt { get; set; }
}