using ByteLabs.Foundations.Domain.Repositories.EntityFrameworkCore;
using ByteLabs.Foundations.EntityFrameworkCore;
using System;

namespace FluentCMS.Domain.ApiTokens;

public class ApiTokenModelRepository : EfCoreRepository<IFluentCMSDbContext, ApiTokenModel, Guid>, IApiTokenModelRepository
{
    public ApiTokenModelRepository(IDbContextProvider<IFluentCMSDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }
}

