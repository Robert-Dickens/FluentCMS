using FluentCMS.Membership;
using FluentCMS.Plugins;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCMS.Content;

public interface IPageAppService : ICrudAppService<Page, Guid, GetPageRequest, PageCreateRequest, PageUpdateRequest>
{
}

public interface IPageService : IApplicationService
{
    Task<IEnumerable<PageModel>> GetBySiteId(Guid siteId, CancellationToken cancellationToken = default);
    Task<IEnumerable<PageModel>> GetHierarchyBySiteId(Guid siteId, CancellationToken cancellationToken = default);
    Task<Page> GetById(Guid id, CancellationToken cancellationToken = default);
    Task<PageModel> GetByFullPath(Guid siteId, string fullPath, CancellationToken cancellationToken = default);
    Task<PageModel> Create(Page page, CancellationToken cancellationToken = default);
    Task<PageModel> Update(Page page, CancellationToken cancellationToken = default);
    Task<Page> Delete(Guid id, CancellationToken cancellationToken = default);
}


public class Page : ExtensibleFullAuditedEntityDto<Guid>
{
    public Guid SiteId { get; set; }
    public string Title { get; set; } = string.Empty;
    public Guid? ParentId { get; set; }
    public int Order { get; set; }
    public string Path { get; set; } = string.Empty;
    public Guid? LayoutId { get; set; }
    public Guid? EditLayoutId { get; set; }
    public Guid? DetailLayoutId { get; set; }
    public bool Locked { get; set; }
}

public class PageModel : Page
{
    public string FullPath { get; set; } = string.Empty;
    public List<PageDetailResponse> Children { get; set; } = [];
}

public class PageParentDetailResponse : EntityDto
{
    public Guid SiteId { get; set; }
    public PageFullDetailResponse? Parent { get; set; }
    public PageFullDetailResponse? Current { get; set; }
    public string Slug { get; set; } = string.Empty;
}

public class PageFullDetailResponse : Page
{
    public string FullPath { get; set; } = default!;
    public LayoutDetailResponse Layout { get; set; } = default!;
    public LayoutDetailResponse DetailLayout { get; set; } = default!;
    public LayoutDetailResponse EditLayout { get; set; } = default!;
    public SiteDetailResponse Site { get; set; } = default!;
    public Dictionary<string, List<PluginDetailResponse>> Sections { get; set; } = [];
    public UserRoleDetailResponse User { get; set; } = default!;
    public List<Guid> ViewRoleIds { get; set; } = [];
    public List<Guid> AdminRoleIds { get; set; } = [];
    public Dictionary<string, string> Settings { get; set; } = [];
}

public class PageDetailResponse : PageModel
{
    public Dictionary<string, string> Settings { get; set; } = [];
    public IEnumerable<Guid> ViewRoleIds { get; set; } = [];
    public IEnumerable<Guid> AdminRoleIds { get; set; } = [];
}


public class GetPageRequest : FilteredAndPagedAndSortedResultRequestDto
{
}

public class PageCreateRequest : ExtensibleEntityDto
{
    [Required]
    public Guid SiteId { get; set; }

    public Guid? ParentId { get; set; }

    public Guid? LayoutId { get; set; } = default!;

    public Guid? DetailLayoutId { get; set; } = default!;

    public Guid? EditLayoutId { get; set; } = default!;

    [Required]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string Path { get; set; } = string.Empty;

    [Required]
    public int Order { get; set; }

    [Required]
    public IEnumerable<Guid> ViewRoleIds { get; set; } = [];

    [Required]
    public IEnumerable<Guid> AdminRoleIds { get; set; } = [];
}

public class PageUpdateRequest : PageCreateRequest
{
    public Guid Id { get; set; }
}
