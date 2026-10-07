using ByteLabs.Foundations.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCMS.Domain.Content;

public interface ISiteModelRepository : IRepository<SiteModel, Guid>
{
    Task<SiteModel?> GetByUrl(string url, CancellationToken cancellationToken = default);
    Task<bool> CheckUrlsForAvailability(IEnumerable<string> urls, CancellationToken cancellationToken = default);
    Task<SiteModel?> DeleteWithResultAsync(Guid id, CancellationToken cancellationToken = default);

    Task<bool> Initialized(CancellationToken cancellationToken = default);
    Task<bool> InitializeDb(CancellationToken cancellationToken = default);

}