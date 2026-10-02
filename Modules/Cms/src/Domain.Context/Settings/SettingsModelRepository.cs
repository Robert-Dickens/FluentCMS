using ByteLabs.Foundations.Domain.Repositories.EntityFrameworkCore;
using ByteLabs.Foundations.EntityFrameworkCore;
using System;

namespace FluentCMS.Domain.Settings;

public class SettingsModelRepository : EfCoreRepository<IFluentCMSDbContext, SettingsModel, Guid>, ISettingsModelRepository
{
    public SettingsModelRepository(IDbContextProvider<IFluentCMSDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }
}
