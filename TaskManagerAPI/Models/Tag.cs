using System.Text.Json.Serialization;
using TaskManagerAPI.Models;

public class Tag
{
    public int Id { get; set; }
    public string Name { get; set; }

    [JsonIgnore]
    public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
}