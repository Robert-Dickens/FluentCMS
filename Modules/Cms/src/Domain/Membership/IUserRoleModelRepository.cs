using ByteLabs.Foundations.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCMS.Domain.Membership;

public interface IUserRoleModelRepository : IRepository<UserRoleModel, Guid>, ISiteAssociatedRepository<UserRoleModel>
{
    Task<IEnumerable<UserRoleModel>> GetUserRoles(Guid userId, Guid siteId, CancellationToken cancellationToken = default);
    Task<IEnumerable<UserRoleModel>> GetByRoleId(Guid roleId, CancellationToken cancellationToken = default);
    Task<IEnumerable<UserRoleModel>> GetByUserId(Guid userId, CancellationToken cancellationToken = default);

    Task<UserRoleModel?> DeleteWithResultAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken));

    Task<IEnumerable<UserRoleModel>> DeleteManyWithResultAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default(CancellationToken));

}