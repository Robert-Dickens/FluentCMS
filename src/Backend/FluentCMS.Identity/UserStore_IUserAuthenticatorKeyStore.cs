using Microsoft.AspNetCore.Identity;

namespace FluentCMS.Identity;

public partial class UserStore : IUserAuthenticatorKeyStore<UserModel>
{
    public Task<string?> GetAuthenticatorKeyAsync(UserModel user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user?.AuthenticatorKey);
    }

    public Task SetAuthenticatorKeyAsync(UserModel user, string key, CancellationToken cancellationToken)
    {
        user.AuthenticatorKey = key;
        return Task.CompletedTask;
    }
}
