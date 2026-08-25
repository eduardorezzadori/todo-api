namespace TodoApi.Validators;

using FluentValidation;
using TodoApi.Models;

public class CreateTodoItemUseCase : AbstractValidator<TodoItemDTO>
{
    public CreateTodoItemUseCase()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("IsComplete is required.")
            .MinimumLength(3).WithMessage("Name must have three or more characters.");

        RuleFor(x => x.IsComplete)
            .NotNull().WithMessage("IsComplete is required.");
    }
}
