using ByteLabs.Foundations.Domain.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCMS.Domain.Content;

public interface IContentTypeModelRepository : IRepository<ContentTypeModel, Guid>, ISiteAssociatedRepository<ContentTypeModel>
{
    Task<ContentTypeModel?> DeleteWithResultAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken));
    Task<ContentTypeModel?> GetBySlug(Guid siteId, string contentTypeSlug, CancellationToken cancellationToken = default);

}