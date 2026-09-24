using TodoApi.Entities;

public class TodoItem
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public bool IsComplete { get; set; }

    public Guid UserId { get; set; }

    public User User { get; set; } = null!;
}
