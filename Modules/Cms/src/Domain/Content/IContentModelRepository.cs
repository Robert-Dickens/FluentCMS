using ByteLabs.Foundations.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCMS.Domain.Content;

public interface IContentModelRepository : IRepository<ContentModel, Guid>
{
    Task<IEnumerable<ContentModel>> GetAllByContentType(Guid contentTypeId, CancellationToken cancellationToken = default);
}