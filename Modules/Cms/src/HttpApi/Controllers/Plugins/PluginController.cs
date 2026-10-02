using ByteLabs.Foundations;
using ByteLabs.Foundations.Application.Dtos;
using FluentCMS.Plugins;
using FluentCMS.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace FluentCMS.Api.Controllers.Plugins;

[Controller]
[RemoteService(Name = FluentCMSRemoteServiceConsts.RemoteServiceName)]
[Area(FluentCMSRemoteServiceConsts.ModuleAreaName)]
[Route($"api/{FluentCMSRemoteServiceConsts.RoutePath}/plugins/plugins")]
[Authorize(FluentCMSPermissions.Plugins.Plugin.Default)]
public class PluginController : FluentCMSBaseController, IPluginAppService
{
    private readonly IPluginAppService _pluginAppService;

    public PluginController(IPluginAppService pluginAppService) => _pluginAppService = pluginAppService;

    [HttpPost]
    [Authorize(FluentCMSPermissions.Plugins.Plugin.Create)]
    public Task<Plugin> CreateAsync(PluginCreateRequest input) => _pluginAppService.CreateAsync(input);

    [HttpDelete("{id}")]
    [Authorize(FluentCMSPermissions.Plugins.Plugin.Delete)]
    public Task DeleteAsync(Guid id) => _pluginAppService.DeleteAsync(id);

    [HttpGet("{id}")]
    public Task<Plugin> GetAsync(Guid id) => _pluginAppService.GetAsync(id);

    [HttpGet]
    public Task<PagedResultDto<Plugin>> GetListAsync(GetPluginRequest input) => _pluginAppService.GetListAsync(input);

    [HttpPut("{id}")]
    [Authorize(FluentCMSPermissions.Plugins.Plugin.Edit)]
    public Task<Plugin> UpdateAsync(Guid id, PluginUpdateRequest input) => _pluginAppService.UpdateAsync(id, input);
}
