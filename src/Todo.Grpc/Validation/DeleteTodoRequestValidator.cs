using FluentValidation;

namespace Todo.Grpc.Validation;

public class DeleteTodoRequestValidator : AbstractValidator<DeleteTodoRequest>
{
    public DeleteTodoRequestValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
    }
}