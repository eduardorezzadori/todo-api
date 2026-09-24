namespace TodoApi.Models;

public class UpdateTodoItemDTO
{
    public string Name { get; set; } = string.Empty;
    public bool? IsComplete { get; set; }
    public Guid UserId { get; set; }
}
