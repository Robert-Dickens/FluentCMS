using ByteLabs.Foundations.Domain.Repositories.EntityFrameworkCore;
using ByteLabs.Foundations.EntityFrameworkCore;
using System;

namespace FluentCMS.Domain.Content;

public class PageModelRepository : EfCoreRepository<IFluentCMSDbContext, PageModel, Guid>, IPageModelRepository
{
    public PageModelRepository(IDbContextProvider<IFluentCMSDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }
}
