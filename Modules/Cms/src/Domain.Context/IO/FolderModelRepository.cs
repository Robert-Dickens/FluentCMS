using ByteLabs.Foundations;
using ByteLabs.Foundations.Domain.Repositories.EntityFrameworkCore;
using ByteLabs.Foundations.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCMS.Domain.IO;

public class FolderModelRepository : EfCoreRepository<IFluentCMSDbContext, FolderModel, Guid>, IFolderModelRepository
{
    public FolderModelRepository(IDbContextProvider<IFluentCMSDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public async Task<FolderModel?> DeleteWithResultAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var siteModel = await GetAsync(id, cancellationToken: cancellationToken);

        await DeleteAsync(siteModel, cancellationToken: cancellationToken);

        using (DataFilter.Disable<ISoftDelete>())
        {
            return await FindAsync(x => x.Id == id, cancellationToken: cancellationToken);
        }

    }

    public async Task<IEnumerable<FolderModel>> GetAllForSite(Guid siteId, CancellationToken cancellationToken = default) => await GetListAsync(x => x.SiteId == siteId, cancellationToken: cancellationToken);

    public Task<FolderModel> GetByName(Guid siteId, Guid? parentId, string normalizedName, CancellationToken cancellationToken = default) => GetAsync(x=> x.SiteId == siteId && x.ParentId == parentId && x.NormalizedName == normalizedName, cancellationToken: cancellationToken);
}
