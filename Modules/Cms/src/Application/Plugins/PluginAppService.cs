using FluentCMS.Domain.Plugins;
using FluentCMS.Plugins;
using FluentCMS.Permissions;
using Microsoft.AspNetCore.Authorization;
using System;

namespace FluentCMS.Services.Plugins;

[Authorize(FluentCMSPermissions.Plugins.Plugin.Default)]
public class PluginAppService : FluentCMSCrudAppService<PluginModel, Plugin, Guid, GetPluginRequest, PluginCreateRequest, PluginUpdateRequest>, IPluginAppService
{
    public PluginAppService(IPluginModelRepository repository) : base(repository)
    {
        CreatePolicyName = FluentCMSPermissions.Plugins.Plugin.Create;
        UpdatePolicyName = FluentCMSPermissions.Plugins.Plugin.Edit;
        DeletePolicyName = FluentCMSPermissions.Plugins.Plugin.Delete;
    }
}
