
using ByteLabs.Foundations.Mapperly;
using FluentCMS.ApiTokens;
using FluentCMS.Content;
using FluentCMS.IO;
using FluentCMS.Membership;
using FluentCMS.Plugins;
using FluentCMS.Settings;
using Riok.Mapperly.Abstractions;
using CmsFile = FluentCMS.IO.File;
using ContentDto = FluentCMS.Content.Content;
using DomainModels = FluentCMS.Domain;
using SettingsDto = FluentCMS.Settings.Settings;

namespace FluentCMS.Services;

[Mapper]
public partial class ApiTokenMapper : TwoWayMapperBase<DomainModels.ApiTokens.ApiTokenModel, ApiToken>
{
    [MapperIgnoreSource(nameof(DomainModels.ApiTokens.ApiTokenModel.Key))]
    [MapperIgnoreSource(nameof(DomainModels.ApiTokens.ApiTokenModel.Secret))]
    [MapperIgnoreSource(nameof(DomainModels.ApiTokens.ApiTokenModel.Policies))]
    public override partial ApiToken Map(DomainModels.ApiTokens.ApiTokenModel source);
    [MapperIgnoreSource(nameof(DomainModels.ApiTokens.ApiTokenModel.Key))]
    [MapperIgnoreSource(nameof(DomainModels.ApiTokens.ApiTokenModel.Secret))]
    [MapperIgnoreSource(nameof(DomainModels.ApiTokens.ApiTokenModel.Policies))]
    public override partial void Map(DomainModels.ApiTokens.ApiTokenModel source, ApiToken destination);
    [MapperIgnoreTarget(nameof(DomainModels.ApiTokens.ApiTokenModel.Key))]
    [MapperIgnoreTarget(nameof(DomainModels.ApiTokens.ApiTokenModel.Secret))]
    [MapperIgnoreTarget(nameof(DomainModels.ApiTokens.ApiTokenModel.Policies))]
    public override partial DomainModels.ApiTokens.ApiTokenModel ReverseMap(ApiToken source);
    [MapperIgnoreTarget(nameof(DomainModels.ApiTokens.ApiTokenModel.Key))]
    [MapperIgnoreTarget(nameof(DomainModels.ApiTokens.ApiTokenModel.Secret))]
    [MapperIgnoreTarget(nameof(DomainModels.ApiTokens.ApiTokenModel.Policies))]
    public override partial void ReverseMap(ApiToken source, DomainModels.ApiTokens.ApiTokenModel destination);
}

[Mapper]
[MapExtraProperties]
public partial class BlockMapper : TwoWayMapperBase<DomainModels.Content.BlockModel, Block>
{
    [MapperIgnoreSource(nameof(DomainModels.Content.BlockModel.TenantId))]
    public override partial Block Map(DomainModels.Content.BlockModel source);
    [MapperIgnoreSource(nameof(DomainModels.Content.BlockModel.TenantId))]
    public override partial void Map(DomainModels.Content.BlockModel source, Block destination);
    [MapperIgnoreTarget(nameof(DomainModels.Content.BlockModel.TenantId))]
    public override partial DomainModels.Content.BlockModel ReverseMap(Block source);
    [MapperIgnoreTarget(nameof(DomainModels.Content.BlockModel.TenantId))]
    public override partial void ReverseMap(Block source, DomainModels.Content.BlockModel destination);
}


[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Source)]
[MapExtraProperties]
public partial class BlockCreateRequestToBlockModelMapper : MapperBase<BlockCreateRequest, DomainModels.Content.BlockModel>
{
    public override partial DomainModels.Content.BlockModel Map(BlockCreateRequest source);
    public override partial void Map(BlockCreateRequest source, DomainModels.Content.BlockModel destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Source)]
[MapExtraProperties]
public partial class BlockUpdateRequestToBlockModelMapper : MapperBase<BlockUpdateRequest, DomainModels.Content.BlockModel>
{
    public override partial DomainModels.Content.BlockModel Map(BlockUpdateRequest source);
    public override partial void Map(BlockUpdateRequest source, DomainModels.Content.BlockModel destination);
}
[Mapper]
[MapExtraProperties]
public partial class ContentMapper : TwoWayMapperBase<DomainModels.Content.ContentModel, ContentDto>
{
    [MapperIgnoreSource(nameof(DomainModels.Content.ContentModel.SiteId))]
    [MapperIgnoreSource(nameof(DomainModels.Content.ContentModel.TenantId))]
    public override partial ContentDto Map(DomainModels.Content.ContentModel source);
    [MapperIgnoreSource(nameof(DomainModels.Content.ContentModel.SiteId))]
    [MapperIgnoreSource(nameof(DomainModels.Content.ContentModel.TenantId))]
    public override partial void Map(DomainModels.Content.ContentModel source, ContentDto destination);
    [MapperIgnoreTarget(nameof(DomainModels.Content.ContentModel.SiteId))]
    [MapperIgnoreTarget(nameof(DomainModels.Content.ContentModel.TenantId))]
    public override partial DomainModels.Content.ContentModel ReverseMap(ContentDto source);
    [MapperIgnoreTarget(nameof(DomainModels.Content.ContentModel.SiteId))]
    [MapperIgnoreTarget(nameof(DomainModels.Content.ContentModel.TenantId))]
    public override partial void ReverseMap(ContentDto source, DomainModels.Content.ContentModel destination);
}

[Mapper]
[MapExtraProperties]
public partial class ContentTypeMapper : TwoWayMapperBase<DomainModels.Content.ContentTypeModel, ContentType>
{
    [MapperIgnoreSource(nameof(DomainModels.Content.ContentTypeModel.Fields))]
    [MapperIgnoreSource(nameof(DomainModels.Content.ContentTypeModel.TenantId))]
    public override partial ContentType Map(DomainModels.Content.ContentTypeModel source);
    [MapperIgnoreSource(nameof(DomainModels.Content.ContentTypeModel.Fields))]
    [MapperIgnoreSource(nameof(DomainModels.Content.ContentTypeModel.TenantId))]
    public override partial void Map(DomainModels.Content.ContentTypeModel source, ContentType destination);
    [MapperIgnoreTarget(nameof(DomainModels.Content.ContentTypeModel.Fields))]
    [MapperIgnoreTarget(nameof(DomainModels.Content.ContentTypeModel.TenantId))]
    public override partial DomainModels.Content.ContentTypeModel ReverseMap(ContentType source);
    [MapperIgnoreTarget(nameof(DomainModels.Content.ContentTypeModel.Fields))]
    [MapperIgnoreTarget(nameof(DomainModels.Content.ContentTypeModel.TenantId))]
    public override partial void ReverseMap(ContentType source, DomainModels.Content.ContentTypeModel destination);
}

[Mapper]
public partial class ContentTypeFieldMapper : TwoWayMapperBase<DomainModels.Content.ContentTypeFieldModel, ContentTypeField>
{
    [MapperIgnoreSource(nameof(DomainModels.Content.ContentTypeFieldModel.ContentType))]
    public override partial ContentTypeField Map(DomainModels.Content.ContentTypeFieldModel source);
    [MapperIgnoreSource(nameof(DomainModels.Content.ContentTypeFieldModel.ContentType))]
    public override partial void Map(DomainModels.Content.ContentTypeFieldModel source, ContentTypeField destination);
    [MapperIgnoreTarget(nameof(DomainModels.Content.ContentTypeFieldModel.ContentType))]
    public override partial DomainModels.Content.ContentTypeFieldModel ReverseMap(ContentTypeField source);
    [MapperIgnoreTarget(nameof(DomainModels.Content.ContentTypeFieldModel.ContentType))]
    public override partial void ReverseMap(ContentTypeField source, DomainModels.Content.ContentTypeFieldModel destination);
}

[Mapper]
[MapExtraProperties]
public partial class LayoutMapper : TwoWayMapperBase<DomainModels.Content.LayoutModel, Layout>
{
    [MapperIgnoreSource(nameof(DomainModels.Content.LayoutModel.TenantId))]
    public override partial Layout Map(DomainModels.Content.LayoutModel source);
    [MapperIgnoreSource(nameof(DomainModels.Content.LayoutModel.TenantId))]
    public override partial void Map(DomainModels.Content.LayoutModel source, Layout destination);
    [MapperIgnoreTarget(nameof(DomainModels.Content.LayoutModel.TenantId))]
    public override partial DomainModels.Content.LayoutModel ReverseMap(Layout source);
    [MapperIgnoreTarget(nameof(DomainModels.Content.LayoutModel.TenantId))]
    public override partial void ReverseMap(Layout source, DomainModels.Content.LayoutModel destination);
}

[Mapper]
[MapExtraProperties]
public partial class PageMapper : TwoWayMapperBase<DomainModels.Content.PageModel, Page>
{
    [MapperIgnoreSource(nameof(DomainModels.Content.PageModel.Parent))]
    [MapperIgnoreSource(nameof(DomainModels.Content.PageModel.Layout))]
    [MapperIgnoreSource(nameof(DomainModels.Content.PageModel.EditLayout))]
    [MapperIgnoreSource(nameof(DomainModels.Content.PageModel.DetailLayout))]
    [MapperIgnoreSource(nameof(DomainModels.Content.PageModel.Children))]
    [MapperIgnoreSource(nameof(DomainModels.Content.PageModel.TenantId))]
    public override partial Page Map(DomainModels.Content.PageModel source);
    [MapperIgnoreSource(nameof(DomainModels.Content.PageModel.Parent))]
    [MapperIgnoreSource(nameof(DomainModels.Content.PageModel.Layout))]
    [MapperIgnoreSource(nameof(DomainModels.Content.PageModel.EditLayout))]
    [MapperIgnoreSource(nameof(DomainModels.Content.PageModel.DetailLayout))]
    [MapperIgnoreSource(nameof(DomainModels.Content.PageModel.Children))]
    [MapperIgnoreSource(nameof(DomainModels.Content.PageModel.TenantId))]
    public override partial void Map(DomainModels.Content.PageModel source, Page destination);
    [MapperIgnoreTarget(nameof(DomainModels.Content.PageModel.Parent))]
    [MapperIgnoreTarget(nameof(DomainModels.Content.PageModel.Layout))]
    [MapperIgnoreTarget(nameof(DomainModels.Content.PageModel.EditLayout))]
    [MapperIgnoreTarget(nameof(DomainModels.Content.PageModel.DetailLayout))]
    [MapperIgnoreTarget(nameof(DomainModels.Content.PageModel.Children))]
    [MapperIgnoreTarget(nameof(DomainModels.Content.PageModel.TenantId))]
    public override partial DomainModels.Content.PageModel ReverseMap(Page source);
    [MapperIgnoreTarget(nameof(DomainModels.Content.PageModel.Parent))]
    [MapperIgnoreTarget(nameof(DomainModels.Content.PageModel.Layout))]
    [MapperIgnoreTarget(nameof(DomainModels.Content.PageModel.EditLayout))]
    [MapperIgnoreTarget(nameof(DomainModels.Content.PageModel.DetailLayout))]
    [MapperIgnoreTarget(nameof(DomainModels.Content.PageModel.Children))]
    [MapperIgnoreTarget(nameof(DomainModels.Content.PageModel.TenantId))]
    public override partial void ReverseMap(Page source, DomainModels.Content.PageModel destination);
}

[Mapper]
public partial class SiteMapper : TwoWayMapperBase<DomainModels.Content.SiteModel, Site>
{
    [MapperIgnoreSource(nameof(DomainModels.Content.SiteModel.TenantId))]
    public override partial Site Map(DomainModels.Content.SiteModel source);
    [MapperIgnoreSource(nameof(DomainModels.Content.SiteModel.TenantId))]
    public override partial void Map(DomainModels.Content.SiteModel source, Site destination);
    [MapperIgnoreTarget(nameof(DomainModels.Content.SiteModel.TenantId))]
    public override partial DomainModels.Content.SiteModel ReverseMap(Site source);
    [MapperIgnoreTarget(nameof(DomainModels.Content.SiteModel.TenantId))]
    public override partial void ReverseMap(Site source, DomainModels.Content.SiteModel destination);
}

[Mapper]
[MapExtraProperties]
public partial class FileMapper : TwoWayMapperBase<DomainModels.IO.FileModel, CmsFile>
{
    [MapperIgnoreSource(nameof(DomainModels.IO.FileModel.Folder))]
    [MapperIgnoreSource(nameof(DomainModels.IO.FileModel.TenantId))]
    public override partial CmsFile Map(DomainModels.IO.FileModel source);
    [MapperIgnoreSource(nameof(DomainModels.IO.FileModel.Folder))]
    [MapperIgnoreSource(nameof(DomainModels.IO.FileModel.TenantId))]
    public override partial void Map(DomainModels.IO.FileModel source, CmsFile destination);
    [MapperIgnoreTarget(nameof(DomainModels.IO.FileModel.Folder))]
    [MapperIgnoreTarget(nameof(DomainModels.IO.FileModel.TenantId))]
    public override partial DomainModels.IO.FileModel ReverseMap(CmsFile source);
    [MapperIgnoreTarget(nameof(DomainModels.IO.FileModel.Folder))]
    [MapperIgnoreTarget(nameof(DomainModels.IO.FileModel.TenantId))]
    public override partial void ReverseMap(CmsFile source, DomainModels.IO.FileModel destination);
}

[Mapper]
[MapExtraProperties]
public partial class FolderMapper : TwoWayMapperBase<DomainModels.IO.FolderModel, Folder>
{
    [MapperIgnoreSource(nameof(DomainModels.IO.FolderModel.Parent))]
    [MapperIgnoreSource(nameof(DomainModels.IO.FolderModel.ChildFolders))]
    [MapperIgnoreSource(nameof(DomainModels.IO.FolderModel.ChildFiles))]
    [MapperIgnoreSource(nameof(DomainModels.IO.FolderModel.TenantId))]
    public override partial Folder Map(DomainModels.IO.FolderModel source);
    [MapperIgnoreSource(nameof(DomainModels.IO.FolderModel.Parent))]
    [MapperIgnoreSource(nameof(DomainModels.IO.FolderModel.ChildFolders))]
    [MapperIgnoreSource(nameof(DomainModels.IO.FolderModel.ChildFiles))]
    [MapperIgnoreSource(nameof(DomainModels.IO.FolderModel.TenantId))]
    public override partial void Map(DomainModels.IO.FolderModel source, Folder destination);
    [MapperIgnoreTarget(nameof(DomainModels.IO.FolderModel.Parent))]
    [MapperIgnoreTarget(nameof(DomainModels.IO.FolderModel.ChildFolders))]
    [MapperIgnoreTarget(nameof(DomainModels.IO.FolderModel.ChildFiles))]
    [MapperIgnoreTarget(nameof(DomainModels.IO.FolderModel.TenantId))]
    public override partial DomainModels.IO.FolderModel ReverseMap(Folder source);
    [MapperIgnoreTarget(nameof(DomainModels.IO.FolderModel.Parent))]
    [MapperIgnoreTarget(nameof(DomainModels.IO.FolderModel.ChildFolders))]
    [MapperIgnoreTarget(nameof(DomainModels.IO.FolderModel.ChildFiles))]
    [MapperIgnoreTarget(nameof(DomainModels.IO.FolderModel.TenantId))]
    public override partial void ReverseMap(Folder source, DomainModels.IO.FolderModel destination);
}

[Mapper]
[MapExtraProperties]
public partial class RoleMapper : TwoWayMapperBase<DomainModels.Membership.RoleModel, Role>
{
    [MapperIgnoreSource(nameof(DomainModels.Membership.RoleModel.TenantId))]
    public override partial Role Map(DomainModels.Membership.RoleModel source);
    [MapperIgnoreSource(nameof(DomainModels.Membership.RoleModel.TenantId))]
    public override partial void Map(DomainModels.Membership.RoleModel source, Role destination);
    [MapperIgnoreTarget(nameof(DomainModels.Membership.RoleModel.TenantId))]
    public override partial DomainModels.Membership.RoleModel ReverseMap(Role source);
    [MapperIgnoreTarget(nameof(DomainModels.Membership.RoleModel.TenantId))]
    public override partial void ReverseMap(Role source, DomainModels.Membership.RoleModel destination);
}

[Mapper]
public partial class UserMapper : TwoWayMapperBase<DomainModels.Membership.UserModel, User>
{
    [MapperIgnoreSource(nameof(DomainModels.Membership.UserModel.TenantId))]
    [MapperIgnoreSource(nameof(DomainModels.Membership.UserModel.Name))]
    [MapperIgnoreSource(nameof(DomainModels.Membership.UserModel.Surname))]
    [MapperIgnoreSource(nameof(DomainModels.Membership.UserModel.IsActive))]
    [MapperIgnoreSource(nameof(DomainModels.Membership.UserModel.ExtraProperties))]
    [MapperIgnoreTarget(nameof(User.FirstName))]
    [MapperIgnoreTarget(nameof(User.LastName))]
    public override partial User Map(DomainModels.Membership.UserModel source);
    [MapperIgnoreSource(nameof(DomainModels.Membership.UserModel.TenantId))]
    [MapperIgnoreSource(nameof(DomainModels.Membership.UserModel.Name))]
    [MapperIgnoreSource(nameof(DomainModels.Membership.UserModel.Surname))]
    [MapperIgnoreSource(nameof(DomainModels.Membership.UserModel.IsActive))]
    [MapperIgnoreSource(nameof(DomainModels.Membership.UserModel.ExtraProperties))]
    [MapperIgnoreTarget(nameof(User.FirstName))]
    [MapperIgnoreTarget(nameof(User.LastName))]
    public override partial void Map(DomainModels.Membership.UserModel source, User destination);
    [MapperIgnoreSource(nameof(User.FirstName))]
    [MapperIgnoreSource(nameof(User.LastName))]
    [MapperIgnoreTarget(nameof(DomainModels.Membership.UserModel.TenantId))]
    [MapperIgnoreTarget(nameof(DomainModels.Membership.UserModel.Name))]
    [MapperIgnoreTarget(nameof(DomainModels.Membership.UserModel.Surname))]
    [MapperIgnoreTarget(nameof(DomainModels.Membership.UserModel.IsActive))]
    public override partial DomainModels.Membership.UserModel ReverseMap(User source);
    [MapperIgnoreSource(nameof(User.FirstName))]
    [MapperIgnoreSource(nameof(User.LastName))]
    [MapperIgnoreTarget(nameof(DomainModels.Membership.UserModel.TenantId))]
    [MapperIgnoreTarget(nameof(DomainModels.Membership.UserModel.Name))]
    [MapperIgnoreTarget(nameof(DomainModels.Membership.UserModel.Surname))]
    [MapperIgnoreTarget(nameof(DomainModels.Membership.UserModel.IsActive))]
    public override partial void ReverseMap(User source, DomainModels.Membership.UserModel destination);
}

[MapExtraProperties]
[Mapper]
public partial class PluginMapper : TwoWayMapperBase<DomainModels.Plugins.PluginModel, Plugin>
{
    [MapperIgnoreSource(nameof(DomainModels.Plugins.PluginModel.TenantId))]
    public override partial Plugin Map(DomainModels.Plugins.PluginModel source);
    [MapperIgnoreSource(nameof(DomainModels.Plugins.PluginModel.TenantId))]
    public override partial void Map(DomainModels.Plugins.PluginModel source, Plugin destination);
    [MapperIgnoreTarget(nameof(DomainModels.Plugins.PluginModel.TenantId))]
    public override partial DomainModels.Plugins.PluginModel ReverseMap(Plugin source);
    [MapperIgnoreTarget(nameof(DomainModels.Plugins.PluginModel.TenantId))]
    public override partial void ReverseMap(Plugin source, DomainModels.Plugins.PluginModel destination);
}

[MapExtraProperties]
[Mapper]
public partial class PluginContentMapper : TwoWayMapperBase<DomainModels.Plugins.PluginContentModel, PluginContent>
{
    [MapperIgnoreSource(nameof(DomainModels.Plugins.PluginContentModel.TenantId))]
    public override partial PluginContent Map(DomainModels.Plugins.PluginContentModel source);
    [MapperIgnoreSource(nameof(DomainModels.Plugins.PluginContentModel.TenantId))]
    public override partial void Map(DomainModels.Plugins.PluginContentModel source, PluginContent destination);
    [MapperIgnoreTarget(nameof(DomainModels.Plugins.PluginContentModel.TenantId))]
    public override partial DomainModels.Plugins.PluginContentModel ReverseMap(PluginContent source);
    [MapperIgnoreTarget(nameof(DomainModels.Plugins.PluginContentModel.TenantId))]
    public override partial void ReverseMap(PluginContent source, DomainModels.Plugins.PluginContentModel destination);
}

[Mapper]
public partial class GlobalSettingsMapper : TwoWayMapperBase<DomainModels.Settings.GlobalSettingsModel, GlobalSettings>
{
    public override partial GlobalSettings Map(DomainModels.Settings.GlobalSettingsModel source);
    public override partial void Map(DomainModels.Settings.GlobalSettingsModel source, GlobalSettings destination);
    public override partial DomainModels.Settings.GlobalSettingsModel ReverseMap(GlobalSettings source);
    public override partial void ReverseMap(GlobalSettings source, DomainModels.Settings.GlobalSettingsModel destination);
}

[Mapper]
public partial class SettingsMapper : TwoWayMapperBase<DomainModels.Settings.SettingsModel, SettingsDto>
{
    [MapperIgnoreSource(nameof(DomainModels.Settings.SettingsModel.TenantId))]
    [MapperIgnoreSource(nameof(DomainModels.Settings.SettingsModel.Values))]
    public override partial SettingsDto Map(DomainModels.Settings.SettingsModel source);
    [MapperIgnoreSource(nameof(DomainModels.Settings.SettingsModel.TenantId))]
    [MapperIgnoreSource(nameof(DomainModels.Settings.SettingsModel.Values))]
    public override partial void Map(DomainModels.Settings.SettingsModel source, SettingsDto destination);
    [MapperIgnoreTarget(nameof(DomainModels.Settings.SettingsModel.TenantId))]
    [MapperIgnoreTarget(nameof(DomainModels.Settings.SettingsModel.Values))]
    public override partial DomainModels.Settings.SettingsModel ReverseMap(SettingsDto source);
    [MapperIgnoreTarget(nameof(DomainModels.Settings.SettingsModel.TenantId))]
    [MapperIgnoreTarget(nameof(DomainModels.Settings.SettingsModel.Values))]
    public override partial void ReverseMap(SettingsDto source, DomainModels.Settings.SettingsModel destination);
}


