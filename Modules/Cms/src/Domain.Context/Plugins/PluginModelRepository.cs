using ByteLabs.Foundations.Domain.Repositories.EntityFrameworkCore;
using ByteLabs.Foundations.EntityFrameworkCore;
using System;

namespace FluentCMS.Domain.Plugins;

public class PluginModelRepository : EfCoreRepository<IFluentCMSDbContext, PluginModel, Guid>, IPluginModelRepository
{
    public PluginModelRepository(IDbContextProvider<IFluentCMSDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }
}
