using ByteLabs.Foundations.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCMS.Domain.Permissions;

public interface IPermissionModelRepository : IRepository<PermissionModel, Guid>, ISiteAssociatedRepository<PermissionModel>
{
    Task<IEnumerable<PermissionModel>> Set(Guid siteId, Guid entityId, string entityTypeName, string action, IEnumerable<Guid> roleIds, CancellationToken cancellationToken = default);
    Task<IEnumerable<PermissionModel>> Get(Guid siteId, Guid entityId, string entityTypeName, string action, CancellationToken cancellationToken);

}
