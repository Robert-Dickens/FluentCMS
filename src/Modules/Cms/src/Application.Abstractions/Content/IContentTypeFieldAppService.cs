using ByteLabs.Foundations.Application.Services;
using System;

namespace FluentCMS.Content;

public interface IContentTypeFieldAppService : ICrudAppService<ContentTypeField, Guid, GetContentTypeFieldRequest, ContentTypeFieldCreateRequest, ContentTypeFieldUpdateRequest>
{
}

public class ContentTypeField : EntityDto<Guid>
{
    public Guid ContentTypeId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public bool Required { get; set; }
    public bool Unique { get; set; }
    public string Label { get; set; } = string.Empty;
    public string Settings { get; set; } = string.Empty;
}

public class GetContentTypeFieldRequest : FilteredAndPagedAndSortedResultRequestDto
{
}

public class ContentTypeFieldCreateRequest : ExtensibleEntityDto
{
    public Guid ContentTypeId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public bool Required { get; set; }
    public bool Unique { get; set; }
    public string Label { get; set; } = string.Empty;
    public string Settings { get; set; } = string.Empty;
}

public class ContentTypeFieldUpdateRequest : ExtensibleEntityDto
{
    public Guid ContentTypeId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public bool Required { get; set; }
    public bool Unique { get; set; }
    public string Label { get; set; } = string.Empty;
    public string Settings { get; set; } = string.Empty;
}
