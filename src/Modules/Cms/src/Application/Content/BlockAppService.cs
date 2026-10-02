using FluentCMS.Domain.Content;
using Microsoft.AspNetCore.Authorization;
using System;

namespace FluentCMS.Services.Content;

[Authorize]
public class BlockAppService : FluentCMSCrudAppService<BlockModel, FluentCMS.Content.Block, Guid, GetBlockRequest, BlockCreateRequest, BlockUpdateRequest>, IBlockAppService
{
    public BlockAppService(IBlockModelRepository repository) : base(repository)
    {
    }
}
