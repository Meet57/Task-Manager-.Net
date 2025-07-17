using Microsoft.EntityFrameworkCore;
using TaskManagerAPI.Data;
using TaskManagerAPI.Models;
using TaskManagerAPI.Repositories.Interfaces;

namespace TaskManagerAPI.Repositories;

public class TagRepository : ITagRepository
{
    private readonly AppDbContext _context;

    public TagRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Tag>> GetAllAsync()
    {
        return await _context.Tags.ToListAsync();
    }
    
    public async Task<List<Tag>> GetOrCreateTagsByNamesAsync(IEnumerable<string> tagNames)
    {
        var existingTags = await _context.Tags
            .Where(t => tagNames.Contains(t.Name))
            .ToListAsync();

        var newTagNames = tagNames.Except(existingTags.Select(t => t.Name)).Distinct();

        foreach (var name in newTagNames)
        {
            var tag = new Tag { Name = name };
            _context.Tags.Add(tag);
            existingTags.Add(tag);
        }

        await _context.SaveChangesAsync();
        return existingTags;
    }
}