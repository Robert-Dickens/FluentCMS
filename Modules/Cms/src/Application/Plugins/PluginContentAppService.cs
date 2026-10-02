using FluentCMS.Domain.Plugins;
using FluentCMS.Plugins;
using FluentCMS.Permissions;
using Microsoft.AspNetCore.Authorization;
using System;

namespace FluentCMS.Services.Plugins;

[Authorize(FluentCMSPermissions.Plugins.PluginContents.Default)]
public class PluginContentAppService : FluentCMSCrudAppService<PluginContentModel, PluginContent, Guid, GetPluginContentRequest, PluginContentCreateRequest, PluginContentUpdateRequest>, IPluginContentAppService
{
    public PluginContentAppService(IPluginContentModelRepository repository) : base(repository)
    {
        CreatePolicyName = FluentCMSPermissions.Plugins.PluginContents.Create;
        UpdatePolicyName = FluentCMSPermissions.Plugins.PluginContents.Edit;
        DeletePolicyName = FluentCMSPermissions.Plugins.PluginContents.Delete;
    }
}
