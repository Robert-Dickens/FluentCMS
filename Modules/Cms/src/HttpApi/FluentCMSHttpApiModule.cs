using ByteLabs.Foundations.AspNetCore.Mvc;
using ByteLabs.Foundations.AspNetCore.UI.Localization;
using ByteLabs.Foundations.Localization;
using ByteLabs.Foundations.Modularity;
using FluentCMS.Localization;
using Microsoft.Extensions.DependencyInjection;

namespace FluentCMS.Api;

[DependsOn(
    typeof(FluentCMSApplicationAbstractionsModule),
    typeof(AspNetCoreMvcModule))]
public class FluentCMSHttpApiModule : PlatformModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(FluentCMSHttpApiModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<PlatformLocalizationOptions>(options =>
        {
            options.Resources
                .Get<FluentCMSResource>()
                .AddBaseTypes(typeof(UICultureResource));
        });
    }
}
