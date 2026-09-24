using FluentValidation;
using TodoApi.DTOs;

namespace TodoApi.Validators.User;

public class CreateUserUseCase : AbstractValidator<CreateUserDTO>
{
    public CreateUserUseCase()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(150);
    }
}