using ByteLabs.Foundations.Domain.Repositories.EntityFrameworkCore;
using ByteLabs.Foundations.EntityFrameworkCore;
using System;

namespace FluentCMS.Domain.Content;

public class ContentTypeFieldModelRepository : EfCoreRepository<IFluentCMSDbContext, ContentTypeFieldModel, Guid>, IContentTypeFieldModelRepository
{
    public ContentTypeFieldModelRepository(IDbContextProvider<IFluentCMSDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

}
