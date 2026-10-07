using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCMS.Content;

public interface IContentTypeAppService : ICrudAppService<ContentType, Guid, GetContentTypeRequest, ContentTypeCreateRequest, ContentTypeUpdateRequest>
{
}

public interface IContentTypeService : IApplicationService
{
    Task<IEnumerable<ContentType>> GetAll(Guid siteId, CancellationToken cancellationToken = default);
    Task<ContentType> GetBySlug(Guid siteId, string contentTypeSlug, CancellationToken cancellationToken = default);
    Task<ContentType> Create(ContentType contentType, CancellationToken cancellationToken = default);
    Task<ContentType> Update(ContentType contentType, CancellationToken cancellationToken = default);
    Task<ContentType> Delete(Guid contentTypeId, CancellationToken cancellationToken = default);
    Task<ContentType> SetField(Guid contentTypeId, ContentTypeField field, CancellationToken cancellationToken = default);
    Task<ContentType> DeleteField(Guid contentTypeId, string name, CancellationToken cancellationToken = default);
    Task<ContentType> GetById(Guid id, CancellationToken cancellationToken);
}

public class ContentType : ExtensibleFullAuditedEntityDto<Guid>
{
    public Guid SiteId { get; set; }
    public string Slug { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public List<ContentTypeField> Fields { get; set; } = [];
}

public class ContentTypeDetailResponse:ContentType
{

}

public class GetContentTypeRequest : FilteredAndPagedAndSortedResultRequestDto
{
}

public class ContentTypeCreateRequest : ExtensibleEntityDto
{
    [Required]
    public Guid SiteId { get; set; } = default!;

    [Required]
    [Slug]
    public string Slug { get; set; } = default!;

    [Required]
    public string Title { get; set; } = default!;
    public string? Description { get; set; } = default!;
    public List<ContentTypeField> Fields { get; set; } = [];
}

public class ContentTypeUpdateRequest : ExtensibleEntityDto
{
    [Required]
    public Guid Id { get; set; }

    [Required]
    public string Title { get; set; } = default!;

    public string? Description { get; set; } = default!;
    public List<ContentTypeField> Fields { get; set; } = [];
}
