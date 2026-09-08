namespace TodoApi.Validators;

using FluentValidation;
using TodoApi.Models;
using TodoApi.Resources;

public class UpdateTodoItemUseCase : AbstractValidator<TodoItemDTO>
{
    public UpdateTodoItemUseCase()
    {
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage(ResourceMessages.NAME_REQUIRED)
                .MinimumLength(3).WithMessage(ResourceMessages.NAME_MINIMUM_LENGTH)
                .MaximumLength(100).WithMessage(ResourceMessages.NAME_MAXIMUM_LENGTH);
        }
    }
}
