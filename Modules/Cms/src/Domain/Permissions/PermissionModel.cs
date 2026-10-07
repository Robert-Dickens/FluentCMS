using FluentCMS.Domain.Membership;
using System;

namespace FluentCMS.Domain.Permissions;

public class PermissionModel : SiteAssociatedEntityModel
{
    public Guid EntityId { get; set; }
    public string EntityType { get; set; } = default!;
    public string Action { get; set; } = default!; // comma separated list of actions
    public Guid RoleId { get; set; }
    public RoleModel Role { get; set; } = default!; // navigation property

    public PermissionModel() : base(Guid.NewGuid())
    {

    }


    public PermissionModel(Guid id, Guid siteId) : base(id)
    {
        SiteId = siteId;
    }
}
