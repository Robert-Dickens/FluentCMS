using ByteLabs.Foundations.Modularity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Playwright;

namespace FluentCMS;

[DependsOn(
    //typeof(AspNetCoreComponentsModule),
    typeof(FluentCMSDomainContextTestModule)
    //typeof(FluentCMSBlazorModule)
)]
public class FluentCMSAcceptanceTestModule : PlatformModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        //context.Services.Replace(ServiceDescriptor.Transient<IComponentActivator, ServiceProviderComponentActivator>());

        RegisterPlaywright(context);

    }

    private  void RegisterPlaywright( ServiceConfigurationContext context)
    {
        context.Services.AddTransient(async _ =>
        {
            var playwright = await Playwright.CreateAsync().ConfigureAwait(false);
            var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = false,
                SlowMo = 200
            }).ConfigureAwait(false);
            return await browser.NewPageAsync().ConfigureAwait(false);
        }).As<Task<IPage>>();
    }
}
