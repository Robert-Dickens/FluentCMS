using ByteLabs.Foundations.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCMS.Domain.Content;

public interface IBlockModelRepository : IRepository<BlockModel, Guid>, ISiteAssociatedRepository<BlockModel>
{
    Task<BlockModel?> DeleteWithResultAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken));
    Task<IEnumerable<BlockModel?>> DeleteManyWithResultAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default(CancellationToken));
}