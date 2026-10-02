using ByteLabs.Foundations.EntityFrameworkCore;
using ByteLabs.Foundations.Data;
using Microsoft.EntityFrameworkCore;
using FluentCMS.Domain.ApiTokens;
using FluentCMS.Domain.Content;
using FluentCMS.Domain.IO;
using FluentCMS.Domain.Settings;
using FluentCMS.Domain.Plugins;
using FluentCMS.Domain.Membership;

namespace FluentCMS.Domain.Context;

[ConnectionStringName(FluentCMSDbProperties.ConnectionStringName)]
public class FluentCMSDbContext : PlatformDbContext<FluentCMSDbContext>, IFluentCMSDbContext
{

    #region DbSets

    // DbSets for all entities
    public DbSet<ApiTokenModel> ApiTokens { get; protected set; } = default!;
    public DbSet<ApiTokenPolicyModel> ApiTokenPolicies { get; protected set; } = default!;
    public DbSet<BlockModel> Blocks { get; protected set; } = default!;
    public DbSet<ContentModel> Contents { get; protected set; } = default!;
    public DbSet<ContentTypeModel> ContentTypes { get; protected set; } = default!;
    public DbSet<ContentTypeFieldModel> ContentTypeFields { get; protected set; } = default!;
    public DbSet<FileModel> Files { get; protected set; } = default!;
    public DbSet<FolderModel> Folders { get; protected set; } = default!;
    public DbSet<GlobalSettingsModel> GlobalSettings { get; protected set; } = default!;
    public DbSet<LayoutModel> Layouts { get; protected set; } = default!;
    public DbSet<PageModel> Pages { get; protected set; } = default!;
    public DbSet<PluginModel> Plugins { get; protected set; } = default!;
    public DbSet<PluginContentModel> PluginContents { get; protected set; } = default!;
    public DbSet<PluginDefinitionModel> PluginDefinitions { get; protected set; } = default!;
    public DbSet<PluginDefinitionTypeModel> PluginDefinitionTypes { get; protected set; } = default!;
    public DbSet<RoleModel> Roles { get; protected set; } = default!;
    public DbSet<SiteModel> Sites { get; protected set; } = default!;
    public DbSet<SettingsModel> Settings { get; protected set; } = default!;
    public DbSet<SettingValuesModel> SettingValues { get; protected set; } = default!;
    public DbSet<UserModel> Users { get; protected set; } = default!;
    public DbSet<UserRoleModel> UserRoles { get; protected set; } = default!;

    #endregion

    public FluentCMSDbContext(DbContextOptions<FluentCMSDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ConfigureFluentCMS();
    }
}
