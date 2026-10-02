using ByteLabs.Foundations.Application.Services;
using System;

namespace FluentCMS.Content;

public interface IContentTypeAppService : ICrudAppService<ContentType, Guid, GetContentTypeRequest, ContentTypeCreateRequest, ContentTypeUpdateRequest>
{
}

public class ContentType : ExtensibleFullAuditedEntityDto<Guid>
{
    public Guid SiteId { get; set; }
    public string Slug { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
}

public class GetContentTypeRequest : FilteredAndPagedAndSortedResultRequestDto
{
}

public class ContentTypeCreateRequest : ExtensibleEntityDto
{
    public Guid SiteId { get; set; }
    public string Slug { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
}

public class ContentTypeUpdateRequest : ExtensibleEntityDto
{
    public Guid SiteId { get; set; }
    public string Slug { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
}
