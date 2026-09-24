namespace TodoApi.Validators;

using FluentValidation;
using TodoApi.DTOs;
using TodoApi.Resources;

public class CreateTodoItemUseCase : AbstractValidator<TodoItemDTO>
{
    public CreateTodoItemUseCase()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(ResourceMessages.NAME_REQUIRED)
            .MinimumLength(3).WithMessage(ResourceMessages.NAME_MINIMUM_LENGTH)
            .MaximumLength(100).WithMessage(ResourceMessages.NAME_MAXIMUM_LENGTH);

        RuleFor(x => x.IsComplete)
            .NotNull().WithMessage(ResourceMessages.ISCOMPLETE_REQUIRED);
    }
}
