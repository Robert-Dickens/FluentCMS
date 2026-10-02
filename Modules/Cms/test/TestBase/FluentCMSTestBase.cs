using ByteLabs.Foundations;
using ByteLabs.Foundations.Domain.Repositories;
using ByteLabs.Foundations.Modularity;
using ByteLabs.Foundations.TestFactory.Testing;
using ByteLabs.Foundations.Uow;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FluentCMS;

/* All test classes are derived from this class, directly or indirectly. */
public abstract class FluentCMSTestBase<TStartupModule> : PlatformIntegratedTestBase<TStartupModule>
    where TStartupModule : IPlatformModule
{
    protected override void SetApplicationCreationOptions(ApplicationCreationOptions options)
    {
        options.UseAutofac();
    }

    protected override void BeforeAddApplication(IServiceCollection services)
    {
        var builder = new ConfigurationBuilder();
        builder.AddJsonFile("appsettings.json", false);
        builder.AddJsonFile("appsettings.secrets.json", true);
        services.ReplaceConfiguration(builder.Build());
    }

    protected virtual Task WithUnitOfWorkAsync(Func<Task> func)
    {
        return WithUnitOfWorkAsync(new UnitOfWorkOptions(), func);
    }

    protected virtual async Task WithUnitOfWorkAsync(UnitOfWorkOptions options, Func<Task> action)
    {
        using (var scope = ServiceProvider.CreateScope())
        {
            var uowManager = scope.ServiceProvider.GetRequiredService<IUnitOfWorkManager>();

            using (var uow = uowManager.Begin(options))
            {
                await action();

                await uow.CompleteAsync();
            }
        }
    }

    protected virtual Task<TResult> WithUnitOfWorkAsync<TResult>(Func<Task<TResult>> func)
    {
        return WithUnitOfWorkAsync(new UnitOfWorkOptions(), func);
    }

    protected virtual async Task<TResult> WithUnitOfWorkAsync<TResult>(UnitOfWorkOptions options, Func<Task<TResult>> func)
    {
        using (var scope = ServiceProvider.CreateScope())
        {
            var uowManager = scope.ServiceProvider.GetRequiredService<IUnitOfWorkManager>();

            using (var uow = uowManager.Begin(options))
            {
                var result = await func();
                await uow.CompleteAsync();
                return result;
            }
        }
    }
}


public abstract class FluentCMSTestBase<TStartupModule, TRepository> : FluentCMSTestBase<TStartupModule>
    where TStartupModule : IPlatformModule
    where TRepository: IRepository
{

    protected virtual TRepository GetRepository()
    {
        return GetRequiredService<TRepository>();
    }

}
