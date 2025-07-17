using TaskManagerAPI.Models;
using TaskManagerAPI.DTOs;

namespace TaskManagerAPI.Services.Interfaces;

public interface ITaskService
{
    Task<IEnumerable<TaskItem>> GetAllTasksAsync();
    Task<TaskItem> CreateTaskAsync(TaskCreateDto dto);
    Task<bool> UpdateTaskAsync(int id, TaskUpdateDto dto);
    
    Task<bool> DeleteTaskAsync(int id);
    
    Task<bool> ToggleTaskCompletionAsync(int id);
    
    Task<IEnumerable<TaskItem>> FilterTasksAsync(Boolean? isCompleted, string tag, string search);
}