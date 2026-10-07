using ByteLabs.Foundations;
using ByteLabs.Foundations.Domain.Repositories.EntityFrameworkCore;
using ByteLabs.Foundations.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCMS.Domain.IO;

public class FileModelRepository : EfCoreRepository<IFluentCMSDbContext, FileModel, Guid>, IFileModelRepository
{
    public FileModelRepository(IDbContextProvider<IFluentCMSDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public async Task<FileModel?> DeleteWithResultAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var siteModel = await GetAsync(id, cancellationToken: cancellationToken);

        await DeleteAsync(siteModel, cancellationToken: cancellationToken);

        using (DataFilter.Disable<ISoftDelete>())
        {
            return await FindAsync(x => x.Id == id, cancellationToken: cancellationToken);
        }
    }

    public async Task<IEnumerable<FileModel>> GetAllForSite(Guid siteId, CancellationToken cancellationToken = default) => await GetListAsync(x => x.SiteId == siteId, cancellationToken: cancellationToken);

    public Task<FileModel> GetByName(Guid siteId, Guid folderId, string normalizedFileName, CancellationToken cancellationToken = default) => GetAsync(x => x.SiteId == siteId && x.FolderId == folderId && x.NormalizedName == normalizedFileName, cancellationToken: cancellationToken);
}
