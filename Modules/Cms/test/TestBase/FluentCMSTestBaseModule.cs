using ByteLabs.Foundations;
using ByteLabs.Foundations.Auditing;
using ByteLabs.Foundations.Authorization;
using ByteLabs.Foundations.Autofac;
using ByteLabs.Foundations.Data;
using ByteLabs.Foundations.Mapperly;
using ByteLabs.Foundations.Modularity;
using ByteLabs.Foundations.MultiTenancy;
using ByteLabs.Foundations.TestFactory;
using ByteLabs.Foundations.Threading;
using FluentCMS.DataSeeders;
using FluentCMS.Domain;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FluentCMS;

[DependsOn(
    typeof(AutofacModule),
    typeof(MapperlyModule),
    typeof(TestBaseModule),
    typeof(PlatformAuthorizationModule),
    typeof(AuditingModule),
    typeof(MultiTenancyModule),
    typeof(FluentCMSDomainModule)
)]
public class FluentCMSTestBaseModule : PlatformModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddRandomDataBuilder();

        context.Services.AddLogging(options =>
        {
            options.AddDebug();
        });
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        SeedTestData(context);
    }

    private static void SeedTestData(ApplicationInitializationContext context)
    {
        AsyncHelper.RunSync(async () =>
        {
            foreach (var tenant in KnownTenants.TenantIds)
            {
                using (var scope = context.ServiceProvider.CreateScope())
                {
                    var currentTenant = scope.ServiceProvider.GetRequiredService<ICurrentTenant>();
                    using (currentTenant.Change(tenant))
                    {

                        await scope.ServiceProvider
                            .GetRequiredService<IDataSeeder>()
                            .SeedAsync(tenant);
                    }
                }
            }
        });
    }
}
