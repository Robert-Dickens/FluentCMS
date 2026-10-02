using System;

namespace FluentCMS.Content;

public interface IPageAppService : ICrudAppService<Page, Guid, GetPageRequest, PageCreateRequest, PageUpdateRequest>
{
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

public class GetPageRequest : FilteredAndPagedAndSortedResultRequestDto
{
}

public class PageCreateRequest : ExtensibleEntityDto
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

public class PageUpdateRequest : ExtensibleEntityDto
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
