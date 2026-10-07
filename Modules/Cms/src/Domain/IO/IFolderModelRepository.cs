using ByteLabs.Foundations.Domain.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCMS.Domain.IO;

public interface IFolderModelRepository : IRepository<FolderModel, Guid>, ISiteAssociatedRepository<FolderModel>
{
    Task<FolderModel?> DeleteWithResultAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken));
    Task<FolderModel?> GetByName(Guid siteId, Guid? parentId, string normalizedName, CancellationToken cancellationToken = default);
}