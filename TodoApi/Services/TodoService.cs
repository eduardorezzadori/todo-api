using System.ComponentModel.DataAnnotations;
using TodoApi.Models;
using TodoApi.Repository;
using TodoApi.Validators;

namespace TodoApi.Services;

public class TodoService
{
    private readonly TodoRepository _repository;
    private readonly CreateTodoItemUseCase _validator;

    public TodoService(TodoRepository repository, CreateTodoItemUseCase validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task<IEnumerable<TodoItemDTO>> GetAllTodoItemsAsync()
    {
        //return await _repository.GetAllAsync();
        var todos = await _repository.GetAllAsync();
        var todoDTOs = todos.Select(todo => new TodoItemDTO
        {
            Id = todo.Id,
            Name = todo.Name,
            IsComplete = todo.IsComplete
        });
        return todoDTOs;
    }

    public async Task<TodoItemDTO?> GetTodoItemAsync(long id)
    {
        var todo = await _repository.GetByIdAsync(id);
        TodoItemDTO? todoDTO = null;

        if (todo != null)
        {
            todoDTO = new TodoItemDTO
            {
                Id = todo.Id,
                Name = todo.Name,
                IsComplete = todo.IsComplete
            };
        }

        return todoDTO;
    }

    public async Task<bool> DeleteAsync(long? id)
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
}
