using Barnamenevisan.Core.Senders;
using Barnamenevisan.Core.Services.Implementations;
using Barnamenevisan.Core.Services.Interfaces;
using Barnamenevisan.Data.Repositories;
using Barnamenevisan.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Barnamenevisan.IoC;

public static class Container
{
    #region AddServices

    public static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserService, UserService>();
        services.AddSingleton<EmailSender>();
    }

    #endregion
}