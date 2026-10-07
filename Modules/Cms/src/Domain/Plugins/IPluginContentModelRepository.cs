using ByteLabs.Foundations.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCMS.Domain.Plugins;

public interface IPluginContentModelRepository : IRepository<PluginContentModel, Guid>
{
    Task<PluginContentModel?> DeleteWithResultAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken));
    Task<IEnumerable<PluginContentModel>> GetByPluginId(Guid pluginId, CancellationToken cancellationToken = default);

}