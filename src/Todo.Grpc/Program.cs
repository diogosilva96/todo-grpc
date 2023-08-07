using Microsoft.EntityFrameworkCore;
using Todo.Grpc.Data;
using Todo.Grpc.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite("Data Source=TodoDatabase"));

builder.Services.AddGrpc();

var app = builder.Build();

app.MapGrpcService<GreeterService>();
app.MapGrpcService<TodoService>();

app.MapGet("/", () =>"Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();