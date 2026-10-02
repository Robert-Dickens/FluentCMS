using ByteLabs.Foundations;
using ByteLabs.Foundations.EntityFrameworkCore.Modeling;
using ByteLabs.PlatformServices.Users.Domain.Context;
using FluentCMS.Domain.ApiTokens;
using FluentCMS.Domain.Content;
using FluentCMS.Domain.IO;
using FluentCMS.Domain.Membership;
using FluentCMS.Domain.Plugins;
using FluentCMS.Domain.Settings;
using FluentCMS.Membership;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace FluentCMS.Domain.Context;

public static class FluentCMSDbContextModelBuilderExtensions
{
    public static void ConfigureFluentCMS(this ModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));

        var jsonSerializerOptions = new JsonSerializerOptions();
        jsonSerializerOptions.Converters.Add(new DictionaryJsonConverter());

        builder.Entity<SiteModel>(entity =>
        {
            entity.ToTable(FluentCMSDbProperties.DbTablePrefix + "Sites", FluentCMSDbProperties.DbSchema);
            entity.ConfigureByConvention();

            entity.ApplyObjectExtensionMappings();
        });

        builder.Entity<ApiTokenModel>(entity =>
        {
            entity.ToTable(FluentCMSDbProperties.DbTablePrefix + "ApiTokens", FluentCMSDbProperties.DbSchema);
            entity.ConfigureByConvention();

            entity.HasMany(e => e.Policies)
                .WithOne(p => p.ApiToken)
                .HasForeignKey(p => p.ApiTokenId)
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete on related Policies

            entity.Navigation(e => e.Policies).AutoInclude();
            entity.ApplyObjectExtensionMappings();
        });

        builder.Entity<ApiTokenPolicyModel>(entity =>
        {
            entity.ToTable(FluentCMSDbProperties.DbTablePrefix + "ApiTokenPolicies", FluentCMSDbProperties.DbSchema);
            entity.ConfigureByConvention();

            entity.HasOne(e => e.ApiToken)
                .WithMany(e => e.Policies)
                .HasForeignKey(e => e.ApiTokenId);
            entity.ApplyObjectExtensionMappings();
        });
      
        builder.Entity<ContentModel>(entity =>
        {
            entity.ToTable(FluentCMSDbProperties.DbTablePrefix + "Content", FluentCMSDbProperties.DbSchema);
            entity.ConfigureByConvention();

            entity.ApplyObjectExtensionMappings();
        });

        builder.Entity<ContentTypeModel>(entity =>
        {
            entity.ToTable(FluentCMSDbProperties.DbTablePrefix + "ContentTypes", FluentCMSDbProperties.DbSchema);
            entity.ConfigureByConvention();

            entity.HasMany(e => e.Fields)
                .WithOne(f => f.ContentType)
                .HasForeignKey(f => f.ContentTypeId)
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete on related Fields

            entity.Navigation(e => e.Fields).AutoInclude();
            entity.ApplyObjectExtensionMappings();
        });

        builder.Entity<ContentTypeFieldModel>(entity =>
        {
            entity.ToTable(FluentCMSDbProperties.DbTablePrefix + "ContentTypeFields", FluentCMSDbProperties.DbSchema);
            entity.ConfigureByConvention();

            entity.ApplyObjectExtensionMappings();
        });

        builder.Entity<BlockModel>(entity =>
        {
            entity.ToTable(FluentCMSDbProperties.DbTablePrefix + "ContentBlocks", FluentCMSDbProperties.DbSchema);
            entity.ConfigureByConvention();

            entity.ApplyObjectExtensionMappings();
        });

        builder.Entity<LayoutModel>(entity =>
        {
            entity.ToTable(FluentCMSDbProperties.DbTablePrefix + "Layouts", FluentCMSDbProperties.DbSchema);
            entity.ConfigureByConvention();

            entity.ApplyObjectExtensionMappings();
        });

        builder.Entity<PageModel>(entity =>
        {
            entity.ToTable(FluentCMSDbProperties.DbTablePrefix + "Pages", FluentCMSDbProperties.DbSchema);
            entity.ConfigureByConvention();

            entity.ApplyObjectExtensionMappings();
        });

        builder.Entity<PluginDefinitionModel>(entity =>
        {
            entity.ToTable(FluentCMSDbProperties.DbTablePrefix + "PluginDefinitions", FluentCMSDbProperties.DbSchema);
            entity.ConfigureByConvention();

            entity.HasMany(e => e.Types)
                .WithOne(f => f.PluginDefinition)
                .HasForeignKey(f => f.PluginDefinitionId)
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete on related Fields

            entity.Navigation(e => e.Types).AutoInclude();
            entity.ApplyObjectExtensionMappings();
        });

        builder.Entity<PluginModel>(entity =>
        {
            entity.ToTable(FluentCMSDbProperties.DbTablePrefix + "Plugins", FluentCMSDbProperties.DbSchema);
            entity.ConfigureByConvention();

            entity.ApplyObjectExtensionMappings();
        });

        builder.Entity<PluginContentModel>(entity =>
        {
            entity.ToTable(FluentCMSDbProperties.DbTablePrefix + "PluginContents", FluentCMSDbProperties.DbSchema);
            entity.ConfigureByConvention();

            entity.ApplyObjectExtensionMappings();
        });

        builder.Entity<PluginDefinitionTypeModel>(entity =>
        {
            entity.ToTable(FluentCMSDbProperties.DbTablePrefix + "PluginDefinitionTypes", FluentCMSDbProperties.DbSchema);
            entity.ConfigureByConvention();

            entity.ApplyObjectExtensionMappings();
        });

        builder.Entity<GlobalSettingsModel>(entity =>
        {
            entity.ToTable(FluentCMSDbProperties.DbTablePrefix + "GlobalSettings", FluentCMSDbProperties.DbSchema);
            entity.ConfigureByConvention();

            entity.ApplyObjectExtensionMappings();
        });

        builder.Entity<SettingsModel>(entity =>
        {
            entity.ToTable(FluentCMSDbProperties.DbTablePrefix + "Settings", FluentCMSDbProperties.DbSchema);
            entity.ConfigureByConvention();

            // Configure the relationship with SettingValuesModel
            entity.HasMany(e => e.Values) // One SettingsModel has many SettingValuesModel
                .WithOne(e => e.Setting) // Each SettingValuesModel belongs to one SettingsModel
                .HasForeignKey(e => e.SettingId) // Foreign key in SettingValuesModel
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete

            // Enable automatic inclusion of navigation property
            entity.Navigation(e => e.Values).AutoInclude(); // Always include Values when querying SettingsModel
            entity.ApplyObjectExtensionMappings();
        });

        builder.Entity<SettingValuesModel>(entity =>
        {
            entity.ToTable(FluentCMSDbProperties.DbTablePrefix + "SettingValues", FluentCMSDbProperties.DbSchema);
            entity.ConfigureByConvention();

            // Define the primary key
            entity.HasKey(e => e.Id);

            // Define the foreign key to SettingsModel
            entity.HasOne(e => e.Setting) // Each SettingValuesModel belongs to one SettingsModel
                .WithMany(e => e.Values) // One SettingsModel has many SettingValuesModel
                .HasForeignKey(e => e.SettingId) // Foreign key in SettingValuesModel
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete on SettingsModel deletion

            entity.ApplyObjectExtensionMappings();
        });

        builder.Entity<FileModel>(entity =>
        {
            entity.ToTable(FluentCMSDbProperties.DbTablePrefix + "Files", FluentCMSDbProperties.DbSchema);
            entity.ConfigureByConvention();

            entity.ApplyObjectExtensionMappings();
        });

        builder.Entity<FolderModel>(entity =>
        {
            entity.ToTable(FluentCMSDbProperties.DbTablePrefix + "Folders", FluentCMSDbProperties.DbSchema);
            entity.ConfigureByConvention();

            entity.ApplyObjectExtensionMappings();
        });

        builder.Entity<UserModel>(entity =>
        {
            entity.ToTable(FluentCMSDbProperties.DbTablePrefix + "SiteMembers", FluentCMSDbProperties.DbSchema);
            entity.ConfigureByConvention();

            entity.ConfigureAppUser();

            entity.Property(u => u.Tokens)
                .HasConversion(
                    tokens => JsonSerializer.Serialize(tokens, jsonSerializerOptions),
                    tokens => JsonSerializer.Deserialize<List<IdentityUserToken<Guid>>>(tokens, jsonSerializerOptions) ?? new List<IdentityUserToken<Guid>>());

            entity.Property(u => u.Logins)
                .HasConversion(
                    logins => JsonSerializer.Serialize(logins, jsonSerializerOptions),
                    logins => JsonSerializer.Deserialize<List<IdentityUserLogin<Guid>>>(logins, jsonSerializerOptions) ?? new List<IdentityUserLogin<Guid>>());

            entity.Property(u => u.RecoveryCodes)
                .HasConversion(
                    codes => JsonSerializer.Serialize(codes, jsonSerializerOptions),
                    codes => JsonSerializer.Deserialize<List<UserTwoFactorRecoveryCode>>(codes, jsonSerializerOptions) ?? new List<UserTwoFactorRecoveryCode>());

            entity.Property(u => u.Claims)
                .HasConversion(
                    claims => JsonSerializer.Serialize(claims, jsonSerializerOptions),
                    claims => JsonSerializer.Deserialize<List<IdentityUserClaim<Guid>>>(claims, jsonSerializerOptions) ?? new List<IdentityUserClaim<Guid>>());
            entity.ApplyObjectExtensionMappings();
        });

        builder.Entity<RoleModel>(entity =>
        {
            entity.ToTable(FluentCMSDbProperties.DbTablePrefix + "Roles", FluentCMSDbProperties.DbSchema);
            entity.ConfigureByConvention();

            entity.ApplyObjectExtensionMappings();
        });

        builder.Entity<UserRoleModel>(entity =>
        {
            entity.ToTable(FluentCMSDbProperties.DbTablePrefix + "UserRoles", FluentCMSDbProperties.DbSchema);
            entity.ConfigureByConvention();

            entity.ApplyObjectExtensionMappings();
        });

        if (builder.IsUsingPostgreSql())
        {
            //This script requierd when use postgres
            // Configure value converters for all DateTime properties
            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    if (property.ClrType == typeof(DateTime) || property.ClrType == typeof(DateTime?))
                    {
                        property.SetValueConverter(new ValueConverter<DateTime, DateTime>(
                            v => v.Kind == DateTimeKind.Unspecified ? DateTime.SpecifyKind(v, DateTimeKind.Utc) : v.ToUniversalTime(),
                            v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
                        ));
                    }
                }
            }
        }

        builder.TryConfigureObjectExtensions<FluentCMSDbContext>();

    }
}
