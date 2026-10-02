using ByteLabs.Foundations.Domain.Repositories.EntityFrameworkCore;
using ByteLabs.Foundations.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCMS.Domain.Content;

public class ContentModelRepository : EfCoreRepository<IFluentCMSDbContext, ContentModel, Guid>, IContentModelRepository
{
    public ContentModelRepository(IDbContextProvider<IFluentCMSDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public async Task<IEnumerable<ContentModel>> GetAllByContentType(Guid contentTypeId, CancellationToken cancellationToken = default)
    {
        var q = await GetQueryableAsync();
        q = q.Where(x => x.TypeId == contentTypeId);
        return await AsyncExecuter.ToListAsync(q, cancellationToken);
    }
}
