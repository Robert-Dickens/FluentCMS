using ByteLabs.Foundations;
using ByteLabs.Foundations.Domain.Repositories.EntityFrameworkCore;
using ByteLabs.Foundations.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCMS.Domain.Content;

public class ContentTypeModelRepository : EfCoreRepository<IFluentCMSDbContext, ContentTypeModel, Guid>, IContentTypeModelRepository
{
    public ContentTypeModelRepository(IDbContextProvider<IFluentCMSDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public async Task<ContentTypeModel?> DeleteWithResultAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var siteModel = await GetAsync(id, cancellationToken: cancellationToken);

        await DeleteAsync(siteModel, cancellationToken: cancellationToken);

        using (DataFilter.Disable<ISoftDelete>())
        {
            return await FindAsync(x => x.Id == id, cancellationToken: cancellationToken);
        }
    }

    public async Task<IEnumerable<ContentTypeModel>> GetAllForSite(Guid siteId, CancellationToken cancellationToken = default) => await GetListAsync(x => x.SiteId == siteId, cancellationToken: cancellationToken);

    public Task<ContentTypeModel> GetBySlug(Guid siteId, string contentTypeSlug, CancellationToken cancellationToken = default) => GetAsync(x => x.SiteId == siteId && string.Equals(x.Slug, contentTypeSlug, StringComparison.CurrentCultureIgnoreCase), cancellationToken: cancellationToken);
}
