using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCMS.Content;

public interface IBlockAppService : ICrudAppService<Block, Guid, GetBlockRequest, BlockCreateRequest, BlockUpdateRequest>
{
}

public interface IBlockService : IApplicationService
{
    Task<IEnumerable<Block>> GetAllForSite(Guid siteId, CancellationToken cancellationToken = default);
    Task<Block> GetById(Guid id, CancellationToken cancellationToken = default);
    Task<Block> Create(Block Block, CancellationToken cancellationToken = default);
    Task<Block> Update(Block Block, CancellationToken cancellationToken = default);
    Task<Block> Delete(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Block>> Delete(IEnumerable<Guid> ids, CancellationToken cancellationToken = default);
}


public class Block : ExtensibleFullAuditedEntityDto<Guid>
{
    public Guid SiteId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Content { get; set; } = string.Empty;
}

public class BlockDetailResponse: Block
{

}

public class GetBlockRequest : FilteredAndPagedAndSortedResultRequestDto
{
}

public class BlockCreateRequest : ExtensibleEntityDto
{
    [Required]
    public Guid SiteId { get; set; } = default!;

    [Required]
    public string Name { get; set; } = default!;

    [Required]
    public string Category { get; set; } = default!;

    public string? Description { get; set; } = default!;

    [Required]
    public string Content { get; set; } = default!;
}

public class BlockUpdateRequest : ExtensibleEntityDto
{
    [Required]
    public Guid Id { get; set; }

    [Required]
    public string Name { get; set; } = default!;

    [Required]
    public string Category { get; set; } = default!;

    public string? Description { get; set; } = default!;

    [Required]
    public string Content { get; set; } = default!;
}
