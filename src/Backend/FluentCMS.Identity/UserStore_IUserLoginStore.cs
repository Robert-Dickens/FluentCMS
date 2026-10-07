using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace FluentCMS.Identity;

public partial class UserStore : IUserLoginStore<UserModel>
{
    public Task AddLoginAsync(UserModel user, UserLoginInfo login, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var userLogin = new IdentityUserLogin<Guid>
        {
            UserId = user.Id,
            LoginProvider = login.LoginProvider,
            ProviderDisplayName = login.ProviderDisplayName,
            ProviderKey = login.ProviderKey
        };

        user.Logins.Add(userLogin);

        return Task.CompletedTask;
    }

    public Task RemoveLoginAsync(UserModel user, string loginProvider, string providerKey, CancellationToken cancellationToken)
    {
        user.Logins.RemoveAll(x => x.LoginProvider == loginProvider && x.ProviderKey == providerKey);
        return Task.CompletedTask;
    }

    public Task<UserModel?> FindByLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return repository.FindByLogin(loginProvider, providerKey, cancellationToken);
    }

    public async Task<IdentityResult> CreateAsync(UserModel user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        await repository.InsertAsync(user, cancellationToken: cancellationToken);
        return IdentityResult.Success;
    }

    public async Task<IdentityResult> DeleteAsync(UserModel user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        await repository.DeleteAsync(user.Id, cancellationToken: cancellationToken);
        return IdentityResult.Success;
    }

    public async Task<UserModel?> FindByIdAsync(string userId, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var id = Guid.Parse(userId);

        return await repository.FindAsync(id, cancellationToken: cancellationToken);
    }

    public Task<UserModel?> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return repository.FindByName(normalizedUserName, cancellationToken);
    }

    public async Task<IdentityResult> UpdateAsync(UserModel user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        await repository.UpdateAsync(user, cancellationToken: cancellationToken);

        return IdentityResult.Success;
    }

    public Task<string?> GetNormalizedUserNameAsync(UserModel user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.NormalizedUserName);
    }

    public Task<string> GetUserIdAsync(UserModel user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user?.Id.ToString() ?? string.Empty);
    }

    public Task<string?> GetUserNameAsync(UserModel user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.UserName);
    }

    public Task<IList<Claim>> GetClaimsAsync(UserModel user, CancellationToken cancellationToken)
    {
        IList<Claim> claims = user.Claims?.Select(x => new Claim(x.ClaimType ?? string.Empty, x.ClaimValue ?? string.Empty))?.ToList() ?? [];
        return Task.FromResult(claims);
    }

    public Task SetNormalizedUserNameAsync(UserModel user, string? normalizedName, CancellationToken cancellationToken)
    {
        user.NormalizedUserName = normalizedName;
        return Task.CompletedTask;
    }

    public Task SetUserNameAsync(UserModel user, string? userName, CancellationToken cancellationToken)
    {
        user.UserName = userName;
        return Task.CompletedTask;
    }

    public Task<IList<UserLoginInfo>> GetLoginsAsync(UserModel user, CancellationToken cancellationToken)
    {
        IList<UserLoginInfo> result = user.Logins?.Select(x => new UserLoginInfo(x.LoginProvider, x.ProviderKey, x.ProviderDisplayName))?.ToList() ?? [];
        return Task.FromResult(result);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
    }

}
