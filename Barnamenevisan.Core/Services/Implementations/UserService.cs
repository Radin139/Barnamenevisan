using System.Security.Claims;
using Barnamenevisan.Core.Extensions;
using Barnamenevisan.Core.Services.Interfaces;
using Barnamenevisan.Domain.Interfaces;
using Barnamenevisan.Domain.Models.Auth;
using Barnamenevisan.Domain.ViewModels.Auth;

namespace Barnamenevisan.Core.Services.Implementations;

public class UserService(IUserRepository userRepository):IUserService
{
    public async Task<RegisterResult> RegisterUserAsync(RegisterViewModel viewModel)
    {
        bool usernameExists = await userRepository.UsernameExistsAsync(viewModel.Username);
        
        if (usernameExists)
        {
            return RegisterResult.UsernameExists;
        }

        User user = new User()
        {
            Email = viewModel.Email,
            IsAdmin = false,
            IsDeleted = false,
            Password = viewModel.Password.EncodeWithMD5(),
            RegisterDate = DateTime.Now,
            Username = viewModel.Username
        };

        await userRepository.InsertAsync(user);
        await userRepository.SaveAsync();
        
        return RegisterResult.Success;
    }

    public async Task<LoginResult> LoginUserAsync(LoginViewModel viewModel)
    {
        User? user = await userRepository.FindUserByUsernameAndPasswordAsync(viewModel.Username, viewModel.Password.EncodeWithMD5());
        if (user == null)
        {
            return LoginResult.UserNotFound;
        }
        
        return LoginResult.Success;
    }

    public async Task<User?> GetUserByUsernameAsync(string username)
    {
        return await userRepository.FindUserByUsernameAsync(username);
    }
}