using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCMS.Content;

public interface IContentAppService : ICrudAppService<Content, Guid, GetContentRequest, ContentCreateRequest, ContentUpdateRequest>
{
    IAsyncEnumerable<Content> GetAllByContentType(Guid contentTypeId, CancellationToken cancellationToken = default);
}

public interface IContentService : IApplicationService
{
    Task<IEnumerable<Content>> GetAllByContentType(Guid contentTypeId, CancellationToken cancellationToken = default);
    Task<Content> Create(Content content, CancellationToken cancellationToken = default);
    Task<Content> Delete(Guid id, CancellationToken cancellationToken = default);
    Task<Content> GetById(Guid id, CancellationToken cancellationToken = default);
    Task<Content> Update(Content content, CancellationToken cancellationToken = default);
}

public class Content : ExtensibleFullAuditedEntityDto<Guid>
{
    [Required]
    public Guid SiteId { get; set; }
    [Required]
    public Guid TypeId { get; set; }
    [Required]
    public Dictionary<string, object?> Data { get; set; } = [];

}

public class ContentDetailResponse : Content
{

}


public class GetContentRequest : FilteredAndPagedAndSortedResultRequestDto
{

}

public class ContentCreateRequest : ExtensibleEntityDto
{
    public Guid SiteId { get; set; }
    public Guid TypeId { get; set; }
    public Dictionary<string, object?> Data { get; set; } = [];
}

public class ContentUpdateRequest : ExtensibleEntityDto
{
    public Guid SiteId { get; set; }
    public Guid TypeId { get; set; }
    public Dictionary<string, object?> Data { get; set; } = [];
}