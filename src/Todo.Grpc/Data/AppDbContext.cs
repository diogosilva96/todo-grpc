using Microsoft.EntityFrameworkCore;
using Todo.Grpc.Models;

namespace Todo.Grpc.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    { }

    public DbSet<TodoItem> TodoItems => Set<TodoItem>();
}