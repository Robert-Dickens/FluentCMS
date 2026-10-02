using ByteLabs.Foundations.Modularity;
using FluentCMS.Domain;

namespace FluentCMS;

/* This class can be used as a base class for EF Core integration tests,
 * while ProductServiceRepositoryTests uses a different approach.
 */
public abstract class FluentCMSServiceDomainContextTestBase<TStartupModule> : FluentCMSTestBase<TStartupModule>
    where TStartupModule : IPlatformModule
{
    protected virtual void UsingDbContext(Action<IFluentCMSDbContext> action)
    {
        using (var dbContext = GetRequiredService<IFluentCMSDbContext>())
        {
            action.Invoke(dbContext);
        }
    }

    protected virtual T UsingDbContext<T>(Func<IFluentCMSDbContext, T> action)
    {
        using (var dbContext = GetRequiredService<IFluentCMSDbContext>())
        {
            return action.Invoke(dbContext);
        }
    }
}


/* This class can be used as a base class for EF Core integration tests,
 * while ProductServiceRepositoryTests uses a different approach.
 */
public abstract class FluentCMSServiceDomainContextTestBase : FluentCMSServiceDomainContextTestBase<FluentCMSDomainContextTestModule>
{
}
