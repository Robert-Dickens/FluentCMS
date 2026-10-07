using ByteLabs.Foundations;
using ByteLabs.Foundations.Domain.Repositories.EntityFrameworkCore;
using ByteLabs.Foundations.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCMS.Domain.ApiTokens;

public class ApiTokenModelRepository : EfCoreRepository<IFluentCMSDbContext, ApiTokenModel, Guid>, IApiTokenModelRepository
{
    public ApiTokenModelRepository(IDbContextProvider<IFluentCMSDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public async Task<ApiTokenModel?> DeleteWithResultAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var siteModel = await GetAsync(id, cancellationToken: cancellationToken);

        await DeleteAsync(siteModel, cancellationToken: cancellationToken);

        using (DataFilter.Disable<ISoftDelete>())
        {
            return await FindAsync(x => x.Id == id, cancellationToken: cancellationToken);
        }

    }

    public Task<ApiTokenModel> GetByKey(string apiKey, CancellationToken cancellationToken) => throw new NotImplementedException();
    public Task<ApiTokenModel> GetByName(string name, CancellationToken cancellationToken) => throw new NotImplementedException();
}

