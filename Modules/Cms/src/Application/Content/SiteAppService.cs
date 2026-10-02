using FluentCMS.Content;
using FluentCMS.Domain.Content;
using FluentCMS.Permissions;
using Microsoft.AspNetCore.Authorization;
using System;

namespace FluentCMS.Services.Content;

[Authorize(FluentCMSPermissions.SiteContent.Sites.Default)]
public class SiteAppService : FluentCMSCrudAppService<SiteModel, Site, Guid, GetSiteRequest, SiteCreateRequest, SiteUpdateRequest>, ISiteAppService
{
    public SiteAppService(ISiteModelRepository repository) : base(repository)
    {
        CreatePolicyName = FluentCMSPermissions.SiteContent.Sites.Create;
        UpdatePolicyName = FluentCMSPermissions.SiteContent.Sites.Edit;
        DeletePolicyName = FluentCMSPermissions.SiteContent.Sites.Delete;
    }
}
