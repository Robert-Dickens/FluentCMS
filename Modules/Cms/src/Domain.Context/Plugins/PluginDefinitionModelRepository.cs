using ByteLabs.Foundations.Domain.Repositories.EntityFrameworkCore;
using ByteLabs.Foundations.EntityFrameworkCore;
using System;

namespace FluentCMS.Domain.Plugins;

public class PluginDefinitionModelRepository : EfCoreRepository<IFluentCMSDbContext, PluginDefinitionModel, Guid>, IPluginDefinitionModelRepository
{
    public PluginDefinitionModelRepository(IDbContextProvider<IFluentCMSDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }
}
