using ByteLabs.Foundations.Authorization.Permissions;
using ByteLabs.Foundations.Localization;
using ByteLabs.Foundations.Reflection;
using FluentCMS.Localization;
using System;

namespace FluentCMS.Permissions;

public class FluentCMSPermissions
{
    public const string GroupName = FluentCMSConsts.ModuleName;
    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(FluentCMSPermissions));
    }

    public class SiteContent
    {
        public const string GroupName = FluentCMSPermissions.GroupName + ".SiteContent";

        public class Content
        {
            public const string Default = GroupName + ".Content";
            public const string Edit = Default + ".Edit";
            public const string Create = Default + ".Create";
            public const string Delete = Default + ".Delete";
        }

        public class Blocks
        {
            public const string Default = GroupName + ".Blocks";
            public const string Edit = Default + ".Edit";
            public const string Create = Default + ".Create";
            public const string Delete = Default + ".Delete";
        }

        public class ContentTypes
        {
            public const string Default = GroupName + ".ContentTypes";
            public const string Edit = Default + ".Edit";
            public const string Create = Default + ".Create";
            public const string Delete = Default + ".Delete";
        }

        public class ContentTypeFields
        {
            public const string Default = GroupName + ".ContentTypeFields";
            public const string Edit = Default + ".Edit";
            public const string Create = Default + ".Create";
            public const string Delete = Default + ".Delete";
        }

        public class Layouts
        {
            public const string Default = GroupName + ".Layouts";
            public const string Edit = Default + ".Edit";
            public const string Create = Default + ".Create";
            public const string Delete = Default + ".Delete";
        }

        public class Pages
        {
            public const string Default = GroupName + ".Pages";
            public const string Edit = Default + ".Edit";
            public const string Create = Default + ".Create";
            public const string Delete = Default + ".Delete";
        }

        public class Sites
        {
            public const string Default = GroupName + ".Sites";
            public const string Edit = Default + ".Edit";
            public const string Create = Default + ".Create";
            public const string Delete = Default + ".Delete";
        }

        public class Permissions
        {
            public const string Default = GroupName + ".Permissions";
            public const string Edit = Default + ".Edit";
            public const string Create = Default + ".Create";
            public const string Delete = Default + ".Delete";
        }

    }

    public class ApiTokens
    {
        public const string GroupName = FluentCMSPermissions.GroupName + ".ApiTokens";

        public class ApiToken
        {
            public const string Default = GroupName + ".ApiToken";
            public const string Edit = Default + ".Edit";
            public const string Create = Default + ".Create";
            public const string Delete = Default + ".Delete";
        }
    }

    public class IO
    {
        public const string GroupName = FluentCMSPermissions.GroupName + ".IO";

        public class Files
        {
            public const string Default = GroupName + ".Files";
            public const string Edit = Default + ".Edit";
            public const string Create = Default + ".Create";
            public const string Delete = Default + ".Delete";
        }

        public class Folders
        {
            public const string Default = GroupName + ".Folders";
            public const string Edit = Default + ".Edit";
            public const string Create = Default + ".Create";
            public const string Delete = Default + ".Delete";
        }
    }

    public class Plugins
    {
        public const string GroupName = FluentCMSPermissions.GroupName + ".Plugins";

        public class Plugin
        {
            public const string Default = GroupName + ".Plugin";
            public const string Edit = Default + ".Edit";
            public const string Create = Default + ".Create";
            public const string Delete = Default + ".Delete";
        }

        public class PluginDefinitions
        {
            public const string Default = GroupName + ".PluginDefinition";
            public const string Edit = Default + ".Edit";
            public const string Create = Default + ".Create";
            public const string Delete = Default + ".Delete";
        }

        public class PluginContents
        {
            public const string Default = GroupName + ".PluginContents";
            public const string Edit = Default + ".Edit";
            public const string Create = Default + ".Create";
            public const string Delete = Default + ".Delete";
        }
    }

    public class Settings
    {
        public const string GroupName = FluentCMSPermissions.GroupName + ".Settings";

        public class GlobalSettings
        {
            public const string Default = GroupName + ".GlobalSettings";
            public const string Edit = Default + ".Edit";
            public const string Create = Default + ".Create";
            public const string Delete = Default + ".Delete";
        }

        public class TenantSettings
        {
            public const string Default = GroupName + ".TenantSettings";
            public const string Edit = Default + ".Edit";
            public const string Create = Default + ".Create";
            public const string Delete = Default + ".Delete";
        }
    }


    public class MemberShips
    {
        public const string GroupName = FluentCMSPermissions.GroupName + ".Membership";

        public class Members
        {
            public const string Default = GroupName + ".Members";
            public const string Edit = Default + ".Edit";
            public const string Create = Default + ".Create";
            public const string Delete = Default + ".Delete";
            public const string ResetPassword = Default + ".ResetPassword";
        }

        public class Roles
        {
            public const string Default = GroupName + ".Roles";
            public const string Edit = Default + ".Edit";
            public const string Create = Default + ".Create";
            public const string Delete = Default + ".Delete";
        }

        public class UserRoles
        {
            public const string Default = GroupName + ".UserRoles";
            public const string Edit = Default + ".Edit";
            public const string Create = Default + ".Create";
            public const string Delete = Default + ".Delete";
        }
    }

}

public class FluentCMSPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(FluentCMSPermissions.GroupName, L($"Permission:{FluentCMSPermissions.GroupName}"));

        RegisterContentPermissions(myGroup);
        RegisterApiTokenPermissions(myGroup);
        RegisterIOPermissions(myGroup);
        RegisterMembershipPermissions(myGroup);
        RegisterPluginPermissions(myGroup);
        RegisterSettingsPermissions(myGroup);
    }

    private static void RegisterContentPermissions(PermissionGroupDefinition myGroup)
    {
        RegisterCrudPermissions(myGroup, FluentCMSPermissions.SiteContent.Content.Default);
        RegisterCrudPermissions(myGroup, FluentCMSPermissions.SiteContent.ContentTypes.Default);
        RegisterCrudPermissions(myGroup, FluentCMSPermissions.SiteContent.ContentTypeFields.Default);
        RegisterCrudPermissions(myGroup, FluentCMSPermissions.SiteContent.Blocks.Default);
        RegisterCrudPermissions(myGroup, FluentCMSPermissions.SiteContent.Layouts.Default);
        RegisterCrudPermissions(myGroup, FluentCMSPermissions.SiteContent.Pages.Default);
        RegisterCrudPermissions(myGroup, FluentCMSPermissions.SiteContent.Sites.Default);
        RegisterCrudPermissions(myGroup, FluentCMSPermissions.SiteContent.Permissions.Default);
    }

    private static void RegisterApiTokenPermissions(PermissionGroupDefinition myGroup)
    {
        RegisterCrudPermissions(myGroup, FluentCMSPermissions.ApiTokens.ApiToken.Default);
    }

    private static void RegisterIOPermissions(PermissionGroupDefinition myGroup)
    {
        RegisterCrudPermissions(myGroup, FluentCMSPermissions.IO.Files.Default);
        RegisterCrudPermissions(myGroup, FluentCMSPermissions.IO.Folders.Default);
    }

    private static void RegisterPluginPermissions(PermissionGroupDefinition myGroup)
    {
        RegisterCrudPermissions(myGroup, FluentCMSPermissions.Plugins.Plugin.Default);
        RegisterCrudPermissions(myGroup, FluentCMSPermissions.Plugins.PluginDefinitions.Default);
        RegisterCrudPermissions(myGroup, FluentCMSPermissions.Plugins.PluginContents.Default);
    }

    private static void RegisterSettingsPermissions(PermissionGroupDefinition myGroup)
    {
        RegisterCrudPermissions(myGroup, FluentCMSPermissions.Settings.GlobalSettings.Default);
        RegisterCrudPermissions(myGroup, FluentCMSPermissions.Settings.TenantSettings.Default);
    }

    private static void RegisterMembershipPermissions(PermissionGroupDefinition myGroup)
    {

        var membershipPermission = RegisterCrudPermissions(myGroup, FluentCMSPermissions.MemberShips.Members.Default);
        membershipPermission.AddChild(FluentCMSPermissions.MemberShips.Members.ResetPassword, L("Permission:ResetPassword"));

        RegisterCrudPermissions(myGroup, FluentCMSPermissions.MemberShips.Roles.Default);
        RegisterCrudPermissions(myGroup, FluentCMSPermissions.MemberShips.UserRoles.Default);

    }

    private static PermissionDefinition RegisterCrudPermissions(PermissionGroupDefinition myGroup, string defaultPermissionName)
    {
        var permission = myGroup.AddPermission(defaultPermissionName, L($"Permission:{defaultPermissionName}"));
        permission.AddChild($"{defaultPermissionName}.Create", L("Permission:Create"));
        permission.AddChild($"{defaultPermissionName}.Edit", L("Permission:Edit"));
        permission.AddChild($"{defaultPermissionName}.Delete", L("Permission:Delete"));
        return permission;
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<FluentCMSResource>(name);
    }
}
