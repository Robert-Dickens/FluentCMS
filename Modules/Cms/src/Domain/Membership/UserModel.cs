using ByteLabs.Foundations.Auditing;
using ByteLabs.Foundations.Data;
using ByteLabs.Foundations.Domain.Entities;
using ByteLabs.Foundations.MultiTenancy;
using ByteLabs.PlatformServices.Users.Domain;
using FluentCMS.Membership;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace FluentCMS.Domain.Membership;

public class UserModel : IdentityUser<Guid>, IFullAuditedObject, IEntity<Guid>, IMultiTenant, IUser
{
    public Guid? TenantId { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public bool IsActive { get; set; } = true;
    public int LoginCount { get; set; }
    public DateTime? LoginAt { get; set; }
    public DateTime? PasswordChangedAt { get; set; }
    public string? PasswordChangedBy { get; set; }
    public bool Enabled { get; set; } = true;
    public List<IdentityUserLogin<Guid>> Logins { get; set; } = [];
    public List<IdentityUserToken<Guid>> Tokens { get; set; } = [];
    public List<UserTwoFactorRecoveryCode> RecoveryCodes { get; set; } = [];
    public string? AuthenticatorKey { get; set; }
    public List<IdentityUserClaim<Guid>> Claims { get; set; } = [];

    public Guid? CreatorId { get; set; } = default!;
    public DateTime CreationTime { get; set; }
    public Guid? LastModifierId { get; set; }
    public DateTime? LastModificationTime { get; set; }
    public Guid? DeleterId { get; set; }
    public DateTime? DeletionTime { get; set; }
    public bool IsDeleted { get; set; }

    public UserModel()
    {
        Id = Guid.NewGuid();
    }


    public ExtraPropertyDictionary ExtraProperties { get; protected set; } = new ExtraPropertyDictionary();

    public object?[] GetKeys()
    {
        return new object[1] { Id };
    }

    public override string ToString()
    {
        return $"[ENTITY: {GetType().Name}] Id = {Id}";
    }
}
