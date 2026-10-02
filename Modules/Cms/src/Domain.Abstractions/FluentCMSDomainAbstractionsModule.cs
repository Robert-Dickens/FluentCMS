using ByteLabs.Foundations.Localization;
using ByteLabs.Foundations.Localization.ExceptionHandling;
using ByteLabs.Foundations.Modularity;
using ByteLabs.Foundations.Validation;
using ByteLabs.Foundations.Validation.Localization;
using ByteLabs.Foundations.VirtualFileSystem;
using ByteLabs.PlatformServices.Users;
using FluentCMS.Localization;

namespace FluentCMS;

[DependsOn(
    typeof(ValidationModule),
    typeof(UsersAbstractionModule)
)]
public class FluentCMSDomainAbstractionsModule : PlatformModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        FluentCMSModuleExtensionConfigurator.Configure();
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<VirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<FluentCMSDomainAbstractionsModule>();
        });

        Configure<PlatformLocalizationOptions>(options =>
        {
            options.Resources
                .Add<FluentCMSResource>("en")
                .AddBaseTypes(typeof(ValidationResource))
                .AddVirtualJson("/Localization/" + FluentCMSConsts.ModuleName);
        });

        Configure<ExceptionLocalizationOptions>(options =>
        {
            options.MapCodeNamespace(FluentCMSConsts.ModuleName, typeof(FluentCMSResource));

        });
    }
}
