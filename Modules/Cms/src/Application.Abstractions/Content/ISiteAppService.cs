using System;

namespace FluentCMS.Content;

public interface ISiteAppService : ICrudAppService<Site, Guid, GetSiteRequest, SiteCreateRequest, SiteUpdateRequest>
{
}

public class Site : FullAuditedEntityDto<Guid>
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Urls { get; set; } = string.Empty;
    public Guid LayoutId { get; set; }
    public Guid DetailLayoutId { get; set; }
    public Guid EditLayoutId { get; set; }
}

public class GetSiteRequest : FilteredAndPagedAndSortedResultRequestDto
{
}

public class SiteCreateRequest : ExtensibleEntityDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Urls { get; set; } = string.Empty;
    public Guid LayoutId { get; set; }
    public Guid DetailLayoutId { get; set; }
    public Guid EditLayoutId { get; set; }
}

public class SiteUpdateRequest : ExtensibleEntityDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Urls { get; set; } = string.Empty;
    public Guid LayoutId { get; set; }
    public Guid DetailLayoutId { get; set; }
    public Guid EditLayoutId { get; set; }
}
