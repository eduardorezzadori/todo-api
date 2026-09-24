using Microsoft.EntityFrameworkCore;
using TodoApi.Data;
using TodoApi.DTOs;
using TodoApi.Models;

namespace TodoApi.Repository;

public class TodoRepository
{
    private readonly TodoContext _context;

    public TodoRepository(TodoContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TodoItemDTO>> GetAllAsync()
    {
        return await _context.TodoItems
            .Select(t => new TodoItemDTO
            {
                Id = t.Id,
                Name = t.Name,
                IsComplete = t.IsComplete,
                UserId = t.UserId
            })
            .ToListAsync();
    }

    public async Task<TodoItemDTO?> GetByIdAsync(Guid? id)
    {
        if (id == null) return null;
        var t = await _context.TodoItems.FindAsync(id);
        if (t == null) return null;
        return new TodoItemDTO
        {
            Id = t.Id,
            Name = t.Name,
            IsComplete = t.IsComplete,
            UserId = t.UserId
        };
    }

    public async Task<TodoItemDTO> AddAsync(TodoItemDTO dto)
    {
        var entity = new TodoItem
        {
            Id = dto.Id == Guid.Empty ? Guid.NewGuid() : dto.Id,
            Name = dto.Name,
            IsComplete = dto.IsComplete,
            UserId = dto.UserId
        };
        _context.TodoItems.Add(entity);
        await _context.SaveChangesAsync();
        // reflect any DB-generated values back to DTO
        dto.Id = entity.Id;
        return dto;
    }

    public async Task<bool> UpdateAsync(Guid? id, UpdateTodoItemDTO dto)
    {
        if (!id.HasValue) throw new ArgumentNullException(nameof(id));

        var entity = await _context.TodoItems.FindAsync(id.Value);

        if (entity == null) throw new KeyNotFoundException($"Todo item with ID {id} not found.");

        entity.Name = dto.Name ?? entity.Name;
        entity.IsComplete = dto.IsComplete ?? entity.IsComplete;
        entity.UserId = dto.UserId;

        _context.TodoItems.Update(entity);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> RemoveAsync(TodoItemDTO dto)
    {
        var entity = await _context.TodoItems.FindAsync(dto.Id);
        if (entity == null) return false;
        _context.TodoItems.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }
}
