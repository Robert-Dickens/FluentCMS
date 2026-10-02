using FluentCMS.Content;
using FluentCMS.Domain.Content;
using FluentCMS.Permissions;
using Microsoft.AspNetCore.Authorization;
using System;

namespace FluentCMS.Services.Content;

[Authorize(FluentCMSPermissions.SiteContent.Blocks.Default)]
public class BlockAppService : FluentCMSCrudAppService<BlockModel, Block, Guid, GetBlockRequest, BlockCreateRequest, BlockUpdateRequest>, IBlockAppService
{
    public BlockAppService(IBlockModelRepository repository) : base(repository)
    {
        CreatePolicyName = FluentCMSPermissions.SiteContent.Blocks.Create;
        UpdatePolicyName = FluentCMSPermissions.SiteContent.Blocks.Edit;
        DeletePolicyName = FluentCMSPermissions.SiteContent.Blocks.Delete;
    }
}
