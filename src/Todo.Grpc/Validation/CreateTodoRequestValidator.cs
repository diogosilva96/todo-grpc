using FluentValidation;

namespace Todo.Grpc.Validation;

public class CreateTodoRequestValidator : AbstractValidator<CreateTodoRequest>
{
    public CreateTodoRequestValidator()
    {
        RuleFor(x => x.Description).NotEmpty();

        RuleFor(x => x.Title).NotEmpty();
    }
}