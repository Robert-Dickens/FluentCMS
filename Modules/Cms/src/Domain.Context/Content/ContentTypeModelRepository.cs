using ByteLabs.Foundations.Domain.Repositories.EntityFrameworkCore;
using ByteLabs.Foundations.EntityFrameworkCore;
using System;

namespace FluentCMS.Domain.Content;

public class ContentTypeModelRepository : EfCoreRepository<IFluentCMSDbContext, ContentTypeModel, Guid>, IContentTypeModelRepository
{
    public ContentTypeModelRepository(IDbContextProvider<IFluentCMSDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }
}
