namespace TodoApi.DTOs;

public class UserDTO
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}
