using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;
using TaskManagerAPI.DTOs;
using TaskManagerAPI.Models;
using TaskManagerAPI.Services.Interfaces;

namespace TaskManagerAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly ITaskService _taskService;

    public TasksController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TaskItem>>> GetTasks()
    {
        var tasks = await _taskService.GetAllTasksAsync();
        return Ok(tasks);
    }
    
    [HttpPost]
    public async Task<ActionResult<TaskItem>> CreateTask([FromBody] TaskCreateDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var created = await _taskService.CreateTaskAsync(dto);
        return CreatedAtAction(nameof(GetTasks), new { id = created.Id }, created);
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTask(int id, [FromBody] TaskUpdateDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var updated = await _taskService.UpdateTaskAsync(id, dto);
        if (!updated)
            return NotFound();

        return NoContent();
    }

    [HttpPost("/toogle/{id}")]
    public async Task<IActionResult> ToogleTaskCompletion(int id)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var result = await _taskService.ToggleTaskCompletionAsync(id);
        
        if(result)
            return Ok();
        return NotFound();
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTask(int id)
    {
        var deleted = await _taskService.DeleteTaskAsync(id);
        
        if (!deleted)
            return NotFound();
        
        return NoContent();
    }
    
    [HttpGet("/search")]
    public async Task<ActionResult<IEnumerable<TaskItem>>> GetFilteredTasks(
        [FromQuery] bool? isCompleted,
        [FromQuery] string? tag,
        [FromQuery] string? search)
    {
        var tasks = await _taskService.FilterTasksAsync(isCompleted, tag, search);
        return Ok(tasks);
    }
}