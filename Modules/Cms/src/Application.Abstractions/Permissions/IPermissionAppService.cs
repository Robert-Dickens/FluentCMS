using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCMS.Permissions;

public interface IPermissionAppService : ICrudAppService<Permission, Guid, GetPermissionRequest, PermissionCreateRequest, PermissionUpdateRequest>
{
}

public interface IPermissionService : IApplicationService
{
    Task<IEnumerable<Permission>> Set(Guid siteId, SitePermissionAction action, IEnumerable<Guid> roleIds, CancellationToken cancellationToken = default);
    Task<IEnumerable<Permission>> Set(Guid siteId, Guid pageId, PagePermissionAction action, IEnumerable<Guid> roleIds, CancellationToken cancellationToken = default);
    Task<IEnumerable<Permission>> Get(Guid siteId, SitePermissionAction action, CancellationToken cancellationToken = default);
    Task<IEnumerable<Permission>> Get(Guid siteId, Guid pageId, PagePermissionAction action, CancellationToken cancellationToken = default);
}


public class Permission : EntityDto<Guid>
{
    public Guid SiteId { get; set; }
    public Guid EntityId { get; set; }
    public string EntityType { get; set; } = default!;
    public string Action { get; set; } = default!;
    public Guid RoleId { get; set; }
}

public class GetPermissionRequest : FilteredAndPagedAndSortedResultRequestDto
{
}

public class PermissionCreateRequest : EntityDto
{
    public Guid SiteId { get; set; }
    public Guid EntityId { get; set; }
    public string EntityType { get; set; } = default!;
    public string Action { get; set; } = default!;
    public Guid RoleId { get; set; }
}

public class PermissionUpdateRequest : EntityDto
{
    public Guid SiteId { get; set; }
    public Guid EntityId { get; set; }
    public string EntityType { get; set; } = default!;
    public string Action { get; set; } = default!;
    public Guid RoleId { get; set; }
}
