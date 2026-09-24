namespace TodoApi.DTOs;

public class TodoItemDTO
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public string Name { get; set; } = string.Empty;
    public bool IsComplete { get; set; }

    public Guid UserId { get; set; }
}
