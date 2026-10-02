using FluentCMS.Domain.Membership;
using FluentCMS.Membership;
using FluentCMS.Permissions;
using Microsoft.AspNetCore.Authorization;
using System;

namespace FluentCMS.Services.Membership;

[Authorize(FluentCMSPermissions.MemberShips.Roles.Default)]
public class RoleAppService : FluentCMSCrudAppService<RoleModel, Role, Guid, GetRoleRequest, RoleCreateRequest, RoleUpdateRequest>, IRoleAppService
{
    public RoleAppService(IRoleModelRepository repository) : base(repository)
    {
        CreatePolicyName = FluentCMSPermissions.MemberShips.Roles.Create;
        UpdatePolicyName = FluentCMSPermissions.MemberShips.Roles.Edit;
        DeletePolicyName = FluentCMSPermissions.MemberShips.Roles.Delete;
    }
}
