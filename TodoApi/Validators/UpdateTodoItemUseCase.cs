namespace TodoApi.Validators;

using FluentValidation;
using TodoApi.Models;

public class UpdateTodoItemUseCase : AbstractValidator<PutTodoItemDTO>
{
    public UpdateTodoItemUseCase()
    {
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id is required.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("IsComplete is required.")
                .MinimumLength(3).WithMessage("Name must have three or more characters.");

            RuleFor(x => x.IsComplete)
                .NotNull().WithMessage("IsComplete is required.");
        }
    }
}
