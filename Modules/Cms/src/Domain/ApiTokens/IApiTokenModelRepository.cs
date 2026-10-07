using ByteLabs.Foundations.Domain.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCMS.Domain.ApiTokens;

public interface IApiTokenModelRepository : IRepository<ApiTokenModel, Guid>
{
    Task<ApiTokenModel?> GetByKey(string apiKey, CancellationToken cancellationToken);
    Task<ApiTokenModel?> GetByName(string name, CancellationToken cancellationToken);
    Task<ApiTokenModel?> DeleteWithResultAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken));

}