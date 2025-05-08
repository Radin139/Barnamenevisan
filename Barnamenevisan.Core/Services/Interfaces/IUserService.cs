using Barnamenevisan.Domain.Models.Auth;
using Barnamenevisan.Domain.ViewModels.Admin.User;
using Barnamenevisan.Domain.ViewModels.Auth;

namespace Barnamenevisan.Core.Services.Interfaces;

public interface IUserService
{
    Task<RegisterResult> RegisterUserAsync(RegisterViewModel viewModel);
    Task<LoginResult> LoginUserAsync(LoginViewModel viewModel);
    Task<User?> GetUserByUsernameAsync(string username);
    Task<User?> GetUserByEmailAsync(string email);
    Task<ForgotPasswordResult> GetConfirmationCodeForUserAsync(ForgotPasswordViewModel viewModel);
    Task<ResetPasswordResult> ResetPasswordAsync(ResetPasswordViewModel viewModel);
    Task<List<UserAdminViewModel>> GetUsersForAdminAsync();
    Task<CreateUserResult> CreateUserAsync(UserCreateViewModel viewModel);
    Task<bool> DeleteUserAsync(int id);
    Task<bool> RestoreUserAsync(int id);
    Task<UserEditViewModel?> GetUserForEditAsync(int id);
    Task EditUserAsync(UserEditViewModel viewModel);
    Task<UserDeletePermanentlyViewModel?> GetUserForPermanentlyDeleteAsync(int id); 
    Task<bool> DeleteUserPermanentlyAsync(int id);
}