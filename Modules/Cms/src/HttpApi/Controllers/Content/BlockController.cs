using ByteLabs.Foundations;
using ByteLabs.Foundations.Application.Dtos;
using FluentCMS.Content;
using FluentCMS.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace FluentCMS.Api.Controllers.Content;

[Controller]
[RemoteService(Name = FluentCMSRemoteServiceConsts.RemoteServiceName)]
[Area(FluentCMSRemoteServiceConsts.ModuleAreaName)]
[Route($"api/{FluentCMSRemoteServiceConsts.RoutePath}/content/blocks")]
[Authorize(FluentCMSPermissions.SiteContent.Blocks.Default)]
public class BlockController : FluentCMSBaseController, IBlockAppService
{
    private readonly IBlockAppService _blockAppService;

    public BlockController(IBlockAppService blockAppService) => _blockAppService = blockAppService;

    [HttpPost]
    [Authorize(FluentCMSPermissions.SiteContent.Blocks.Create)]
    public Task<Block> CreateAsync(BlockCreateRequest input) => _blockAppService.CreateAsync(input);

    [HttpDelete("{id}")]
    [Authorize(FluentCMSPermissions.SiteContent.Blocks.Delete)]
    public Task DeleteAsync(Guid id) => _blockAppService.DeleteAsync(id);

    [HttpGet("{id}")]
    public Task<Block> GetAsync(Guid id) => _blockAppService.GetAsync(id);

    [HttpGet]
    public Task<PagedResultDto<Block>> GetListAsync(GetBlockRequest input) => _blockAppService.GetListAsync(input);

    [HttpPut("{id}")]
    [Authorize(FluentCMSPermissions.SiteContent.Blocks.Edit)]
    public Task<Block> UpdateAsync(Guid id, BlockUpdateRequest input) => _blockAppService.UpdateAsync(id, input);
}
