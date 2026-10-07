using ByteLabs.Foundations.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCMS.Domain.Plugins;

public interface IPluginModelRepository : IRepository<PluginModel, Guid>
{
    Task<PluginModel?> DeleteWithResultAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken));

    Task<IEnumerable<PluginModel>> GetByPageId(Guid pageId, CancellationToken cancellationToken = default);
    Task<PluginModel?> UpdateOrder(Guid pluginId, string section, int order, CancellationToken cancellationToken = default);
    Task<PluginModel?> UpdateCols(Guid pluginId, int cols, int colsMd, int colsLg, CancellationToken cancellationToken = default);

}