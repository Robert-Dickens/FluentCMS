using ByteLabs.Foundations;
using ByteLabs.Foundations.Domain.Repositories.EntityFrameworkCore;
using ByteLabs.Foundations.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCMS.Domain.Content;

public class SiteModelRepository : EfCoreRepository<IFluentCMSDbContext, SiteModel, Guid>, ISiteModelRepository
{
    public SiteModelRepository(IDbContextProvider<IFluentCMSDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public async Task<bool> CheckUrlsForAvailability(IEnumerable<string> urls, CancellationToken cancellationToken = default)
    {
        foreach (var url in urls)
        {
            if (await GetByUrl(url, cancellationToken) is not null)
            {
                return false;
            }
        }
        return true;
    }

    public async Task<SiteModel?> DeleteWithResultAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var siteModel = await GetAsync(id, cancellationToken: cancellationToken);

        await DeleteAsync(siteModel, cancellationToken: cancellationToken);

        using(DataFilter.Disable<ISoftDelete>())
        {
           return await FindAsync(x => x.Id == id, cancellationToken: cancellationToken);
        }
    }

    public async Task<SiteModel?> GetByUrl(string url, CancellationToken cancellationToken = default)
    {
        return await FindAsync(x => x.Urls.Contains(url), cancellationToken: cancellationToken);
    }

    public async Task<bool> Initialized(CancellationToken cancellationToken = default)
    {
        var db = await GetDbContextAsync();
        var globalSettings = await db.GlobalSettings.ToListAsync(cancellationToken: cancellationToken);

        return globalSettings.Count > 0;
    }

    public Task<bool> InitializeDb(CancellationToken cancellationToken = default) => throw new NotImplementedException();
}
