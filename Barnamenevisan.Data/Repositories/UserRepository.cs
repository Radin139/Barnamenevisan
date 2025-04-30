using Barnamenevisan.Data.Context;
using Barnamenevisan.Domain.Interfaces;
using Barnamenevisan.Domain.Models.Auth;
using Microsoft.EntityFrameworkCore;

namespace Barnamenevisan.Data.Repositories;

public class UserRepository(BarnamenevisanDbContext context) : Repository<User>(context), IUserRepository
{
    public async Task<bool> UsernameExistsAsync(string username)
    {
        List<User> users = await GetAllAsync();
        return users.Any(user => user.Username == username);
    }

    public async Task<User?> FindUserByUsernameAndPasswordAsync(string username, string password)
    {
        return await context.Users.FirstOrDefaultAsync(user => user.Password == password && user.Username == username);
    }

    public async Task<User?> FindUserByUsernameAsync(string username)
    {
        return await context.Users.FirstOrDefaultAsync(user => user.Username == username);
    }
}