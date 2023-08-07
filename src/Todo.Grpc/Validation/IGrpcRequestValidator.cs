namespace Todo.Grpc.Validation;

public interface IGrpcRequestValidator
{
    public Task ValidateAndThrowIfInvalidAsync<T>(T request, CancellationToken cancellationToken = default);
}