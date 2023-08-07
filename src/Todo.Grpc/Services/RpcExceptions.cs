using FluentValidation.Results;
using Grpc.Core;

namespace Todo.Grpc.Services;

public static class RpcExceptions
{
    public static RpcException NotFound() => new(new(StatusCode.NotFound, "The object could not be found."));

    public static RpcException BadRequest(ValidationResult validationResult) =>
        // TODO: find better way to do (e.g., maybe via metadata)
        new(new(StatusCode.InvalidArgument,
            $"The object is not valid, details: {string.Join(", ", validationResult.Errors.Select(x => $"{x.PropertyName}: {x.ErrorMessage}"))}"));
}