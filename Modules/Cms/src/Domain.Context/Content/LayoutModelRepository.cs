using ByteLabs.Foundations;
using ByteLabs.Foundations.Domain.Repositories.EntityFrameworkCore;
using ByteLabs.Foundations.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCMS.Domain.Content;

public class LayoutModelRepository : EfCoreRepository<IFluentCMSDbContext, LayoutModel, Guid>, ILayoutModelRepository
{
    public LayoutModelRepository(IDbContextProvider<IFluentCMSDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public async Task<IEnumerable<LayoutModel>> DeleteManyWithResultAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default)
    {
        await DeleteManyAsync(ids, true, cancellationToken: cancellationToken);

        using (DataFilter.Disable<ISoftDelete>())
        {
            return await GetListAsync(x => ids.Contains(x.Id), cancellationToken: cancellationToken);
        }
    }

    public async Task<LayoutModel?> DeleteWithResultAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var siteModel = await GetAsync(id, cancellationToken: cancellationToken);

        await DeleteAsync(siteModel, cancellationToken: cancellationToken);

        using (DataFilter.Disable<ISoftDelete>())
        {
            return await FindAsync(x => x.Id == id, cancellationToken: cancellationToken);
        }

    }

    public async Task<IEnumerable<LayoutModel>> GetAllForSite(Guid siteId, CancellationToken cancellationToken = default) => await GetListAsync(x => x.SiteId == siteId, cancellationToken: cancellationToken);
}
