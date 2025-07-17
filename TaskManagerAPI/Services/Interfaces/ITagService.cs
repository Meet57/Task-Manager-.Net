using TaskManagerAPI.Models;

namespace TaskManagerAPI.Services.Interfaces;

public interface ITagService
{
    Task<IEnumerable<Tag>> GetAllTagsAsync();
}