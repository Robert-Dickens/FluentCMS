using ByteLabs.Foundations;
using ByteLabs.Foundations.Domain.Repositories.EntityFrameworkCore;
using ByteLabs.Foundations.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCMS.Domain.Plugins;

public class PluginContentModelRepository : EfCoreRepository<IFluentCMSDbContext, PluginContentModel, Guid>, IPluginContentModelRepository
{
    public PluginContentModelRepository(IDbContextProvider<IFluentCMSDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public async Task<PluginContentModel?> DeleteWithResultAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var siteModel = await GetAsync(id, cancellationToken: cancellationToken);

        await DeleteAsync(siteModel, cancellationToken: cancellationToken);

        using (DataFilter.Disable<ISoftDelete>())
        {
            return await FindAsync(x => x.Id == id, cancellationToken: cancellationToken);
        }

    }

    public async Task<IEnumerable<PluginContentModel>> GetByPluginId(Guid pluginId, CancellationToken cancellationToken = default) => await GetListAsync(x => x.PluginId == pluginId, cancellationToken: cancellationToken);
}
