using ByteLabs.Foundations.Domain.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCMS.Domain.Settings;

public interface IGlobalSettingsModelRepository : IRepository<GlobalSettingsModel, Guid>
{
    Task<GlobalSettingsModel?> Get(CancellationToken cancellationToken = default);

}