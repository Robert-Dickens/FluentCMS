using ByteLabs.Foundations.Domain.Repositories.EntityFrameworkCore;
using ByteLabs.Foundations.EntityFrameworkCore;
using System;

namespace FluentCMS.Domain.IO;

public class FolderModelRepository : EfCoreRepository<IFluentCMSDbContext, FolderModel, Guid>, IFolderModelRepository
{
    public FolderModelRepository(IDbContextProvider<IFluentCMSDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }
}
