using Microsoft.EntityFrameworkCore;
using TodoApi.Entities;

namespace TodoApi.Data;

public class TodoContext : DbContext
{
    public TodoContext(DbContextOptions<TodoContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<TodoItem> TodoItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<TodoItem>()
            .Property(todo => todo.Id)
            .ValueGeneratedNever();

        modelBuilder.Entity<User>()
           .Property(user => user.Id)
           .ValueGeneratedNever();

        modelBuilder.Entity<User>()
            .HasMany(user => user.TodoItems)
            .WithOne(todo => todo.User)
            .HasForeignKey(todo => todo.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}