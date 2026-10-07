using ByteLabs.Foundations;
using ByteLabs.Foundations.Domain.Repositories.EntityFrameworkCore;
using ByteLabs.Foundations.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCMS.Domain.Plugins;

public class PluginDefinitionModelRepository : EfCoreRepository<IFluentCMSDbContext, PluginDefinitionModel, Guid>, IPluginDefinitionModelRepository
{
    public PluginDefinitionModelRepository(IDbContextProvider<IFluentCMSDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public async Task<PluginDefinitionModel?> DeleteWithResultAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var siteModel = await GetAsync(id, cancellationToken: cancellationToken);

        await DeleteAsync(siteModel, cancellationToken: cancellationToken);

        using (DataFilter.Disable<ISoftDelete>())
        {
            return await FindAsync(x => x.Id == id, cancellationToken: cancellationToken);
        }

    }
}
