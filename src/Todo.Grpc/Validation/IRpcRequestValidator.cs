namespace Todo.Grpc.Validation;

public interface IRpcRequestValidator
{
    public Task ValidateAndThrowIfInvalidAsync<T>(T request, CancellationToken cancellationToken = default);
}