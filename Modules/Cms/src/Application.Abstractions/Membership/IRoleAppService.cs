using System;

namespace FluentCMS.Membership;

public interface IRoleAppService : ICrudAppService<Role, Guid, GetRoleRequest, RoleCreateRequest, RoleUpdateRequest>
{
}

public class Role : ExtensibleFullAuditedEntityDto<Guid>
{
    public Guid SiteId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public RoleTypes Type { get; set; }
}

public class GetRoleRequest : FilteredAndPagedAndSortedResultRequestDto
{
}

public class RoleCreateRequest : ExtensibleEntityDto
{
    public Guid SiteId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public RoleTypes Type { get; set; }
}

public class RoleUpdateRequest : ExtensibleEntityDto
{
    public Guid SiteId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public RoleTypes Type { get; set; }
}
