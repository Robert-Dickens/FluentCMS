using ByteLabs.Foundations.Domain.Repositories.EntityFrameworkCore;
using ByteLabs.Foundations.EntityFrameworkCore;
using System;

namespace FluentCMS.Domain.IO;

public class FileModelRepository : EfCoreRepository<IFluentCMSDbContext, FileModel, Guid>, IFileModelRepository
{
    public FileModelRepository(IDbContextProvider<IFluentCMSDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }
}
