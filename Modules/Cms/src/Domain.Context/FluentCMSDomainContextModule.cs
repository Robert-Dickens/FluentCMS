using ByteLabs.Foundations.EntityFrameworkCore;
using ByteLabs.Foundations.Modularity;
using ByteLabs.PlatformServices.Users.Domain.Context;
using FluentCMS.Domain.ApiTokens;
using FluentCMS.Domain.Content;
using FluentCMS.Domain.Context;
using FluentCMS.Domain.IO;
using FluentCMS.Domain.Membership;
using FluentCMS.Domain.Permissions;
using FluentCMS.Domain.Plugins;
using FluentCMS.Domain.Settings;
using Microsoft.Extensions.DependencyInjection;

namespace FluentCMS.Domain;

[DependsOn(
    typeof(EntityFrameworkCoreModule),
    typeof(FluentCMSDomainModule),
    typeof(UsersDomainContextModule)
)]
public class FluentCMSDomainContextModule : PlatformModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        FluentCMSEfCoreEntityExtensionMappings.Configure();
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddPlatformDbContext<FluentCMSDbContext>(options =>
        {
            options.AddRepository<SettingsModel, SettingsModelRepository>();
            options.AddRepository<GlobalSettingsModel, GlobalSettingsModelRepository>();
            options.AddRepository<PluginModel, PluginModelRepository>();
            options.AddRepository<PluginDefinitionModel, PluginDefinitionModelRepository>();
            options.AddRepository<PluginContentModel, PluginContentModelRepository>();
            options.AddRepository<FolderModel, FolderModelRepository>();
            options.AddRepository<FileModel, FileModelRepository>();
            options.AddRepository<SiteModel, SiteModelRepository>();
            options.AddRepository<PageModel, PageModelRepository>();
            options.AddRepository<LayoutModel, LayoutModelRepository>();
            options.AddRepository<ContentTypeModel, ContentTypeModelRepository>();
            options.AddRepository<ContentTypeFieldModel, ContentTypeFieldModelRepository>();
            options.AddRepository<ContentModel, ContentModelRepository>();
            options.AddRepository<BlockModel, BlockModelRepository>();
            options.AddRepository<ApiTokenModel, ApiTokenModelRepository>();
            options.AddRepository<RoleModel, RoleModelRepository>();
            options.AddRepository<UserModel, UserMembershipRepository>();
            options.AddRepository<UserRoleModel, UserRoleModelRepository>();
            options.AddRepository<PermissionModel, PermissionModelRepository>();
            /* default repositories only for aggregate roots */
            options.AddDefaultRepositories(includeAllEntities: true);
        });
    }
}
