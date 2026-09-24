using Microsoft.AspNetCore.Mvc;
using TodoApi.DTOs;
using TodoApi.Services;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly UserService _service;

    public UsersController(UserService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<ActionResult<UserDTO>> Create(
        CreateUserDTO dto)
    {
        var user = await _service.CreateAsync(dto);

        return CreatedAtAction(
            nameof(GetById),
            new { id = user.Id },
            user
        );
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDTO>>> GetAll()
    {
        var user = await _service.GetAllUsersAsync();

        if (user is null)
            return NotFound();

        return Ok(user);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<UserDTO>> GetById(Guid id)
    {
        var user = await _service.GetByIdAsync(id);

        if (user is null)
            return NotFound();

        return Ok(user);
    }
}