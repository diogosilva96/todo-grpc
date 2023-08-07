using FluentValidation;
using Todo.Grpc.Services;

namespace Todo.Grpc.Validation;

public class GrpcRequestValidator : IGrpcRequestValidator
{
    private readonly IServiceProvider _serviceProvider;

    public GrpcRequestValidator(IServiceProvider serviceProvider) => _serviceProvider = serviceProvider;

    public async Task ValidateAndThrowIfInvalidAsync<T>(T request,
        CancellationToken cancellationToken = default)
    {
        var validator = GetValidatorOrThrow<T>();

        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (validationResult.IsValid) return;

        throw RpcExceptions.BadRequest(validationResult);
    }

    private IValidator<T> GetValidatorOrThrow<T>()
    {
        var validator = _serviceProvider.GetService<IValidator<T>>();
        if (validator == default)
        {
            throw new InvalidOperationException($"Could not find validator for object of type '{typeof(T)}'");
        }

        return validator;
    }
}