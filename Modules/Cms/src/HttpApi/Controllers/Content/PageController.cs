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
[Route($"api/{FluentCMSRemoteServiceConsts.RoutePath}/content/pages")]
[Authorize(FluentCMSPermissions.SiteContent.Pages.Default)]
public class PageController : FluentCMSBaseController, IPageAppService
{
    private readonly IPageAppService _pageAppService;

    public PageController(IPageAppService pageAppService) => _pageAppService = pageAppService;

    [HttpPost]
    [Authorize(FluentCMSPermissions.SiteContent.Pages.Create)]
    public Task<Page> CreateAsync(PageCreateRequest input) => _pageAppService.CreateAsync(input);

    [HttpDelete("{id}")]
    [Authorize(FluentCMSPermissions.SiteContent.Pages.Delete)]
    public Task DeleteAsync(Guid id) => _pageAppService.DeleteAsync(id);

    [HttpGet("{id}")]
    public Task<Page> GetAsync(Guid id) => _pageAppService.GetAsync(id);

    [HttpGet]
    public Task<PagedResultDto<Page>> GetListAsync(GetPageRequest input) => _pageAppService.GetListAsync(input);

    [HttpPut("{id}")]
    [Authorize(FluentCMSPermissions.SiteContent.Pages.Edit)]
    public Task<Page> UpdateAsync(Guid id, PageUpdateRequest input) => _pageAppService.UpdateAsync(id, input);
}
