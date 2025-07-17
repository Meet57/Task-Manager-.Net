using Microsoft.AspNetCore.Mvc;
using TaskManagerAPI.Services.Interfaces;
using TaskManagerAPI.Models;

namespace TaskManagerAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TagsController : ControllerBase
{
    private readonly ITagService _tagService;

    public TagsController(ITagService tagService)
    {
        _tagService = tagService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Tag>>> GetTags()
    {
        var tags = await _tagService.GetAllTagsAsync();
        return Ok(tags);
    }
}