using ByteLabs.Foundations;
using ByteLabs.Foundations.Application.Dtos;
using FluentCMS.Content;
using FluentCMS.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ContentDto = FluentCMS.Content.Content;

namespace FluentCMS.Api.Controllers.Content;

[Controller]
[RemoteService(Name = FluentCMSRemoteServiceConsts.RemoteServiceName)]
[Area(FluentCMSRemoteServiceConsts.ModuleAreaName)]
[Route($"api/{FluentCMSRemoteServiceConsts.RoutePath}/content/contents")]
[Authorize(FluentCMSPermissions.SiteContent.Content.Default)]
public class ContentController : FluentCMSBaseController, IContentAppService
{
    private readonly IContentAppService _contentAppService;

    public ContentController(IContentAppService contentAppService) => _contentAppService = contentAppService;

    [HttpPost]
    [Authorize(FluentCMSPermissions.SiteContent.Content.Create)]
    public Task<ContentDto> CreateAsync(ContentCreateRequest input) => _contentAppService.CreateAsync(input);

    [HttpDelete("{id}")]
    [Authorize(FluentCMSPermissions.SiteContent.Content.Delete)]
    public Task DeleteAsync(Guid id) => _contentAppService.DeleteAsync(id);

    [HttpGet("{id}")]
    public Task<ContentDto> GetAsync(Guid id) => _contentAppService.GetAsync(id);

    [HttpGet]
    public Task<PagedResultDto<ContentDto>> GetListAsync(GetContentRequest input) => _contentAppService.GetListAsync(input);

    [HttpGet("by-content-type/{contentTypeId}")]
    public IAsyncEnumerable<ContentDto> GetAllByContentType(Guid contentTypeId, CancellationToken cancellationToken = default) =>
        _contentAppService.GetAllByContentType(contentTypeId, cancellationToken);

    [HttpPut("{id}")]
    [Authorize(FluentCMSPermissions.SiteContent.Content.Edit)]
    public Task<ContentDto> UpdateAsync(Guid id, ContentUpdateRequest input) => _contentAppService.UpdateAsync(id, input);
}
