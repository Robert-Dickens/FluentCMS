using FluentCMS.Domain.Content;
using Microsoft.AspNetCore.Authorization;
using System;

namespace FluentCMS.Services.Content;

[Authorize]
public class ContentTypeFieldAppService : FluentCMSCrudAppService<ContentTypeFieldModel, FluentCMS.Content.ContentTypeField, Guid, GetContentTypeFieldRequest, ContentTypeFieldCreateRequest, ContentTypeFieldUpdateRequest>, IContentTypeFieldAppService
{
    public ContentTypeFieldAppService(IContentTypeFieldModelRepository repository) : base(repository)
    {
    }
}
