using ByteLabs.Foundations.Domain.Repositories;
using FluentCMS.DataSeeders;
using FluentCMS.Domain.Content;
using FluentCMS.Repositories.Content;
using ByteLabs.Foundations.MultiTenancy;
using Moq;

namespace FluentCMS.Content;

public class BlockModelRepositoryTests : BaseBlockModelRepositoryTestFixture<FluentCMSDomainTestModule>
{

    protected override IBlockModelRepository GetRepository()
    {
        var currentTenant = GetRequiredService<ICurrentTenant>();
        var mockBlocks = new List<BlockModel>();

        mockBlocks.AddRange([
            new BlockModel(Guid.NewGuid(),Guid.NewGuid()),
            new BlockModel(Guid.NewGuid(),Guid.NewGuid()){ TenantId = currentTenant.Id}
            ]);

        var mockRepository = new Mock<IBlockModelRepository>();
        mockRepository.Setup(m => m.GetCountAsync(It.IsAny<CancellationToken>())).ReturnsAsync(25);
        mockRepository
            .Setup(m => m.GetListAsync(It.IsAny<bool>(), It.IsAny<CancellationToken>()))
            .Returns((bool _, CancellationToken _) => Task.FromResult(mockBlocks));
        return mockRepository.Object;
    }
}
