using ByteLabs.Foundations.Domain.Repositories.EntityFrameworkCore;
using ByteLabs.Foundations.EntityFrameworkCore;
using System;

namespace FluentCMS.Domain.Content;


public class BlockModelRepository : EfCoreRepository<IFluentCMSDbContext, BlockModel, Guid>, IBlockModelRepository
{
    public BlockModelRepository(IDbContextProvider<IFluentCMSDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }
}
