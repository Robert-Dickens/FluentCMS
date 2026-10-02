using ByteLabs.Foundations.Modularity;
using FluentCMS.Services;
using Microsoft.Extensions.DependencyInjection;

namespace FluentCMS;

[DependsOn(
    typeof(FluentCMSApplicationModule),
    typeof(FluentCMSDomainTestModule)
    )]
public class FluentCMSApplicationTestModule : PlatformModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAlwaysAllowAuthorization();
    }
}
