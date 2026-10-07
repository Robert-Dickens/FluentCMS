using ByteLabs.Foundations.Domain.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCMS.Domain.Content;

public interface IPageModelRepository : IRepository<PageModel, Guid>, ISiteAssociatedRepository<PageModel>
{
    Task<PageModel?> DeleteWithResultAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken));
}