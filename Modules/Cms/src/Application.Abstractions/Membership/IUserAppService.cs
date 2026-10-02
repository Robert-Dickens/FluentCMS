using ByteLabs.Foundations.Auditing;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCMS.Membership;

public interface IUserAppService : ICrudAppService<User, Guid, GetUserRequest, UserCreateRequest, UserUpdateRequest>
{
    Task<bool> ChangePassword(Guid id, ChangePasswordRequest request, CancellationToken cancellationToken = default);
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

public class UserCreateRequest : EntityDto
{
    public bool Enabled { get; set; } = default!;
    public List<IdentityUserClaim<Guid>> Claims { get; set; } = [];
    public string? FirstName { get; set; }
    public string? LastName { get; set; }

    public string UserName { get; set; } = default!;
    public string Password { get; set; } = default!;
}

public class UserUpdateRequest : EntityDto
{
    public bool Enabled { get; set; } = default!;
    public List<IdentityUserToken<Guid>> Tokens { get; set; } = [];
    public List<UserTwoFactorRecoveryCode> RecoveryCodes { get; set; } = [];
    public string? AuthenticatorKey { get; set; }
    public List<IdentityUserClaim<Guid>> Claims { get; set; } = [];
    public string? FirstName { get; set; }
    public string? LastName { get; set; }

}

public class GetUserRequest : FilteredAndPagedAndSortedResultRequestDto
{

}

public class ChangePasswordRequest : EntityDto
{
    public string OldPassword { get; set; } = default!;
    public string NewPassword { get; set; } = default!;
}
