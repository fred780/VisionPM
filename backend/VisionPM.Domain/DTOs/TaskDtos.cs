using System.ComponentModel.DataAnnotations;
using VisionPM.Domain.Entities;

namespace VisionPM.Domain.DTOs;

public record TaskCreateDto(
    [Required, MaxLength(120)] string Title,
    TaskPriority Priority,
    DateTime? DueDate,
    string? Tags
);

public record TaskUpdateDto(
    [Required, MaxLength(120)] string Title,
    TaskPriority Priority,
    VisionPM.Domain.Entities.TaskStatus Status,
    DateTime? UpdatedAt,
    DateTime? DueDate,
    string? Tags
);