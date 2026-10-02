using ByteLabs.Foundations.Domain.Repositories.EntityFrameworkCore;
using ByteLabs.Foundations.EntityFrameworkCore;
using System;

namespace FluentCMS.Domain.Membership;

public class RoleModelRepository : EfCoreRepository<IFluentCMSDbContext, RoleModel, Guid>, IRoleModelRepository
{
    public RoleModelRepository(IDbContextProvider<IFluentCMSDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }
}
