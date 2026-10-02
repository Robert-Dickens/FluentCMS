using ByteLabs.Foundations;
using ByteLabs.Foundations.Application.Dtos;
using FluentCMS.Permissions;
using FluentCMS.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCMS.Api.Controllers.Settings;

[Controller]
[RemoteService(Name = FluentCMSRemoteServiceConsts.RemoteServiceName)]
[Area(FluentCMSRemoteServiceConsts.ModuleAreaName)]
[Route($"api/{FluentCMSRemoteServiceConsts.RoutePath}/settings/global-settings")]
[Authorize(FluentCMSPermissions.Settings.GlobalSettings.Default)]
public class GlobalSettingsController : FluentCMSBaseController, IGlobalSettingsAppService
{
    private readonly IGlobalSettingsAppService _globalSettingsAppService;

    public GlobalSettingsController(IGlobalSettingsAppService globalSettingsAppService) => _globalSettingsAppService = globalSettingsAppService;

    [HttpPost]
    [Authorize(FluentCMSPermissions.Settings.GlobalSettings.Create)]
    public Task<GlobalSettings> CreateAsync(GlobalSettingsCreateRequest input) => _globalSettingsAppService.CreateAsync(input);

    [HttpDelete("{id}")]
    [Authorize(FluentCMSPermissions.Settings.GlobalSettings.Delete)]
    public Task DeleteAsync(Guid id) => _globalSettingsAppService.DeleteAsync(id);

    [HttpGet("current")]
    public Task<GlobalSettings?> Get(CancellationToken cancellationToken = default) => _globalSettingsAppService.Get(cancellationToken);

    [HttpGet("{id}")]
    public Task<GlobalSettings> GetAsync(Guid id) => _globalSettingsAppService.GetAsync(id);

    [HttpGet]
    public Task<PagedResultDto<GlobalSettings>> GetListAsync(GetGlobalSettingsRequest input) => _globalSettingsAppService.GetListAsync(input);

    [HttpPut("{id}")]
    [Authorize(FluentCMSPermissions.Settings.GlobalSettings.Edit)]
    public Task<GlobalSettings> UpdateAsync(Guid id, GlobalSettingsUpdateRequest input) => _globalSettingsAppService.UpdateAsync(id, input);
}
