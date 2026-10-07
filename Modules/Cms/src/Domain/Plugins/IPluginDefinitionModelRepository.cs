using ByteLabs.Foundations.Domain.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCMS.Domain.Plugins;

public interface IPluginDefinitionModelRepository : IRepository<PluginDefinitionModel, Guid>
{
    Task<PluginDefinitionModel?> DeleteWithResultAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken));

}