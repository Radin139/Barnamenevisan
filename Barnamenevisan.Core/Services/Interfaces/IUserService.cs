using Barnamenevisan.Domain.Models.Auth;
using Barnamenevisan.Domain.ViewModels.Auth;

namespace Barnamenevisan.Core.Services.Interfaces;

public interface IUserService
{
    Task<RegisterResult> RegisterUserAsync(RegisterViewModel viewModel);
    Task<LoginResult> LoginUserAsync(LoginViewModel viewModel);
    Task<User?> GetUserByUsernameAsync(string username);
}