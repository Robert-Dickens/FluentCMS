using ByteLabs.Foundations.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCMS.Domain.Content;

public interface IContentModelRepository : IRepository<ContentModel, Guid>, ISiteAssociatedRepository<ContentModel>
{
    Task<IEnumerable<ContentModel>> GetAllByContentType(Guid contentTypeId, CancellationToken cancellationToken = default);
    Task<ContentModel?> DeleteWithResultAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken));

}