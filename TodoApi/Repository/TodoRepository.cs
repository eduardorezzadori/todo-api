using Microsoft.EntityFrameworkCore;
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
        return await _context.TodoItems.ToListAsync();
    }

    public async Task<TodoItemDTO?> GetByIdAsync(long id)
    {
        return await _context.TodoItems.FindAsync(id);
    }

    public async Task<TodoItemDTO> AddAsync(TodoItemDTO todoitem)
    {
        _context.TodoItems.Add(todoitem);
        await _context.SaveChangesAsync();
        return todoitem;
    }
}
