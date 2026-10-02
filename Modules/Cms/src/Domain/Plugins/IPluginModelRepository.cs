using ByteLabs.Foundations.Domain.Repositories;
using System;

namespace FluentCMS.Domain.Plugins;

public interface IPluginModelRepository : IRepository<PluginModel, Guid>
{

}