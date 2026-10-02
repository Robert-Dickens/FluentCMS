using FluentCMS.Membership;
using System;

namespace FluentCMS.Domain.Membership;

public class RoleModel : SiteAssociatedEntityModel
{
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public RoleTypes Type { get; set; }

    public RoleModel() : base(Guid.NewGuid())
    {

    }

}
