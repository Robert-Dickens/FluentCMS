using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCMS.Membership;

public interface IRoleAppService : ICrudAppService<Role, Guid, GetRoleRequest, RoleCreateRequest, RoleUpdateRequest>
{
}

public interface IRoleService : IApplicationService
{
    Task<IEnumerable<Role>> GetAllForSite(Guid siteId, CancellationToken cancellationToken = default);
    Task<Role> Create(Role role, CancellationToken cancellationToken = default);
    Task<Role> Update(Role role, CancellationToken cancellationToken = default);
    Task<Role> Delete(Guid roleId, CancellationToken cancellationToken = default);
    Task<Role> GetById(Guid roleId, CancellationToken cancellationToken = default);
}


public class Role : ExtensibleFullAuditedEntityDto<Guid>
{
    public Guid SiteId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public RoleTypes Type { get; set; }
}

public class RoleDetailResponse : Role
{
    public int UsersCount { get; set; }
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
