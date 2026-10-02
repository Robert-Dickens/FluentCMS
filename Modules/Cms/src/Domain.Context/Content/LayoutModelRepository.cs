using ByteLabs.Foundations.Domain.Repositories.EntityFrameworkCore;
using ByteLabs.Foundations.EntityFrameworkCore;
using System;

namespace FluentCMS.Domain.Content;

public class LayoutModelRepository : EfCoreRepository<IFluentCMSDbContext, LayoutModel, Guid>, ILayoutModelRepository
{
    public LayoutModelRepository(IDbContextProvider<IFluentCMSDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }
}
