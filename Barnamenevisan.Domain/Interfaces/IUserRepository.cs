using Barnamenevisan.Domain.Models.Auth;

namespace Barnamenevisan.Domain.Interfaces;

public interface IUserRepository:IRepository<User>
{
    Task<bool> UsernameAndEmailExistsAsync(string username, string email);
    Task<User?> FindUserByUsernameAndPasswordAsync(string username, string password);
    Task<User?> FindUserByUsernameAsync(string username);
    Task<User?> GetUserByEmailAsync(string email);
    Task<User?> GetUserByConfirmationCodeAsync(string confirmationCode);
}