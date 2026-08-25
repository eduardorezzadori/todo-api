namespace TodoApi.Models;

public class PutTodoItemDTO
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsComplete { get; set; }
}
