using Microsoft.AspNetCore.Identity;

namespace FluentCMS.Identity;

public partial class UserStore : IUserSecurityStampStore<UserModel>
{
    public Task<string?> GetSecurityStampAsync(UserModel user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.SecurityStamp);
    }

    public Task SetSecurityStampAsync(UserModel user, string stamp, CancellationToken cancellationToken)
    {
        user.SecurityStamp = stamp;
        return Task.CompletedTask;
    }
}
