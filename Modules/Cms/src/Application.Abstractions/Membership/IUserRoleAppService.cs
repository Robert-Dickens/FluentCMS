using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCMS.Membership;

public interface IUserRoleAppService : ICrudAppService<UserRole, Guid, GetUserRoleRequest, UserRoleCreateRequest, UserRoleUpdateRequest>
{
}

public interface IUserRoleService : IApplicationService
{
    Task<IEnumerable<Guid>> GetUserRoleIds(Guid userId, Guid siteId, CancellationToken cancellationToken = default);
    Task<IEnumerable<UserRole>> Update(Guid userId, Guid siteId, IEnumerable<Guid> roleIds, CancellationToken cancellationToken = default);
    Task<IEnumerable<UserRole>> GetAllForSite(Guid siteId, CancellationToken cancellationToken = default);
    Task<IEnumerable<UserRole>> DeleteByRoleId(Guid roleId, CancellationToken cancellationToken = default);
    Task<IEnumerable<UserRole>> DeleteByUserId(Guid userId, CancellationToken cancellationToken = default);
}


public class UserRole : EntityDto<Guid>
{
    public Guid UserId { get; set; }
    public Guid RoleId { get; set; }
}

public class UserRoleDetailResponse : EntityDto<Guid>
{
    public string Username { get; set; } = string.Empty;
    public bool IsAuthenticated { get; set; } = false;
    public bool IsSuperAdmin { get; set; } = false;
    public Guid UserId { get; set; } = Guid.Empty;
    public List<RoleDetailResponse> Roles { get; set; } = [];
}


public class UserRoleCreateRequest : EntityDto
{
    public Guid UserId { get; set; }
    public Guid RoleId { get; set; }
}

public class UserRoleUpdateRequest : EntityDto
{
    [Required]
    public Guid SiteId { get; set; }

    [Required]
    public Guid UserId { get; set; }

    [Required]
    public IEnumerable<Guid> RoleIds { get; set; } = default!;

}

public class GetUserRoleRequest : FilteredAndPagedAndSortedResultRequestDto
{

}

