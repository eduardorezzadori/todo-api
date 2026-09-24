using Microsoft.EntityFrameworkCore;
using TodoApi.Data;
using TodoApi.Entities;

namespace TodoApi.Repository;

public class UserRepository
{
    private readonly TodoContext _context;

    public UserRepository(TodoContext context)
    {
        _context = context;
    }

    public async Task<User> AddAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        return user;
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
        return await _context.Users
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<List<User>> GetAllAsync()
    {
        return await _context.Users.ToListAsync();
    }
}