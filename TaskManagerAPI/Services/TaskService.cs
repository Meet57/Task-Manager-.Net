using TaskManagerAPI.DTOs;
using TaskManagerAPI.Models;
using TaskManagerAPI.Repositories.Interfaces;
using TaskManagerAPI.Services.Interfaces;

namespace TaskManagerAPI.Services;

public class TaskService : ITaskService
{
    private readonly ITaskRepository _taskRepository;
    private readonly ITagRepository _tagRepository;

    public TaskService(ITaskRepository taskRepository, ITagRepository tagRepository)
    {
        _taskRepository = taskRepository;
        _tagRepository = tagRepository;
    }

    public async Task<IEnumerable<TaskItem>> GetAllTasksAsync()
    {
        return await _taskRepository.GetAllAsync();
    }
    
    public async Task<TaskItem> CreateTaskAsync(TaskCreateDto dto)
    {
        var tags = await _tagRepository.GetOrCreateTagsByNamesAsync(dto.Tags);

        var task = new TaskItem
        {
            Title = dto.Title,
            Description = dto.Description,
            IsCompleted = dto.IsCompleted,
            CreatedAt = DateTime.UtcNow,
            Tags = tags
        };

        await _taskRepository.CreateAsync(task);
        return task;
    }

    public async Task<bool> ToggleTaskCompletionAsync(int id)
    {
        var task = await _taskRepository.GetByIdAsync(id);
        
        if (task == null)
            return false;
        
        task.IsCompleted = !task.IsCompleted;
        await _taskRepository.UpdateAsync(task);
        return true;
    }

    public async Task<IEnumerable<TaskItem>> FilterTasksAsync(bool? isCompleted, string tag, string search)
    {
        var result = await _taskRepository.FilterTasksAsync(isCompleted, tag, search);
        
        return result;
    }

    public async Task<bool> UpdateTaskAsync(int id, TaskUpdateDto dto)
    {
        var task = await _taskRepository.GetByIdAsync(id);
        if (task == null)
            return false;

        if (!string.IsNullOrWhiteSpace(dto.Title))
            task.Title = dto.Title;
        
        if (dto.Description != null)
            task.Description = dto.Description;
        
        task.IsCompleted = (bool)dto.IsCompleted!;
        
        if (dto.Tags != null && dto.Tags.Any())
            task.Tags = await _tagRepository.GetOrCreateTagsByNamesAsync(dto.Tags);

        await _taskRepository.UpdateAsync(task);
        return true;
    }

    public async Task<bool> DeleteTaskAsync(int id)
    {
        TaskItem? task = await _taskRepository.GetByIdAsync(id);
        
        if(task == null)
            return false;

        await _taskRepository.DeleteAsync(task);
        
        return true;
    }
}