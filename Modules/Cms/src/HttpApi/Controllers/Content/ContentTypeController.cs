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
[Route($"api/{FluentCMSRemoteServiceConsts.RoutePath}/content/content-types")]
[Authorize(FluentCMSPermissions.SiteContent.ContentTypes.Default)]
public class ContentTypeController : FluentCMSBaseController, IContentTypeAppService
{
    private readonly IContentTypeAppService _contentTypeAppService;

    public ContentTypeController(IContentTypeAppService contentTypeAppService) => _contentTypeAppService = contentTypeAppService;

    [HttpPost]
    [Authorize(FluentCMSPermissions.SiteContent.ContentTypes.Create)]
    public Task<ContentType> CreateAsync(ContentTypeCreateRequest input) => _contentTypeAppService.CreateAsync(input);

    [HttpDelete("{id}")]
    [Authorize(FluentCMSPermissions.SiteContent.ContentTypes.Delete)]
    public Task DeleteAsync(Guid id) => _contentTypeAppService.DeleteAsync(id);

    [HttpGet("{id}")]
    public Task<ContentType> GetAsync(Guid id) => _contentTypeAppService.GetAsync(id);

    [HttpGet]
    public Task<PagedResultDto<ContentType>> GetListAsync(GetContentTypeRequest input) => _contentTypeAppService.GetListAsync(input);

    [HttpPut("{id}")]
    [Authorize(FluentCMSPermissions.SiteContent.ContentTypes.Edit)]
    public Task<ContentType> UpdateAsync(Guid id, ContentTypeUpdateRequest input) => _contentTypeAppService.UpdateAsync(id, input);
}
