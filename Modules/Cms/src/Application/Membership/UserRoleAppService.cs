using FluentCMS.Domain.Membership;
using FluentCMS.Membership;
using FluentCMS.Permissions;
using Microsoft.AspNetCore.Authorization;
using System;

namespace FluentCMS.Services.Membership;

[Authorize(FluentCMSPermissions.MemberShips.UserRoles.Default)]
public class UserRoleAppService : FluentCMSCrudAppService<UserRoleModel, UserRole, Guid, GetUserRoleRequest, UserRoleCreateRequest, UserRoleUpdateRequest>, IUserRoleAppService
{
    public UserRoleAppService(IUserRoleModelRepository repository) : base(repository)
    {
        CreatePolicyName = FluentCMSPermissions.MemberShips.UserRoles.Create;
        UpdatePolicyName = FluentCMSPermissions.MemberShips.UserRoles.Edit;
        DeletePolicyName = FluentCMSPermissions.MemberShips.UserRoles.Delete;
    }
}
