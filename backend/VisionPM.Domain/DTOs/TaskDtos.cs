using System.ComponentModel.DataAnnotations;
using VisionPM.Domain.Entities;

namespace VisionPM.Domain.DTOs;

public record TaskCreateDto(
    [Required, MaxLength(120)] string Title,
    TaskPriority Priority
);

public record TaskUpdateDto(
    [Required, MaxLength(120)] string Title,
    TaskPriority Priority,
    VisionPM.Domain.Entities.TaskStatus Status,
    DateTime? UpdatedAt
);