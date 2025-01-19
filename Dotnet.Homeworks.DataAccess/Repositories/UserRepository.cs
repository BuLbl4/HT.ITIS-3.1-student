using Dotnet.Homeworks.Data.DatabaseContext;
using Dotnet.Homeworks.Domain.Abstractions.Repositories;
using Dotnet.Homeworks.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dotnet.Homeworks.DataAccess.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public Task<IQueryable<User>> GetUsersAsync(CancellationToken cancellationToken)
    {
        return Task.FromResult(_context.Users.AsQueryable());
    }

    public async Task<User?> GetUserByGuidAsync(Guid guid, CancellationToken cancellationToken)
    {
        return await _context.Users.FirstOrDefaultAsync(x => x.Id == guid, cancellationToken);
    }

    public async Task DeleteUserByGuidAsync(Guid guid, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FindAsync(guid, cancellationToken);
        
        if (user != null)
        {
            _context.Users.Remove(user);
        }
    }

    public Task UpdateUserAsync(User user, CancellationToken cancellationToken)
    {
        _context.Users.Update(user);
        return Task.CompletedTask;
    }

    public async Task<Guid> InsertUserAsync(User user, CancellationToken cancellationToken)
    {
        var entry = await _context.Users.AddAsync(user, cancellationToken);
        return entry.Entity.Id;
    }
}