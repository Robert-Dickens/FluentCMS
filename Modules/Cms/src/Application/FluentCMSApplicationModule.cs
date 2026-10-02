using ByteLabs.Foundations.Application;
using ByteLabs.Foundations.Modularity;
using Microsoft.Extensions.DependencyInjection;
using FluentCMS.Domain;

namespace FluentCMS.Services;

[DependsOn(
    typeof(FluentCMSDomainModule),
    typeof(FluentCMSApplicationAbstractionsModule),
    typeof(PlatformApplicationModule)
    )]
public class FluentCMSApplicationModule : PlatformModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddMapperlyObjectMapper<FluentCMSApplicationModule>();
    }
}
