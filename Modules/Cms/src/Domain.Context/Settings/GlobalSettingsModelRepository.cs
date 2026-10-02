using ByteLabs.Foundations.Domain.Repositories.EntityFrameworkCore;
using ByteLabs.Foundations.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using System;

namespace FluentCMS.Domain.Settings;

public class GlobalSettingsModelRepository : EfCoreRepository<IFluentCMSDbContext, GlobalSettingsModel, Guid>, IGlobalSettingsModelRepository
{
    public GlobalSettingsModelRepository(IDbContextProvider<IFluentCMSDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public async Task<GlobalSettingsModel?> Get(CancellationToken cancellationToken = default)
    {
        var query = await GetQueryableAsync();
        return await AsyncExecuter.FirstOrDefaultAsync(query, cancellationToken);
    }
}
