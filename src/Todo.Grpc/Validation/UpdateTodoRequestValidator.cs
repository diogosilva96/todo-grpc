using FluentValidation;

namespace Todo.Grpc.Validation;

public class UpdateTodoRequestValidator : AbstractValidator<UpdateTodoRequest>
{
    public UpdateTodoRequestValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);

        RuleFor(x => x.Description).NotEmpty();

        RuleFor(x => x.Title).NotEmpty();

        RuleFor(x => x.Status).NotEmpty();
    }
}