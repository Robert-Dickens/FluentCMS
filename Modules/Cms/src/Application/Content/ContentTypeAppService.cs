using FluentCMS.Content;
using FluentCMS.Domain.Content;
using FluentCMS.Permissions;
using Microsoft.AspNetCore.Authorization;
using System;

namespace FluentCMS.Services.Content;

[Authorize(FluentCMSPermissions.SiteContent.ContentTypes.Default)]
public class ContentTypeAppService : FluentCMSCrudAppService<ContentTypeModel, ContentType, Guid, GetContentTypeRequest, ContentTypeCreateRequest, ContentTypeUpdateRequest>, IContentTypeAppService
{
    public ContentTypeAppService(IContentTypeModelRepository repository) : base(repository)
    {
        CreatePolicyName = FluentCMSPermissions.SiteContent.ContentTypes.Create;
        UpdatePolicyName = FluentCMSPermissions.SiteContent.ContentTypes.Edit;
        DeletePolicyName = FluentCMSPermissions.SiteContent.ContentTypes.Delete;
    }
}
