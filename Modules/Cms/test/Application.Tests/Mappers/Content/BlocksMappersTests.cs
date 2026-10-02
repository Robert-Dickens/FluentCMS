using ByteLabs.Foundations.ObjectMapping;
using ByteLabs.Foundations.TestFactory.RandomData;
using FluentCMS.Content;
using FluentCMS.Domain.Content;
using FluentCMS.Services;
using Meziantou.Xunit;

namespace FluentCMS.Mappers.Content;

[EnableParallelization]
[Collection(nameof(BlockModel))]
public class BlocksMappersTests: FluentCMSApplicationTestBase
{
    private readonly IRandomData<BlockModel> _randomBlockModel;
    private readonly IRandomData<Block> _randomBlock;

    private IObjectMapper _mapper;

    public BlocksMappersTests()
    {
        _randomBlockModel = GetRequiredService<IRandomData<BlockModel>>();
        _randomBlock = GetRequiredService<IRandomData<Block>>();
        _mapper = GetRequiredService<IObjectMapper<FluentCMSApplicationModule>>();
    }

    [Fact]
    public void Mapping_From_BlockModel_To_Blocks_Should_Map()
    {
        var sut = _randomBlockModel.CreateNew().Build();
        var mappedSut = _mapper.Map<BlockModel, Block>(sut);

        mappedSut.ShouldNotBeNull();

        mappedSut.Category.ShouldBe(sut.Category);

    }


    [Fact]
    public void Mapping_From_Block_To_BlocksModel_Should_Map()
    {
        var sut = _randomBlock.CreateNew().Build();
        var mappedSut = _mapper.Map<Block, BlockModel>(sut);

        mappedSut.ShouldNotBeNull();

        mappedSut.Category.ShouldBe(sut.Category);

    }
}
