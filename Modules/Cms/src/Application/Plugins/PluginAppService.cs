using FluentCMS.Domain.Plugins;
using FluentCMS.Plugins;
using FluentCMS.Permissions;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Threading.Tasks;
using System.Threading;
using ByteLabs.Foundations.Application.Dtos;

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

    [Authorize(FluentCMSPermissions.Plugins.Plugin.Create)]
    public Task<Plugin> InitialCreateAsync(InitialCreateRequest body, CancellationToken cancellationToken = default) => throw new NotImplementedException();

    [Authorize(FluentCMSPermissions.Plugins.Plugin.Edit)]
    public Task<Plugin> UpdateColsAsync(Guid id, UpdateColsRequest body, CancellationToken cancellationToken = default) => throw new NotImplementedException();

    [Authorize(FluentCMSPermissions.Plugins.Plugin.Edit)]
    public Task<PagedResultDto<Plugin>> UpdateOrdersAsync(UpdateOrdersRequest body, CancellationToken cancellationToken = default) => throw new NotImplementedException();
}
