using ByteLabs.Foundations.Domain.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCMS.Domain.Membership;

public interface IRoleModelRepository : IRepository<RoleModel, Guid>, ISiteAssociatedRepository<RoleModel>
{
    Task<RoleModel?> DeleteWithResultAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken));

}
