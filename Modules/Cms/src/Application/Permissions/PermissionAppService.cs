using FluentCMS.Domain.Permissions;
using FluentCMS.Permissions;
using Microsoft.AspNetCore.Authorization;
using System;

namespace FluentCMS.Services.Permissions;

[Authorize(FluentCMSPermissions.SiteContent.Permissions.Default)]
public class PermissionAppService : FluentCMSCrudAppService<PermissionModel, Permission, Guid, GetPermissionRequest, PermissionCreateRequest, PermissionUpdateRequest>
{
    public PermissionAppService(IPermissionModelRepository repository) : base(repository)
    {
        CreatePolicyName = FluentCMSPermissions.SiteContent.Permissions.Create;
        UpdatePolicyName = FluentCMSPermissions.SiteContent.Permissions.Edit;
        DeletePolicyName = FluentCMSPermissions.SiteContent.Permissions.Delete;

    }
}
