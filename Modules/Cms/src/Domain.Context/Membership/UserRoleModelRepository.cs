using ByteLabs.Foundations;
using ByteLabs.Foundations.Domain.Repositories.EntityFrameworkCore;
using ByteLabs.Foundations.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCMS.Domain.Membership;

public class UserRoleModelRepository : EfCoreRepository<IFluentCMSDbContext, UserRoleModel, Guid>, IUserRoleModelRepository
{
    public UserRoleModelRepository(IDbContextProvider<IFluentCMSDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public async Task<IEnumerable<UserRoleModel>> DeleteManyWithResultAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default)
    {
        await DeleteManyAsync(ids, true, cancellationToken: cancellationToken);

        using (DataFilter.Disable<ISoftDelete>())
        {
            return await GetListAsync(x => ids.Contains(x.Id), cancellationToken: cancellationToken);
        }
    }

    public async Task<UserRoleModel?> DeleteWithResultAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var siteModel = await GetAsync(id, cancellationToken: cancellationToken);

        await DeleteAsync(siteModel, cancellationToken: cancellationToken);

        using (DataFilter.Disable<ISoftDelete>())
        {
            return await FindAsync(x => x.Id == id, cancellationToken: cancellationToken);
        }

    }

    public async Task<IEnumerable<UserRoleModel>> GetAllForSite(Guid siteId, CancellationToken cancellationToken = default) => await GetListAsync(x => x.SiteId == siteId, cancellationToken: cancellationToken);

    public async Task<IEnumerable<UserRoleModel>> GetByRoleId(Guid roleId, CancellationToken cancellationToken = default) => await GetListAsync(x=> x.RoleId == roleId, cancellationToken: cancellationToken);

    public async Task<IEnumerable<UserRoleModel>> GetByUserId(Guid userId, CancellationToken cancellationToken = default) =>  await GetListAsync(x=> x.UserId == userId, cancellationToken: cancellationToken);

    public async Task<IEnumerable<UserRoleModel>> GetUserRoles(Guid userId, Guid siteId, CancellationToken cancellationToken = default) =>  await GetListAsync(x=> x.UserId == userId && x.SiteId == siteId, cancellationToken: cancellationToken);
}
