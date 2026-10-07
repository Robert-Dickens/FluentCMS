using FluentCMS.Domain.Plugins;
using FluentCMS.Plugins;
using FluentCMS.Permissions;
using Microsoft.AspNetCore.Authorization;
using System;

namespace FluentCMS.Services.Plugins;

[Authorize(FluentCMSPermissions.Plugins.PluginDefinitions.Default)]
public class PluginDefinitionAppService : FluentCMSCrudAppService<PluginDefinitionModel, PluginDefinition, Guid, GetPluginDefinitionRequest, PluginDefinitionCreateRequest, PluginDefinitionUpdateRequest>, IPluginDefinitionAppService
{
    public PluginDefinitionAppService(IPluginDefinitionModelRepository repository) : base(repository)
    {
        CreatePolicyName = FluentCMSPermissions.Plugins.PluginDefinitions.Create;
        UpdatePolicyName = FluentCMSPermissions.Plugins.PluginDefinitions.Edit;
        DeletePolicyName = FluentCMSPermissions.Plugins.PluginDefinitions.Delete;

    }
}