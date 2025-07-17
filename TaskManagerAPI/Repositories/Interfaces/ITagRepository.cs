namespace TaskManagerAPI.Repositories.Interfaces;

using TaskManagerAPI.Models;

public interface ITagRepository
{
    Task<IEnumerable<Tag>> GetAllAsync();
    Task<List<Tag>> GetOrCreateTagsByNamesAsync(IEnumerable<string> tagNames);
}