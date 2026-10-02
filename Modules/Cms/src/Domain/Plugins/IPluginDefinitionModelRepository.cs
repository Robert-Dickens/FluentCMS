using ByteLabs.Foundations.Domain.Repositories;
using System;

namespace FluentCMS.Domain.Plugins;

public interface IPluginDefinitionModelRepository : IRepository<PluginDefinitionModel, Guid>
{

}