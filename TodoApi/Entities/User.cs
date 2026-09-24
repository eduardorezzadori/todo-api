namespace TodoApi.Entities;

public class User
{
    public Guid Id { get; private set; } = Guid.CreateVersion7();

    public string Name { get; private set; } = string.Empty;

    public string Email { get; private set; } = string.Empty;

    public ICollection<TodoItem> TodoItems { get; private set; } = [];

    private User()
    {
    }

    public User(string name, string email)
    {
        Name = name;
        Email = email;
    }
}