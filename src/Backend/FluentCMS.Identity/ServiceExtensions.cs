using FluentCMS;
using FluentCMS.Identity;
using FluentCMS.Services.Models;
using Microsoft.AspNetCore.Identity;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceExtensions
{

    public static IdentityBuilder AddIdentity(this IServiceCollection services)
    {
        services.AddOptions<JwtOptions>()
            .BindConfiguration("JwtOptions");

        var builder = services.AddIdentityCore<UserModel>();

        builder
            .AddUserStore<UserStore>()
            .AddUserManager<UserManager<UserModel>>()
            .AddDefaultTokenProviders()
            .AddTokenProvider<DataProtectorTokenProvider<UserModel>>(ServiceConstants.PASSWORD_RESET_TOKEN_PROVIDER);

        return builder;
    }

}
