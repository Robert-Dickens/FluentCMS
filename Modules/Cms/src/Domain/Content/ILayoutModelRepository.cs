using ByteLabs.Foundations.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCMS.Domain.Content;

public interface ILayoutModelRepository : IRepository<LayoutModel, Guid>, ISiteAssociatedRepository<LayoutModel>
{
    Task<LayoutModel?> DeleteWithResultAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken));
    Task<IEnumerable<LayoutModel>> DeleteManyWithResultAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default(CancellationToken));
}