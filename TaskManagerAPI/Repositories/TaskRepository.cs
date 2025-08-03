using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using TaskManagerAPI.Data;
using TaskManagerAPI.DTOs;
using TaskManagerAPI.Models;
using TaskManagerAPI.Repositories.Interfaces;

namespace TaskManagerAPI.Repositories;

public class TaskRepository : ITaskRepository
{
    private readonly AppDbContext _context;

    public TaskRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TaskItem>> GetAllAsync()
    {
        return await _context.Tasks.Include(t => t.Tags).ToListAsync();
    }
    
    public Task CreateAsync(TaskItem task)
    {
        _context.Tasks.Add(task);
        return _context.SaveChangesAsync();
    }
    
    public async Task<TaskItem?> CreateTaskUsingProcedureAsync(TaskCreateDto task)
    {
        var tagString = task.Tags != null ? string.Join(",", task.Tags) : "";

        var result = await _context.Database.ExecuteSqlRawAsync(
            "CALL CreateTaskWithTags({0}, {1}, {2}, {3})",
            task.Title,
            task.Description ?? "",
            task.IsCompleted,
            tagString
        );

        var latestTask = await _context.Tasks
            .Include(t => t.Tags)
            .OrderByDescending(t => t.Id)
            .FirstOrDefaultAsync();

        return latestTask;
    }
    
    public async Task<TaskItem?> GetByIdAsync(int id)
    {
        return await _context.Tasks.Include(t => t.Tags).FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task UpdateAsync(TaskItem task)
    {
        _context.Tasks.Update(task);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(TaskItem task)
    {
        _context.Tasks.Remove(task);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<TaskItem>> FilterTasksAsync(Boolean? isCompleted, string tag, string search)
    {
        var query = _context.Tasks.Include(t => t.Tags).AsQueryable();

        if (isCompleted.HasValue)
        {
            query = query.Where(t => t.IsCompleted == isCompleted.Value);
        }

        if (!string.IsNullOrEmpty(tag))
        {
            var tagList = tag.Split(',').Select(t => t.Trim()).ToList();
            query = query.Where(t => t.Tags.Any(tag => tagList.Contains(tag.Name)));
        }

        if (!string.IsNullOrEmpty(search))
        {
            query = query.Where(t => t.Title.Contains(search) || t.Description.Contains(search));
        }
        
        return await query.ToListAsync();
    }

}