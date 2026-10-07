using FluentCMS.Content;
using FluentCMS.Domain.Content;
using FluentCMS.Permissions;
using Microsoft.AspNetCore.Authorization;
using System;
using PageModel = FluentCMS.Domain.Content.PageModel;

namespace FluentCMS.Services.Content;

[Authorize(FluentCMSPermissions.SiteContent.Pages.Default)]
public class PageAppService : FluentCMSCrudAppService<PageModel, Page, Guid, GetPageRequest, PageCreateRequest, PageUpdateRequest>, IPageAppService
{
    public PageAppService(IPageModelRepository repository) : base(repository)
    {
        CreatePolicyName = FluentCMSPermissions.SiteContent.Pages.Create;
        UpdatePolicyName = FluentCMSPermissions.SiteContent.Pages.Edit;
        DeletePolicyName = FluentCMSPermissions.SiteContent.Pages.Delete;
    }
}
