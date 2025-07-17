using System.ComponentModel.DataAnnotations;

namespace TaskManagerAPI.DTOs;

public class TaskCreateDto
{
    [Required]
    public string Title { get; set; }

    public string? Description { get; set; }

    public bool IsCompleted { get; set; } = false;

    public List<string> Tags { get; set; } = new();
}