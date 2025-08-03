using TaskManagerAPI.DTOs;

namespace TaskManagerAPI.Repositories.Interfaces;

using TaskManagerAPI.Models;

public interface ITaskRepository
{
    Task<IEnumerable<TaskItem>> GetAllAsync();
    Task CreateAsync(TaskItem task);
    Task<TaskItem?> CreateTaskUsingProcedureAsync(TaskCreateDto task);
    
    Task<TaskItem?> GetByIdAsync(int id);
    Task UpdateAsync(TaskItem task);
    
    Task DeleteAsync(TaskItem task);
    
    Task<IEnumerable<TaskItem>> FilterTasksAsync(Boolean? isCompleted, string tag, string search);
}