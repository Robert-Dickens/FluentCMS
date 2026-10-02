using System;
using System.Collections.Generic;
using System.Threading;

namespace FluentCMS.Content;

public interface IContentAppService : ICrudAppService<Content, Guid, GetContentRequest, ContentCreateRequest, ContentUpdateRequest>
{
    IAsyncEnumerable<Content> GetAllByContentType(Guid contentTypeId, CancellationToken cancellationToken = default);
}

public class Content : ExtensibleFullAuditedEntityDto<Guid>
{
    public Guid TypeId { get; set; }
    public Dictionary<string, object?> Data { get; set; } = [];

}

public class GetContentRequest : FilteredAndPagedAndSortedResultRequestDto
{

}

public class ContentCreateRequest : ExtensibleEntityDto
{
    public Guid TypeId { get; set; }
    public Dictionary<string, object?> Data { get; set; } = [];
}

public class ContentUpdateRequest : ExtensibleEntityDto
{
    public Guid TypeId { get; set; }
    public Dictionary<string, object?> Data { get; set; } = [];
}