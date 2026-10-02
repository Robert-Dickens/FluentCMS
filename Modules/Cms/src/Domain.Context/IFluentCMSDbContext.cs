using ByteLabs.Foundations.EntityFrameworkCore;
using ByteLabs.Foundations.Data;
using Microsoft.EntityFrameworkCore;
using FluentCMS.Domain.ApiTokens;
using FluentCMS.Domain.Content;
using FluentCMS.Domain.IO;
using FluentCMS.Domain.Settings;
using FluentCMS.Domain.Plugins;
using FluentCMS.Domain.Membership;

namespace FluentCMS.Domain
{
    [ConnectionStringName(FluentCMSDbProperties.ConnectionStringName)]
    public interface IFluentCMSDbContext : IEfCoreDbContext
    {
        DbSet<ApiTokenModel> ApiTokens { get; }
        DbSet<ApiTokenPolicyModel> ApiTokenPolicies { get; }
        DbSet<BlockModel> Blocks { get; }
        DbSet<ContentModel> Contents { get; }
        DbSet<ContentTypeModel> ContentTypes { get; }
        DbSet<ContentTypeFieldModel> ContentTypeFields { get; }
        DbSet<FileModel> Files { get; }
        DbSet<FolderModel> Folders { get; }
        DbSet<GlobalSettingsModel> GlobalSettings { get; }
        DbSet<LayoutModel> Layouts { get; }
        DbSet<PageModel> Pages { get; }
        DbSet<PluginModel> Plugins { get; }
        DbSet<PluginContentModel> PluginContents { get; }
        DbSet<PluginDefinitionModel> PluginDefinitions { get; }
        DbSet<PluginDefinitionTypeModel> PluginDefinitionTypes { get; }
        DbSet<RoleModel> Roles { get; }
        DbSet<SiteModel> Sites { get; }
        DbSet<SettingsModel> Settings { get; }
        DbSet<SettingValuesModel> SettingValues { get; }
        DbSet<UserModel> Users { get; }
        DbSet<UserRoleModel> UserRoles { get; }

    }
}
