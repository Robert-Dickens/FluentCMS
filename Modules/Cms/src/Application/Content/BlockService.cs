using ByteLabs.Foundations.DependencyInjection;
using FluentCMS.Content;
using FluentCMS.Domain.Content;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCMS.Services.Content;

public class BlockService(IBlockModelRepository blockRepository) : FluentCMSServiceAppService, IBlockService, ITransientDependency
{
    public async Task<IEnumerable<Block>> GetAllForSite(Guid siteId, CancellationToken cancellationToken = default)
    {
        var results = await blockRepository.GetAllForSite(siteId, cancellationToken);
        return ObjectMapper.Map<IEnumerable<BlockModel>, List<Block>>(results);
    }

    public async Task<Block> GetById(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await blockRepository.FindAsync(id, cancellationToken: cancellationToken) ??
            throw new AppException(ExceptionCodes.BlockNotFound);
        return ObjectMapper.Map<BlockModel, Block>(result);
    }

    public async Task<Block> Create(Block block, CancellationToken cancellationToken = default)
    {
        var blockEntity = ObjectMapper.Map<Block, BlockModel>(block);
        var newBlock = await blockRepository.InsertAsync(blockEntity, cancellationToken: cancellationToken) ??
            throw new AppException(ExceptionCodes.BlockUnableToCreate);
        return ObjectMapper.Map<BlockModel, Block>(newBlock);
    }

    public async Task<Block> Update(Block block, CancellationToken cancellationToken = default)
    {
        var original = await blockRepository.FindAsync(block.Id, cancellationToken: cancellationToken) ??
            throw new AppException(ExceptionCodes.BlockNotFound);

        original.Name = block.Name;
        original.Category = block.Category;
        original.Description = block.Description;
        original.Content = block.Content;

        var result = await blockRepository.UpdateAsync(original, cancellationToken: cancellationToken) ??
            throw new AppException(ExceptionCodes.BlockUnableToUpdate);
        return ObjectMapper.Map<BlockModel, Block>(result);
    }

    public async Task<Block> Delete(Guid id, CancellationToken cancellationToken = default)
    {
        _ = await blockRepository.FindAsync(id, cancellationToken: cancellationToken) ??
            throw new AppException(ExceptionCodes.BlockNotFound);

        var result = await blockRepository.DeleteWithResultAsync(id, cancellationToken: cancellationToken) ??
            throw new AppException(ExceptionCodes.BlockUnableToDelete);
        return ObjectMapper.Map<BlockModel, Block>(result);

    }

    public async Task<IEnumerable<Block>> Delete(IEnumerable<Guid> ids, CancellationToken cancellationToken = default)
    {
        var results = await blockRepository.DeleteManyWithResultAsync(ids, cancellationToken: cancellationToken);

        return ObjectMapper.Map<IEnumerable<BlockModel>, List<Block>>(results);
    }
}
