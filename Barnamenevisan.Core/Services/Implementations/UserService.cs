using System.Security.Claims;
using Barnamenevisan.Core.Extensions;
using Barnamenevisan.Core.Generators;
using Barnamenevisan.Core.Senders;
using Barnamenevisan.Core.Services.Interfaces;
using Barnamenevisan.Domain.Interfaces;
using Barnamenevisan.Domain.Models.Auth;
using Barnamenevisan.Domain.ViewModels.Admin.User;
using Barnamenevisan.Domain.ViewModels.Auth;

namespace Barnamenevisan.Core.Services.Implementations;

public class UserService(IUserRepository userRepository, EmailSender emailSender):IUserService
{
    public async Task<RegisterResult> RegisterUserAsync(RegisterViewModel viewModel)
    {
        bool usernameExists = await userRepository.UsernameAndEmailExistsAsync(viewModel.Username.Trim(), viewModel.Email.Trim().ToLower());
        
        if (usernameExists)
        {
            return RegisterResult.UsernameExists;
        }

        User user = new User()
        {
            Email = viewModel.Email.Trim().ToLower(),
            IsAdmin = false,
            IsDeleted = false,
            Password = viewModel.Password.EncodeWithMD5(),
            RegisterDate = DateTime.Now,
            Username = viewModel.Username.Trim()
        };

        await userRepository.InsertAsync(user);
        await userRepository.SaveAsync();
        
        return RegisterResult.Success;
    }

    public async Task<LoginResult> LoginUserAsync(LoginViewModel viewModel)
    {
        User? user = await userRepository.FindUserByUsernameAndPasswordAsync(viewModel.Username.Trim(), viewModel.Password.EncodeWithMD5());
        if (user == null)
        {
            return LoginResult.UserNotFound;
        }
        
        return LoginResult.Success;
    }

    public async Task<User?> GetUserByUsernameAsync(string username)
    {
        return await userRepository.FindUserByUsernameAsync(username.Trim());
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await userRepository.GetUserByEmailAsync(email.Trim().ToLower());
    }

    public async Task<ForgotPasswordResult> GetConfirmationCodeForUserAsync(ForgotPasswordViewModel viewModel)
    {
        string email = viewModel.Email.Trim().ToLower();
        User? user = await userRepository.GetUserByEmailAsync(email);

        if (user == null)
        {
            return ForgotPasswordResult.UserNotFound;
        }

        string code = ConfirmationCodeGenerator.GetCode();
        user.ConfirmationCode = code;
        
        userRepository.Update(user);
        await userRepository.SaveAsync();

        string body = $"<h1>کد تایید: {code}</h1>";
        bool result = emailSender.SendEmail(email, "کد تایید", "کد تایید برای فراموشی رمز عبور", body);

        if (!result)
        {
            return ForgotPasswordResult.EmailNotFound;
        }
        
        return ForgotPasswordResult.Success;
    }

    public async Task<ResetPasswordResult> ResetPasswordAsync(ResetPasswordViewModel viewModel)
    {
        User? user = await userRepository.GetUserByConfirmationCodeAsync(viewModel.ConfirmCode);
        if (user == null)
        {
            return ResetPasswordResult.WrongCode;
        }

        user.ConfirmationCode = null;
        user.Password = viewModel.Password.EncodeWithMD5();
        userRepository.Update(user);
        await userRepository.SaveAsync();

        return ResetPasswordResult.Success;
    }

    public async Task<List<UserAdminViewModel>> GetUsersForAdminAsync()
    {
        var list = await userRepository.GetAllAsync();
        
        var result =  list.Select(user => new UserAdminViewModel()
        {
            Id = user.Id,
            IsAdmin = user.IsAdmin,
            IsDeleted = user.IsDeleted,
            Email = user.Email,
            Username = user.Username,
            RegisterDate = user.RegisterDate,
        }).ToList();

        return result
            .OrderBy(user => user.IsDeleted)
            .ThenBy(user => user.RegisterDate)
            .ToList();
    }

    public async Task<CreateUserResult> CreateUserAsync(UserCreateViewModel viewModel)
    {
        User user = new User()
        {
            Email = viewModel.Email.Trim().ToLower(),
            IsAdmin = viewModel.IsAdmin,
            IsDeleted = false,
            ConfirmationCode = null,
            Username = viewModel.Username.Trim(),
            Password = viewModel.Password.EncodeWithMD5(),
            RegisterDate = DateTime.Now,
        };
        
        await userRepository.InsertAsync(user);
        await userRepository.SaveAsync();
        
        return CreateUserResult.Success;
    }

    public async Task<bool> DeleteUserAsync(int id)
    {
        User? user = await userRepository.GetByIdAsync(id);
        if (user == null)
        {
            return false;
        }
        
        user.IsDeleted = true;
        userRepository.Update(user);
        await userRepository.SaveAsync();
        
        return true;
    }

    public async Task<bool> RestoreUserAsync(int id)
    {
        User? user = await userRepository.GetByIdAsync(id);
        if (user == null)
        {
            return false;
        }
        
        user.IsDeleted = false;
        userRepository.Update(user);
        await userRepository.SaveAsync();
        
        return true;
    }

    public async Task<UserEditViewModel?> GetUserForEditAsync(int id)
    {
        User? user = await userRepository.GetByIdAsync(id);
        if (user == null)
        {
            return null;
        }

        return new UserEditViewModel()
        {
            Id = user.Id,
            IsAdmin = user.IsAdmin,
            Email = user.Email,
            Username = user.Username,
        };
    }

    public async Task EditUserAsync(UserEditViewModel viewModel)
    {
        User user = await userRepository.GetByIdAsync(viewModel.Id);
        user.Username = viewModel.Username;
        user.IsAdmin = viewModel.IsAdmin;
        user.Email = viewModel.Email;
        if (!string.IsNullOrEmpty(viewModel.Password))
        {
            user.Password = viewModel.Password.EncodeWithMD5();
        }
        
        userRepository.Update(user);
        await userRepository.SaveAsync();
    }

    public async Task<UserDeletePermanentlyViewModel?> GetUserForPermanentlyDeleteAsync(int id)
    {
        User? user = await userRepository.GetByIdAsync(id);
        if (user == null)
        {
            return null;
        }

        return new UserDeletePermanentlyViewModel()
        {
            Id = user.Id,
            Username = user.Username,
        };
    }

    public async Task<bool> DeleteUserPermanentlyAsync(int id)
    {
        User? user = await userRepository.GetByIdAsync(id);
        if (user == null)
        {
            return false;
        }
        
        userRepository.Delete(user);
        await userRepository.SaveAsync();
        
        return true;
    }
    
    
}