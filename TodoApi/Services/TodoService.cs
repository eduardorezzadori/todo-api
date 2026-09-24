using TodoApi.DTOs;
using TodoApi.Models;
using TodoApi.Repository;

namespace TodoApi.Services;

public class TodoService
{
    private readonly TodoRepository _repository;

    public TodoService(TodoRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<TodoItemDTO>> GetAllTodoItemsAsync()
    {
        
        var todos = await _repository.GetAllAsync();
        var todoDTOs = todos.Select(todo => new TodoItemDTO
        {
            Id = todo.Id,
            Name = todo.Name,
            IsComplete = todo.IsComplete,
            UserId = todo.UserId
        });
        return todoDTOs;
    }

    public async Task<TodoItemDTO?> GetTodoItemAsync(Guid? id)
    {
        var todo = await _repository.GetByIdAsync(id);
        TodoItemDTO? todoDTO = null;

        if (todo != null)
        {
            todoDTO = new TodoItemDTO
            {
                Id = todo.Id,
                Name = todo.Name,
                IsComplete = todo.IsComplete,
                UserId = todo.UserId
            };
        }

        return todoDTO;
    }

    public async Task<bool> DeleteAsync(Guid? id)
    {
        var todoitem = await _repository.GetByIdAsync(id);
        if (todoitem == null)
        {
            return false;
        }

        var deleteResult = await _repository.RemoveAsync(todoitem);

        return deleteResult;
    }

    public async Task<TodoItemDTO> CreateAsync(TodoItemDTO todoitem)
    {
        return await _repository.AddAsync(todoitem);
    }

    public async Task<TodoItemDTO?> UpdateAsync(Guid? id, UpdateTodoItemDTO todoitem)
    {
        var existingTodo = await _repository.GetByIdAsync(id);
        if (existingTodo == null)
        {
            return null;
        }
        
        var updatedTodo = await _repository.UpdateAsync(id, todoitem);
        return existingTodo;
    }
}
