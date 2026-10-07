using ByteLabs.Foundations;
using ByteLabs.Foundations.Domain.Repositories.EntityFrameworkCore;
using ByteLabs.Foundations.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCMS.Domain.Content;

public class PageModelRepository : EfCoreRepository<IFluentCMSDbContext, PageModel, Guid>, IPageModelRepository
{
    public PageModelRepository(IDbContextProvider<IFluentCMSDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public async Task<PageModel?> DeleteWithResultAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var siteModel = await GetAsync(id, cancellationToken: cancellationToken);

        await DeleteAsync(siteModel, cancellationToken: cancellationToken);

        using (DataFilter.Disable<ISoftDelete>())
        {
            return await FindAsync(x => x.Id == id, cancellationToken: cancellationToken);
        }

    }
    public async Task<IEnumerable<PageModel>> GetAllForSite(Guid siteId, CancellationToken cancellationToken = default) => await GetListAsync(x => x.SiteId == siteId, cancellationToken: cancellationToken);
}
