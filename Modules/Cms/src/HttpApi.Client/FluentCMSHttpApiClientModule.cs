using ByteLabs.Foundations.Http.Client;
using ByteLabs.Foundations.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace FluentCMS.Client;

[DependsOn(
    typeof(FluentCMSApplicationAbstractionsModule),
    typeof(HttpClientModule))]
public class FluentCMSHttpApiClientModule : PlatformModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddHttpClientProxies(typeof(FluentCMSApplicationAbstractionsModule).Assembly, FluentCMSRemoteServiceConsts.RemoteServiceName);
    }
}
