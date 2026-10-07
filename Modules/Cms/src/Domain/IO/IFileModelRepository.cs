using ByteLabs.Foundations.Domain.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCMS.Domain.IO;

public interface IFileModelRepository : IRepository<FileModel, Guid>, ISiteAssociatedRepository<FileModel>
{
    Task<FileModel?> GetByName(Guid siteId, Guid folderId, string normalizedFileName, CancellationToken cancellationToken = default);
    Task<FileModel?> DeleteWithResultAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken));

}