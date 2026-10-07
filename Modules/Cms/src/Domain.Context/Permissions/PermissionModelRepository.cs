using ByteLabs.Foundations.Domain.Repositories.EntityFrameworkCore;
using ByteLabs.Foundations.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCMS.Domain.Permissions;

public class PermissionModelRepository : EfCoreRepository<IFluentCMSDbContext, PermissionModel, Guid>, IPermissionModelRepository
{
    public PermissionModelRepository(IDbContextProvider<IFluentCMSDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public async Task<IEnumerable<PermissionModel>> Set(Guid siteId, Guid entityId, string entityTypeName, string action, IEnumerable<Guid> roleIds, CancellationToken cancellationToken = default)
    {
        var existPermissions = await GetListAsync(x => x.EntityId == entityId && x.EntityType == entityTypeName && x.Action == action);

        await DeleteManyAsync(existPermissions.Select(x => x.Id), cancellationToken: cancellationToken);

        var permissions = roleIds.Select(x => new PermissionModel
        {
            EntityType = entityTypeName,
            Action = action,
            EntityId = entityId,
            RoleId = x,
            SiteId = siteId
        });

        if (!permissions.Any())
            return [];

        var results = new List<PermissionModel>();

        foreach (var permission in permissions)
        {
            results.Add(await InsertAsync(permission, cancellationToken: cancellationToken));
        }

        return results;
    }

    public async Task<IEnumerable<PermissionModel>> Get(Guid siteId, Guid entityId, string entityTypeName, string action, CancellationToken cancellationToken)
    {
        return await GetListAsync(x => x.EntityId == entityId && x.EntityType == entityTypeName && x.Action == action);
    }

    public async Task<IEnumerable<PermissionModel>> GetAllForSite(Guid siteId, CancellationToken cancellationToken = default) => await GetListAsync(x => x.SiteId == siteId, cancellationToken: cancellationToken);
}
