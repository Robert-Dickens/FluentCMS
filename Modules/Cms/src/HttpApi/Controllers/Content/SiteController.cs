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
[Route($"api/{FluentCMSRemoteServiceConsts.RoutePath}/content/sites")]
[Authorize(FluentCMSPermissions.SiteContent.Sites.Default)]
public class SiteController : FluentCMSBaseController, ISiteAppService
{
    private readonly ISiteAppService _siteAppService;

    public SiteController(ISiteAppService siteAppService) => _siteAppService = siteAppService;

    [HttpPost]
    [Authorize(FluentCMSPermissions.SiteContent.Sites.Create)]
    public Task<Site> CreateAsync(SiteCreateRequest input) => _siteAppService.CreateAsync(input);

    [HttpDelete("{id}")]
    [Authorize(FluentCMSPermissions.SiteContent.Sites.Delete)]
    public Task DeleteAsync(Guid id) => _siteAppService.DeleteAsync(id);

    [HttpGet("{id}")]
    public Task<Site> GetAsync(Guid id) => _siteAppService.GetAsync(id);

    [HttpGet]
    public Task<PagedResultDto<Site>> GetListAsync(GetSiteRequest input) => _siteAppService.GetListAsync(input);

    [HttpPut("{id}")]
    [Authorize(FluentCMSPermissions.SiteContent.Sites.Edit)]
    public Task<Site> UpdateAsync(Guid id, SiteUpdateRequest input) => _siteAppService.UpdateAsync(id, input);
}
