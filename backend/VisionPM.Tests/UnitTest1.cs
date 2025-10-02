using VisionPM.Infrastructure.Services;
using VisionPM.Domain.DTOs;
using VisionPM.Domain.Entities;
using VisionPM.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Xunit;

public class TaskServiceTests
{
    private AppDbContext GetInMemoryDb()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;
        return new AppDbContext(options);
    }

    [Fact]
    public async Task CreateAsync_ShouldAddTask()
    {
        // Arrange
        var db = GetInMemoryDb();
        var svc = new TaskService(db);
        var dto = new TaskCreateDto("Test Task", TaskPriority.Medium);

        // Act
        var result = await svc.CreateAsync(dto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Test Task", result.Title);
        Assert.Equal(TaskPriority.Medium, result.Priority);
    }
    [Fact]
    public async Task UpdateAsync_ShouldUpdateTask()
    {
        // Arrange
        var db = GetInMemoryDb();
        var svc = new TaskService(db);
        var createDto = new TaskCreateDto("Original Task", TaskPriority.Low);
        var created = await svc.CreateAsync(createDto);

        var updateDto = new TaskUpdateDto(
            "Updated Task",
            TaskPriority.High,
            VisionPM.Domain.Entities.TaskStatus.Todo,
            DateTime.UtcNow
        );

        // Act
        var updated = await svc.UpdateAsync(created.Id, updateDto);

        // Assert
        Assert.NotNull(updated);
        Assert.Equal("Updated Task", updated.Title);
        Assert.Equal(TaskPriority.High, updated.Priority);
        Assert.Equal(VisionPM.Domain.Entities.TaskStatus.Todo, updated.Status);
    }
}