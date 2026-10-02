using FluentCMS.Content;
using FluentCMS.Domain.Content;
using FluentCMS.Permissions;
using Microsoft.AspNetCore.Authorization;
using System;

namespace FluentCMS.Services.Content;

[Authorize(FluentCMSPermissions.SiteContent.ContentTypeFields.Default)]
public class ContentTypeFieldAppService : FluentCMSCrudAppService<ContentTypeFieldModel, ContentTypeField, Guid, GetContentTypeFieldRequest, ContentTypeFieldCreateRequest, ContentTypeFieldUpdateRequest>, IContentTypeFieldAppService
{
    public ContentTypeFieldAppService(IContentTypeFieldModelRepository repository) : base(repository)
    {
        CreatePolicyName = FluentCMSPermissions.SiteContent.ContentTypeFields.Create;
        UpdatePolicyName = FluentCMSPermissions.SiteContent.ContentTypeFields.Edit;
        DeletePolicyName = FluentCMSPermissions.SiteContent.ContentTypeFields.Delete;
    }
}
