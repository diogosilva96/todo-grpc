# todo-grpc
Simple todo application to play around with gRPC and json transcoding in .NET.

The protobuf specs with json transcoding can be found in the folder `src\Todo.Grpc\Protos`.

## Running the project

To run the application do the following (at `src\Todo.Grpc` path):
- Run `dotnet build` to build the project.
- Run `dotnet ef database update` to make sure the entity framework core migrations are applied.
- Run `dotnet run` to start the application.
- You can then perform gRPC (or regular http) requests/calls using a program like postman for example.

