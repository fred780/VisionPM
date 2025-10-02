using Microsoft.AspNetCore.Mvc;
using VisionPM.Application.Abstractions;
using VisionPM.Domain.DTOs;
using VisionPM.Domain.Entities;

namespace VisionPM.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly ITaskService _svc;
    private readonly ILogger<TasksController> _logger;

    public TasksController(ITaskService svc, ILogger<TasksController> logger)
    {
        _svc = svc;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TaskItem>>> Get([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        _logger.LogInformation("Getting all tasks");

        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;

        var allItems = await _svc.ListAsync();
        var pagedItems = allItems
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var totalCount = allItems.Count();

        return Ok(new
        {
            items = pagedItems,
            page,
            pageSize,
            totalCount
        });
    }

    [HttpPost]
    public async Task<ActionResult<TaskItem>> Create([FromBody] TaskCreateDto dto)
    {
        _logger.LogInformation("Creating a new task with title: {Title}", dto.Title);
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        var item = await _svc.CreateAsync(dto);
        return CreatedAtAction(nameof(Get), new { id = item.Id }, item);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<TaskItem>> Update(Guid id, [FromBody] TaskUpdateDto dto)
    {
        _logger.LogInformation("Updating task with ID: {Id}", id);
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);
        
        var updated = await _svc.UpdateAsync(id, dto);
        if (updated == null) return NotFound();

        return Ok(updated);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        _logger.LogInformation("Deleting task with ID: {Id}", id);
        var deleted = await _svc.DeleteAsync(id);
        if (!deleted) return NotFound();
        return NoContent();
    }
}