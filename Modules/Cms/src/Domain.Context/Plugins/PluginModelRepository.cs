using ByteLabs.Foundations;
using ByteLabs.Foundations.Domain.Repositories.EntityFrameworkCore;
using ByteLabs.Foundations.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCMS.Domain.Plugins;

public class PluginModelRepository : EfCoreRepository<IFluentCMSDbContext, PluginModel, Guid>, IPluginModelRepository
{
    public PluginModelRepository(IDbContextProvider<IFluentCMSDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public async Task<PluginModel?> DeleteWithResultAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var siteModel = await GetAsync(id, cancellationToken: cancellationToken);

        await DeleteAsync(siteModel, cancellationToken: cancellationToken);

        using (DataFilter.Disable<ISoftDelete>())
        {
            return await FindAsync(x => x.Id == id, cancellationToken: cancellationToken);
        }
    }

    public Task<IEnumerable<PluginModel>> GetByPageId(Guid pageId, CancellationToken cancellationToken = default) => throw new NotImplementedException();
    public Task<PluginModel> UpdateCols(Guid pluginId, int cols, int colsMd, int colsLg, CancellationToken cancellationToken = default) => throw new NotImplementedException();
    public Task<PluginModel> UpdateOrder(Guid pluginId, string section, int order, CancellationToken cancellationToken = default) => throw new NotImplementedException();
}
