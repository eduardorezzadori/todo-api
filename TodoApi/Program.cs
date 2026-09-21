using Microsoft.EntityFrameworkCore;
using TodoApi.Models;
using TodoApi.Repository;
using TodoApi.Services;
using TodoApi.Validators;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("TodoContext") ?? throw new InvalidOperationException("Connection string 'TodoContext' not found.");

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Contexto de database
builder.Services.AddDbContext<TodoContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddScoped<TodoService>();
builder.Services.AddScoped<TodoRepository>();
builder.Services.AddScoped<CreateTodoItemUseCase>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
