using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using Todo.Grpc.Data;
using Todo.Grpc.Models;

namespace Todo.Grpc.Services;

public class TodoService : Todo.TodoBase
{
    private readonly AppDbContext _dbContext;

    public TodoService(AppDbContext dbContext) => _dbContext = dbContext;

    public override async Task<CreateTodoResponse> CreateTodo(CreateTodoRequest request, ServerCallContext context)
    {
        if (string.IsNullOrWhiteSpace(request.Title) || string.IsNullOrWhiteSpace(request.Description))
        {
            throw new RpcException(new(StatusCode.InvalidArgument, "The object is not valid."));
        }

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
        if (request.Id <= 0)
        {
            throw new RpcException(new(StatusCode.InvalidArgument, "The id is invalid."));
        }

        var todo = await _dbContext.TodoItems.FirstOrDefaultAsync(t => t.Id == request.Id, context.CancellationToken);
        if (todo == default)
        {
            throw new RpcException(new(StatusCode.NotFound, "The object could not be found."));
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
        if (request.Id <= 0 || string.IsNullOrWhiteSpace(request.Title) ||
            string.IsNullOrWhiteSpace(request.Description))
        {
            throw new RpcException(new(StatusCode.InvalidArgument, "The object is not valid."));
        }

        var todo = await _dbContext.TodoItems.FirstOrDefaultAsync(t => t.Id == request.Id, context.CancellationToken);
        if (todo == default)
        {
            throw new RpcException(new(StatusCode.NotFound, "The object could not be found."));
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
        if (request.Id <= 0)
        {
            throw new RpcException(new(StatusCode.InvalidArgument, "The id is invalid."));
        }

        var todo = await _dbContext.TodoItems.FirstOrDefaultAsync(t => t.Id == request.Id, context.CancellationToken);
        if (todo == default)
        {
            throw new RpcException(new(StatusCode.NotFound, "The object could not be found."));
        }

        _dbContext.Remove(todo);

        await _dbContext.SaveChangesAsync(context.CancellationToken);

        return new()
        {
            Id = todo.Id
        };
    }
}