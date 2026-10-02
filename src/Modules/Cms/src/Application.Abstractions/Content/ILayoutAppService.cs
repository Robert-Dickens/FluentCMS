using ByteLabs.Foundations.Application.Services;
using System;

namespace FluentCMS.Content;

public interface ILayoutAppService : ICrudAppService<Layout, Guid, GetLayoutRequest, LayoutCreateRequest, LayoutUpdateRequest>
{
}

public class Layout : ExtensibleFullAuditedEntityDto<Guid>
{
    public Guid SiteId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public string Head { get; set; } = string.Empty;
}

public class GetLayoutRequest : FilteredAndPagedAndSortedResultRequestDto
{
}

public class LayoutCreateRequest : ExtensibleEntityDto
{
    public Guid SiteId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public string Head { get; set; } = string.Empty;
}

public class LayoutUpdateRequest : ExtensibleEntityDto
{
    public Guid SiteId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public string Head { get; set; } = string.Empty;
}
