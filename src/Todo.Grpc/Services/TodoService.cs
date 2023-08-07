using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using Todo.Grpc.Data;
using Todo.Grpc.Models;
using Todo.Grpc.Validation;

namespace Todo.Grpc.Services;

public class TodoService : Todo.TodoBase
{
    private readonly AppDbContext _dbContext;
    private readonly IGrpcRequestValidator _requestValidator;

    public TodoService(AppDbContext dbContext, IGrpcRequestValidator requestValidator)
    {
        _dbContext = dbContext;
        _requestValidator = requestValidator;
    }

    public override async Task<CreateTodoResponse> CreateTodo(CreateTodoRequest request, ServerCallContext context)
    {
        await _requestValidator.ValidateAndThrowIfInvalidAsync(request, context.CancellationToken);

        var todoItem = new TodoItem
        {
            Title = request.Title,
            Description = request.Description
        };

        await _dbContext.AddAsync(todoItem, context.CancellationToken);
        await _dbContext.SaveChangesAsync(context.CancellationToken);

        return new()
        {
            Id = todoItem.Id
        };
    }

    public override async Task<GetTodoResponse> GetTodo(GetTodoRequest request, ServerCallContext context)
    {
        await _requestValidator.ValidateAndThrowIfInvalidAsync(request, context.CancellationToken);

        var todo = await _dbContext.TodoItems.FirstOrDefaultAsync(t => t.Id == request.Id, context.CancellationToken);
        if (todo == default)
        {
            throw RpcExceptions.NotFound();
        }

        return new()
        {
            Id = todo.Id,
            Description = todo.Description,
            Status = todo.Description,
            Title = todo.Title
        };
    }

    public override async Task<ListTodoResponse> ListTodo(ListTodoRequest request, ServerCallContext context)
    {
        var todos = await _dbContext.TodoItems.ToArrayAsync(context.CancellationToken);
        var listResponse = new ListTodoResponse();

        foreach (var todo in todos)
        {
            listResponse.ToDo.Add(new GetTodoResponse
            {
                Id = todo.Id,
                Description = todo.Description,
                Status = todo.TodoStatus,
                Title = todo.Title
            });
        }

        return listResponse;
    }

    public override async Task<UpdateTodoResponse> UpdateTodo(UpdateTodoRequest request, ServerCallContext context)
    {
        await _requestValidator.ValidateAndThrowIfInvalidAsync(request, context.CancellationToken);

        var todo = await _dbContext.TodoItems.FirstOrDefaultAsync(t => t.Id == request.Id, context.CancellationToken);
        if (todo == default)
        {
            throw RpcExceptions.NotFound();
        }

        todo.Description = request.Description;
        todo.TodoStatus = request.Status;
        todo.Title = request.Title;

        await _dbContext.SaveChangesAsync(context.CancellationToken);
        return new()
        {
            Id = todo.Id
        };
    }

    public override async Task<DeleteTodoResponse> DeleteTodo(DeleteTodoRequest request, ServerCallContext context)
    {
        await _requestValidator.ValidateAndThrowIfInvalidAsync(request, context.CancellationToken);

        var todo = await _dbContext.TodoItems.FirstOrDefaultAsync(t => t.Id == request.Id, context.CancellationToken);
        if (todo == default)
        {
            throw RpcExceptions.NotFound();
        }

        _dbContext.Remove(todo);

        await _dbContext.SaveChangesAsync(context.CancellationToken);

        return new()
        {
            Id = todo.Id
        };
    }
}