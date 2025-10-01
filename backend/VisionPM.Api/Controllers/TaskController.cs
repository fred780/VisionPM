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
    public TasksController(ITaskService svc) => _svc = svc;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TaskItem>>> Get()
        => Ok(await _svc.ListAsync());

    [HttpPost]
    public async Task<ActionResult<TaskItem>> Create([FromBody] TaskCreateDto dto)
    {
        var item = await _svc.CreateAsync(dto);
        return CreatedAtAction(nameof(Get), new { id = item.Id }, item);
    }
}