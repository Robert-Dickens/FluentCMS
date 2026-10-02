using ByteLabs.Foundations.Domain.Repositories.EntityFrameworkCore;
using ByteLabs.Foundations.EntityFrameworkCore;
using System;

namespace FluentCMS.Domain.Plugins;

public class PluginContentModelRepository : EfCoreRepository<IFluentCMSDbContext, PluginContentModel, Guid>, IPluginContentModelRepository
{
    public PluginContentModelRepository(IDbContextProvider<IFluentCMSDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }
}
