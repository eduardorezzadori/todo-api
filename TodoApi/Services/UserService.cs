using FluentValidation;
using TodoApi.DTOs;
using TodoApi.Entities;
using TodoApi.Repository;

namespace TodoApi.Services;

public class UserService
{
    private readonly UserRepository _repository;
    private readonly IValidator<CreateUserDTO> _validator;

    public UserService(
        UserRepository repository,
        IValidator<CreateUserDTO> validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task<UserDTO> CreateAsync(CreateUserDTO dto)
    {
        var validationResult = await _validator.ValidateAsync(dto);

        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var user = new User(
            dto.Name,
            dto.Email
        );

        var createdUser = await _repository.AddAsync(user);

        return new UserDTO
        {
            Id = createdUser.Id,
            Name = createdUser.Name,
            Email = createdUser.Email
        };
    }

    public async Task<UserDTO?> GetByIdAsync(Guid id)
    {
        var user = await _repository.GetByIdAsync(id);
        if (user == null)
        {
            return null;
        }
        return new UserDTO
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email
        };
    }

    public async Task<IEnumerable<UserDTO>> GetAllUsersAsync()
    {

        var users = await _repository.GetAllAsync();
        var userDTOs = users.Select(user => new UserDTO
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email
        });
        return userDTOs;
    }
}