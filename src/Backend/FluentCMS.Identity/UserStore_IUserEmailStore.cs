using Microsoft.AspNetCore.Identity;

namespace FluentCMS.Identity;

public partial class UserStore : IUserEmailStore<UserModel>
{
    public Task<string?> GetEmailAsync(UserModel user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.Email);
    }

    public Task<bool> GetEmailConfirmedAsync(UserModel user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.EmailConfirmed);
    }

    public async Task<UserModel?> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return await repository.FindByEmail(normalizedEmail, cancellationToken);
    }

    public Task<string?> GetNormalizedEmailAsync(UserModel user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.NormalizedEmail);
    }

    public Task SetEmailConfirmedAsync(UserModel user, bool confirmed, CancellationToken cancellationToken)
    {
        user.EmailConfirmed = confirmed;
        return Task.CompletedTask;
    }

    public Task SetNormalizedEmailAsync(UserModel user, string? normalizedEmail, CancellationToken cancellationToken)
    {
        user.NormalizedEmail = normalizedEmail;
        return Task.CompletedTask;
    }

    public Task SetEmailAsync(UserModel user, string? email, CancellationToken cancellationToken)
    {
        user.Email = email;
        return Task.CompletedTask;
    }
}
