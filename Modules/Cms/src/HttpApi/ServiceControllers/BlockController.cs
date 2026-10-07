using FluentCMS.Api;
using FluentCMS.Content;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCMS.Web.Api.Controllers;

public class BlockController(IBlockService blockService) : BaseGlobalController
{
    public const string AREA = "Block Management";
    public const string READ = "Read";
    public const string UPDATE = $"Update/{READ}";
    public const string CREATE = "Create";
    public const string DELETE = $"Delete/{READ}";

    [HttpGet("{id}")]
    [Policy(AREA, READ)]
    public async Task<IApiResult<BlockDetailResponse>> GetById([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        var blocks = await blockService.GetById(id, cancellationToken);
        var blockResponse = ObjectMapper.Map<Block, BlockDetailResponse>(blocks);
        return Ok(blockResponse);
    }

    [HttpGet]
    [Policy(AREA, READ)]
    public async Task<IApiPagingResult<BlockDetailResponse>> GetAllForSite([FromQuery] Guid siteId, CancellationToken cancellationToken = default)
    {
        var blocks = await blockService.GetAllForSite(siteId, cancellationToken);
        var blockResponse = ObjectMapper.Map<IEnumerable<Block>, List<BlockDetailResponse>>(blocks);
        return OkPaged(blockResponse);
    }

    [HttpPost]
    [Policy(AREA, CREATE)]
    public async Task<IApiResult<BlockDetailResponse>> Create([FromBody] BlockCreateRequest request, CancellationToken cancellationToken = default)
    {
        var block = ObjectMapper.Map<BlockCreateRequest, Block>(request);
        var newBlock = await blockService.Create(block, cancellationToken);
        var response = ObjectMapper.Map<Block, BlockDetailResponse>(newBlock);
        return Ok(response);
    }

    [HttpPut]
    [Policy(AREA, UPDATE)]
    public async Task<IApiResult<BlockDetailResponse>> Update([FromBody] BlockUpdateRequest request, CancellationToken cancellationToken = default)
    {
        var block = ObjectMapper.Map<BlockUpdateRequest, Block>(request);
        var updated = await blockService.Update(block, cancellationToken);
        var response = ObjectMapper.Map<Block,BlockDetailResponse>(updated);
        return Ok(response);
    }

    [HttpDelete("{id}")]
    [Policy(AREA, DELETE)]
    public async Task<IApiResult<bool>> Delete([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        await blockService.Delete(id, cancellationToken);
        return Ok(true);
    }
}
