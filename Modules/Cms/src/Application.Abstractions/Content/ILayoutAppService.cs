using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCMS.Content;

public interface ILayoutAppService : ICrudAppService<Layout, Guid, GetLayoutRequest, LayoutCreateRequest, LayoutUpdateRequest>
{
}

public interface ILayoutService : IApplicationService
{
    Task<IEnumerable<Layout>> GetAllForSite(Guid siteId, CancellationToken cancellationToken = default);
    Task<Layout> Create(Layout layout, CancellationToken cancellationToken = default);
    Task<Layout> Delete(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Layout>> DeleteBySiteId(Guid siteId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Layout>> GetAll(CancellationToken cancellationToken = default);
    Task<Layout> GetById(Guid id, CancellationToken cancellationToken = default);
    Task<Layout> Update(Layout layout, CancellationToken cancellationToken);
}

public class Layout : ExtensibleFullAuditedEntityDto<Guid>
{
    public Guid SiteId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public string Head { get; set; } = string.Empty;
}

public class LayoutDetailResponse:Layout
{

}

public class GetLayoutRequest : FilteredAndPagedAndSortedResultRequestDto
{
}

public class LayoutCreateRequest : ExtensibleEntityDto
{
    [Required]
    public Guid SiteId { get; set; } = Guid.Empty;

    [Required]
    public string Name { get; set; } = default!;

    [Required]
    public string Body { get; set; } = default!;

    [Required]
    public string Head { get; set; } = default!;
}

public class LayoutUpdateRequest : ExtensibleEntityDto
{
    [Required]
    public Guid Id { get; set; }

    [Required]
    public string Name { get; set; } = default!;

    [Required]
    public string Body { get; set; } = default!;

    [Required]
    public string Head { get; set; } = default!;
}
