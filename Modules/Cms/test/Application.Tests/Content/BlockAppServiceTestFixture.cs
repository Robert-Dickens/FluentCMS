using Microsoft.EntityFrameworkCore;

namespace FluentCMS.Content;

public class BlockAppServiceTestFixture : FluentCMSApplicationTestBase
{
    private IBlockAppService _appService;

    public BlockAppServiceTestFixture()
    {
        _appService = GetRequiredService<IBlockAppService>();
    }


    [Fact]
    public async Task GetAll_Limited_To_10_Should_Return_10_Records()
    {
        var allBlocks = await UsingDbContext(async dbContext => await dbContext.Blocks.ToListAsync());
        allBlocks.ShouldNotBeNull();

        var totalRecordsInDb = allBlocks.Count();
        var restrictToRecords = 10;

        totalRecordsInDb.ShouldBeGreaterThan(10);


        var results = await _appService.GetListAsync(new GetBlockRequest { MaxResultCount = restrictToRecords, SkipCount = 1 });

        results.ShouldNotBeNull();
        results.TotalCount.ShouldBe(totalRecordsInDb);
        results.Items.Count.ShouldBe(restrictToRecords);
    }
}
