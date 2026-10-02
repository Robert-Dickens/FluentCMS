using ByteLabs.Foundations.Domain.Repositories.EntityFrameworkCore;
using ByteLabs.Foundations.EntityFrameworkCore;
using System;

namespace FluentCMS.Domain.Content;

public class SiteModelRepository : EfCoreRepository<IFluentCMSDbContext, SiteModel, Guid>, ISiteModelRepository
{
    public SiteModelRepository(IDbContextProvider<IFluentCMSDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }
}
