using ByteLabs.Foundations.AspNetCore.TestFactory;

namespace FluentCMS.Content;

public class BlockControllerTestFixture: FluentCMSHttpApiTestBase
{

    protected override string? RoutePath { get; } = $"api/{FluentCMSRemoteServiceConsts.RoutePath}/content/blocks";

    public BlockControllerTestFixture()
    {
        EnableAuthorization();
    }


    [Fact]
    public async Task GetAll_Limited_To_10_Should_Return_10_Records()
    {
        var restrictToRecords = 10;
        var results = await this.GetPagedAsync<Block>(RoutePath, new GetBlockRequest { MaxResultCount = restrictToRecords, SkipCount = 2 });
        results.ShouldNotBeNull();
        results.TotalCount.ShouldBeGreaterThan(restrictToRecords);
        results.Items.Count.ShouldBe(restrictToRecords);
    }

    [Fact]
    public async Task Create_Get_Update_Delete_Should_Work()
    {
        var siteId = GetRequiredService<FluentCMSTestData>().SiteId;
        var blockName = $"HTTP API test block {Guid.NewGuid()}";
        var created = await this.PostAsync<BlockCreateRequest, Block>(RoutePath, new BlockCreateRequest
        {
            SiteId = siteId,
            Name = blockName,
            Category = "Integration test",
            Description = "Created through the block API",
            Content = "Initial content"
        });

        created.ShouldNotBeNull();
        created.Name.ShouldBe(blockName);
        created.SiteId.ShouldBe(siteId);

        var retrieved = await this.GetAsync<Block>($"{RoutePath}/{created.Id}");
        retrieved.Id.ShouldBe(created.Id);
        retrieved.Name.ShouldBe(blockName);

        var updatedName = $"Updated {blockName}";
        var updated = await this.PutAsync<BlockUpdateRequest, Block>($"{RoutePath}/{created.Id}", new BlockUpdateRequest
        {
            SiteId = siteId,
            Name = updatedName,
            Category = "Updated integration test",
            Description = "Updated through the block API",
            Content = "Updated content"
        });

        updated.Name.ShouldBe(updatedName);
        updated.Content.ShouldBe("Updated content");

        await this.DeleteAsync($"{RoutePath}/{created.Id}");
    }
}
