using ByteLabs.Foundations;
using ByteLabs.Foundations.Application.Dtos;
using FluentCMS.Content;
using FluentCMS.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace FluentCMS.Api.Controllers.Content;

[Controller]
[RemoteService(Name = FluentCMSRemoteServiceConsts.RemoteServiceName)]
[Area(FluentCMSRemoteServiceConsts.ModuleAreaName)]
[Route($"api/{FluentCMSRemoteServiceConsts.RoutePath}/content/layouts")]
[Authorize(FluentCMSPermissions.SiteContent.Layouts.Default)]
public class LayoutController : FluentCMSBaseController, ILayoutAppService
{
    private readonly ILayoutAppService _layoutAppService;

    public LayoutController(ILayoutAppService layoutAppService) => _layoutAppService = layoutAppService;

    [HttpPost]
    [Authorize(FluentCMSPermissions.SiteContent.Layouts.Create)]
    public Task<Layout> CreateAsync(LayoutCreateRequest input) => _layoutAppService.CreateAsync(input);

    [HttpDelete("{id}")]
    [Authorize(FluentCMSPermissions.SiteContent.Layouts.Delete)]
    public Task DeleteAsync(Guid id) => _layoutAppService.DeleteAsync(id);

    [HttpGet("{id}")]
    public Task<Layout> GetAsync(Guid id) => _layoutAppService.GetAsync(id);

    [HttpGet]
    public Task<PagedResultDto<Layout>> GetListAsync(GetLayoutRequest input) => _layoutAppService.GetListAsync(input);

    [HttpPut("{id}")]
    [Authorize(FluentCMSPermissions.SiteContent.Layouts.Edit)]
    public Task<Layout> UpdateAsync(Guid id, LayoutUpdateRequest input) => _layoutAppService.UpdateAsync(id, input);
}
