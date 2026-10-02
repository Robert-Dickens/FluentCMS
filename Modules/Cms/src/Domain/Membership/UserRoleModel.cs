using System;

namespace FluentCMS.Domain.Membership;

public class UserRoleModel : SiteAssociatedEntityModel
{
    public Guid UserId { get; set; }
    public Guid RoleId { get; set; }

    public UserRoleModel() : base(Guid.NewGuid())
    {

    }

}
