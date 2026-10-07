using ByteLabs.Foundations.Auditing;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCMS.Membership;

public interface IUserAppService : ICrudAppService<User, Guid, GetUserRequest, UserCreateRequest, UserUpdateRequest>
{
    Task<bool> ChangePassword(Guid id, ChangePasswordRequest request, CancellationToken cancellationToken = default);
}

public interface IUserService : IApplicationService
{
    Task<IEnumerable<User>> GetAll(CancellationToken cancellationToken = default);
    Task<User> GetById(Guid id, CancellationToken cancellationToken = default);
    Task<User> Update(User user, CancellationToken cancellationToken = default);
    Task<User> Create(User user, string password, CancellationToken cancellationToken = default);
    Task<bool> Delete(Guid id, CancellationToken cancellationToken = default);
    Task<bool> ChangePassword(User user, string newPassword, CancellationToken cancellationToken = default);
}

public class User : IdentityUser<Guid>, IFullAuditedObject
{
    public DateTime? LoginAt { get; set; }
    public int LoginCount { get; set; } = 0;
    public DateTime? PasswordChangedAt { get; set; }
    public Guid? PasswordChangedBy { get; set; }
    public bool Enabled { get; set; } = true;
    public List<IdentityUserLogin<Guid>> Logins { get; set; } = [];
    public List<IdentityUserToken<Guid>> Tokens { get; set; } = [];
    public List<UserTwoFactorRecoveryCode> RecoveryCodes { get; set; } = [];
    public string? AuthenticatorKey { get; set; }
    public List<IdentityUserClaim<Guid>> Claims { get; set; } = [];
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime CreationTime { get; set; }
    public Guid? CreatorId { get; set; }
    public Guid? LastModifierId { get; set; }
    public DateTime? LastModificationTime { get; set; }
    public Guid? DeleterId { get; set; }
    public DateTime? DeletionTime { get; set; }
    public bool IsDeleted { get; set; }

    public User()
    {
    }

    public User(string userName) : base(userName)
    {
    }
}

public class UserDetailResponse : FullAuditedEntityDto
{
    public string Username { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string? PhoneNumber { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime? LoginAt { get; set; }
    public int LoginCount { get; set; }
    public bool Enabled { get; set; }
    public bool EmailConfirmed { get; set; }
    public bool PhoneConfirmed { get; set; }
    public bool Locked { get; set; }
    public bool IsSuperAdmin { get; set; }
}


public class UserCreateRequest : EntityDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = default!;

    [Required]
    public string UserName { get; set; } = default!;

    [Required]
    public string Password { get; set; } = default!;

    public string? PhoneNumber { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }

    [Required]
    public bool Enabled { get; set; }

    public List<IdentityUserClaim<Guid>> Claims { get; set; } = [];
}

public class UserUpdateRequest : EntityDto
{
    [Required]
    [EmailAddress]
    public required string Email { get; set; }

    public string? PhoneNumber { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }

    [Required]
    public bool Enabled { get; set; }

}

public class GetUserRequest : FilteredAndPagedAndSortedResultRequestDto
{

}

public class UserSetPasswordRequest
{
    [Required]
    public Guid UserId { get; set; }

    [Required]
    public string Password { get; set; } = default!;
}

public class UserSendPasswordResetTokenRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = default!;
}

public class ChangePasswordRequest : EntityDto
{
    [Required]
    public string OldPassword { get; set; } = default!;
    [Required]
    public string NewPassword { get; set; } = default!;
}

public class UserChangePasswordRequest: ChangePasswordRequest
{
    [Required]
    public Guid UserId { get; set; }
}


public class UserValidatePasswordResetTokenRequest
{
    [Required]
    public string Token { get; set; } = default!;

    [Required]
    public string Email { get; set; } = default!;

    [Required]
    public string NewPassword { get; set; } = default!;

    [Required]
    [Compare(nameof(NewPassword))]
    public string NewPasswordConfirm { get; set; } = default!;
}

public class UserRegisterRequest
{
    [Required]
    public string Email { get; set; } = default!;

    [Required]
    public string Username { get; set; } = default!;

    [Required]
    public string Password { get; set; } = default!;

    [Required]
    [Compare("Password")]
    public string ConfirmPassword { get; set; } = default!;
}

public class UserLoginResponse:EntityDto
{
    public Guid UserId { get; set; }
    public string Token { get; set; } = default!;
    public string UserName { get; set; } = default!;
    public string Email { get; set; } = default!;
}

public class UserLoginRequest : EntityDto
{
    [Required]
    public string Username { get; set; } = default!;

    [Required]
    public string Password { get; set; } = default!;

    public bool RememberMe { get; set; }
}
