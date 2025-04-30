using Barnamenevisan.Domain.Models.Auth;

namespace Barnamenevisan.Domain.Interfaces;

public interface IUserRepository:IRepository<User>
{
    Task<bool> UsernameExistsAsync(string username);
    Task<User?> FindUserByUsernameAndPasswordAsync(string username, string password);
    Task<User?> FindUserByUsernameAsync(string username);
}