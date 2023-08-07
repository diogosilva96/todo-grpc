using FluentValidation;

namespace Todo.Grpc.Validation;

public class GetTodoRequestValidator : AbstractValidator<GetTodoRequest>
{
    public GetTodoRequestValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
    }
}