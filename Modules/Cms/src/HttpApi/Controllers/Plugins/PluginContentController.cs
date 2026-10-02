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
[Route($"api/{FluentCMSRemoteServiceConsts.RoutePath}/plugins/plugin-contents")]
[Authorize(FluentCMSPermissions.Plugins.PluginContents.Default)]
public class PluginContentController : FluentCMSBaseController, IPluginContentAppService
{
    private readonly IPluginContentAppService _pluginContentAppService;

    public PluginContentController(IPluginContentAppService pluginContentAppService) => _pluginContentAppService = pluginContentAppService;

    [HttpPost]
    [Authorize(FluentCMSPermissions.Plugins.PluginContents.Create)]
    public Task<PluginContent> CreateAsync(PluginContentCreateRequest input) => _pluginContentAppService.CreateAsync(input);

    [HttpDelete("{id}")]
    [Authorize(FluentCMSPermissions.Plugins.PluginContents.Delete)]
    public Task DeleteAsync(Guid id) => _pluginContentAppService.DeleteAsync(id);

    [HttpGet("{id}")]
    public Task<PluginContent> GetAsync(Guid id) => _pluginContentAppService.GetAsync(id);

    [HttpGet]
    public Task<PagedResultDto<PluginContent>> GetListAsync(GetPluginContentRequest input) => _pluginContentAppService.GetListAsync(input);

    [HttpPut("{id}")]
    [Authorize(FluentCMSPermissions.Plugins.PluginContents.Edit)]
    public Task<PluginContent> UpdateAsync(Guid id, PluginContentUpdateRequest input) => _pluginContentAppService.UpdateAsync(id, input);
}
