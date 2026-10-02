using ByteLabs.Foundations.Modularity;
using Microsoft.Extensions.DependencyInjection;
using MyNamespace.Blazor.Menus;
using ByteLabs.Foundations.AutoMapper;
using ByteLabs.Foundations.AspNetCore.UI.Navigation;
using ByteLabs.Foundations.AspNetCore.Components.Web;
using ByteLabs.Foundations.AspNetCore.Components.Web.Routing;

namespace MyNamespace.Blazor;

[DependsOn(
    typeof(MyProjectNameApplicationAbstractionsModule),
    typeof(AspNetCoreComponentsWebThemingModule),
    typeof(AutoMapperModule)
    )]
public class MyProjectNameBlazorModule : PlatformModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAutoMapperObjectMapper<MyProjectNameBlazorModule>();

        Configure<AutoMapperOptions>(options =>
        {
            options.AddProfile<MyProjectNameBlazorAutoMapperProfile>(validate: true);
        });

        Configure<AspNetCoreNavigationOptions>(options =>
        {
            options.MenuContributors.Add(new MyProjectNameMenuContributor());
        });

        Configure<AspNetCoreRouterOptions>(options =>
        {
            options.AdditionalAssemblies.Add(typeof(MyProjectNameBlazorModule).Assembly);
        });
    }
}
