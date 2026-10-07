using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCMS.Membership;

public interface IAccountService : IApplicationService
{
    Task<User> Register(User user, string password, CancellationToken cancellationToken = default);
    Task<User> Get(CancellationToken cancellationToken = default);
    Task<User> Update(User user, CancellationToken cancellationToken = default);
    Task<User> Authenticate(string username, string password, CancellationToken cancellationToken = default);
    Task<User> ChangePassword(Guid id, string oldPassword, string newPassword, CancellationToken cancellationToken = default);
    Task<bool> ChangePasswordByResetToken(string email, string token, string newPassword, CancellationToken cancellationToken = default);
    Task<UserToken> GetToken(User user, CancellationToken cancellationToken = default);
    Task<bool> SendPasswordResetToken(string email, CancellationToken cancellationToken = default);
    Task<bool> ValidatePassword(string password, CancellationToken cancellationToken = default);
    Task<bool> ValidateUserName(string username, CancellationToken cancellationToken = default);
}


public class AccountChangePasswordRequest : ChangePasswordRequest
{
    [Required]
    [Compare(nameof(NewPassword))]
    public string NewPasswordConfirm { get; set; } = default!;
}

public class AccountUpdateRequest:EntityDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = default!;

    public string? PhoneNumber { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
}
