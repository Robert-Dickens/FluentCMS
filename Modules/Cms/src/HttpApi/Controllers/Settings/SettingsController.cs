using ByteLabs.Foundations;
using ByteLabs.Foundations.Application.Dtos;
using FluentCMS.Permissions;
using FluentCMS.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using SettingsDto = FluentCMS.Settings.Settings;

namespace FluentCMS.Api.Controllers.Settings;

[Controller]
[RemoteService(Name = FluentCMSRemoteServiceConsts.RemoteServiceName)]
[Area(FluentCMSRemoteServiceConsts.ModuleAreaName)]
[Route($"api/{FluentCMSRemoteServiceConsts.RoutePath}/settings/tenant-settings")]
[Authorize(FluentCMSPermissions.Settings.TenantSettings.Default)]
public class SettingsController : FluentCMSBaseController, ISettingsAppService
{
    private readonly ISettingsAppService _settingsAppService;

    public SettingsController(ISettingsAppService settingsAppService) => _settingsAppService = settingsAppService;

    [HttpPost]
    [Authorize(FluentCMSPermissions.Settings.TenantSettings.Create)]
    public Task<SettingsDto> CreateAsync(SettingsCreateRequest input) => _settingsAppService.CreateAsync(input);

    [HttpDelete("{id}")]
    [Authorize(FluentCMSPermissions.Settings.TenantSettings.Delete)]
    public Task DeleteAsync(Guid id) => _settingsAppService.DeleteAsync(id);

    [HttpGet("{id}")]
    public Task<SettingsDto> GetAsync(Guid id) => _settingsAppService.GetAsync(id);

    [HttpGet]
    public Task<PagedResultDto<SettingsDto>> GetListAsync(GetSettingsRequest input) => _settingsAppService.GetListAsync(input);

    [HttpPut("{id}")]
    [Authorize(FluentCMSPermissions.Settings.TenantSettings.Edit)]
    public Task<SettingsDto> UpdateAsync(Guid id, SettingsUpdateRequest input) => _settingsAppService.UpdateAsync(id, input);
}
