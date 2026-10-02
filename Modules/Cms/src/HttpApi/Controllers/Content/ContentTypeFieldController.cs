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
[Route($"api/{FluentCMSRemoteServiceConsts.RoutePath}/content/content-type-fields")]
[Authorize(FluentCMSPermissions.SiteContent.ContentTypeFields.Default)]
public class ContentTypeFieldController : FluentCMSBaseController, IContentTypeFieldAppService
{
    private readonly IContentTypeFieldAppService _contentTypeFieldAppService;

    public ContentTypeFieldController(IContentTypeFieldAppService contentTypeFieldAppService) => _contentTypeFieldAppService = contentTypeFieldAppService;

    [HttpPost]
    [Authorize(FluentCMSPermissions.SiteContent.ContentTypeFields.Create)]
    public Task<ContentTypeField> CreateAsync(ContentTypeFieldCreateRequest input) => _contentTypeFieldAppService.CreateAsync(input);

    [HttpDelete("{id}")]
    [Authorize(FluentCMSPermissions.SiteContent.ContentTypeFields.Delete)]
    public Task DeleteAsync(Guid id) => _contentTypeFieldAppService.DeleteAsync(id);

    [HttpGet("{id}")]
    public Task<ContentTypeField> GetAsync(Guid id) => _contentTypeFieldAppService.GetAsync(id);

    [HttpGet]
    public Task<PagedResultDto<ContentTypeField>> GetListAsync(GetContentTypeFieldRequest input) => _contentTypeFieldAppService.GetListAsync(input);

    [HttpPut("{id}")]
    [Authorize(FluentCMSPermissions.SiteContent.ContentTypeFields.Edit)]
    public Task<ContentTypeField> UpdateAsync(Guid id, ContentTypeFieldUpdateRequest input) => _contentTypeFieldAppService.UpdateAsync(id, input);
}
