using FluentCMS.Content;
using FluentCMS.Domain.Content;
using FluentCMS.Permissions;
using Microsoft.AspNetCore.Authorization;
using System;

namespace FluentCMS.Services.Content;

[Authorize(FluentCMSPermissions.SiteContent.Layouts.Default)]
public class LayoutAppService : FluentCMSCrudAppService<LayoutModel, Layout, Guid, GetLayoutRequest, LayoutCreateRequest, LayoutUpdateRequest>, ILayoutAppService
{
    public LayoutAppService(ILayoutModelRepository repository) : base(repository)
    {
        CreatePolicyName = FluentCMSPermissions.SiteContent.Layouts.Create;
        UpdatePolicyName = FluentCMSPermissions.SiteContent.Layouts.Edit;
        DeletePolicyName = FluentCMSPermissions.SiteContent.Layouts.Delete;
    }
}
