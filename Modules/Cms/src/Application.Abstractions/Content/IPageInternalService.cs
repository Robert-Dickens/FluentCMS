using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCMS.Content;

public interface IPageInternalService : IApplicationService
{
    Task<IEnumerable<PageModel>> GetHierarchyBySiteId(Guid siteId, CancellationToken cancellationToken = default);
    Task<Dictionary<Guid, PageModel>> GetAllBySiteId(Guid siteId, CancellationToken cancellationToken = default);
    Task<PageModel?> GetByFullPath(Guid siteId, string fullPath, CancellationToken cancellationToken = default);
    Task<PageModel?> GetById(Guid siteId, Guid pageId, CancellationToken cancellationToken = default);
    void Invalidate(Guid siteId);
}
