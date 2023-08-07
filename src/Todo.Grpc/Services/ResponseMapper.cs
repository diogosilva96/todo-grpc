using Todo.Grpc.Models;

namespace Todo.Grpc.Services;

public static class ResponseMapper
{
    public static UpdateTodoResponse MapToUpdateTodoResponse(TodoItem todo) => new()
    {
        Id = todo.Id
    };

    public static DeleteTodoResponse MapToDeleteTodoResponse(TodoItem todo) => new()
    {
        Id = todo.Id
    };

    public static GetTodoResponse MapToGetTodoResponse(TodoItem todo) => new()
    {
        Id = todo.Id,
        Description = todo.Description,
        Status = todo.TodoStatus,
        Title = todo.Title
    };

    public static ListTodoResponse MapToListTodoResponse(IEnumerable<TodoItem> todos)
    {
        var listResponse = new ListTodoResponse();

        foreach (var todo in todos)
        {
            listResponse.ToDo.Add(MapToGetTodoResponse(todo));
        }

        return listResponse;
    }

    public static CreateTodoResponse MapToCreateTodoResponse(TodoItem todo) => new()
    {
        Id = todo.Id
    };
}