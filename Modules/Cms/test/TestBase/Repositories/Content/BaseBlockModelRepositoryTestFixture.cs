using ByteLabs.Foundations.Modularity;
using ByteLabs.Foundations.MultiTenancy;
using FluentCMS.DataSeeders;
using FluentCMS.Domain.Content;
using Meziantou.Xunit;

namespace FluentCMS.Repositories.Content;

[EnableParallelization]
[Collection(nameof(BlockModel))]
public abstract class BaseBlockModelRepositoryTestFixture<TStartupModule> : FluentCMSTestBase<TStartupModule, IBlockModelRepository>
    where TStartupModule : IPlatformModule
{

    [Fact]
    public virtual async Task Get_All_Records()
    {
        var repository = GetRepository();
        repository.ShouldNotBeNull();

        var allRecordCount = await repository.GetCountAsync();
        allRecordCount.ShouldBeGreaterThan(0);
    }

    [Fact]
    public virtual async Task Get_All_Records_For_Non_Tenant()
    {
        var repository = GetRepository();
        repository.ShouldNotBeNull();

        var allRecords = await repository.GetListAsync();
        allRecords.ShouldNotBeNull();
        allRecords.Count.ShouldBeGreaterThan(0);

        allRecords.All(x => x.TenantId.HasValue == false).ShouldBeTrue();
    }


    [Fact]
    public virtual async Task Get_All_Records_For_Given_Tenant()
    {
        var currentTenant = GetRequiredService<ICurrentTenant>();
        var knownTenantId = KnownTenants.TenantId;

        using (var tenant = currentTenant.Change(knownTenantId))
        {
            var repository = GetRepository();
            repository.ShouldNotBeNull();
            var allRecords = await repository.GetListAsync();
            allRecords.ShouldNotBeNull();
            allRecords.Count.ShouldBeGreaterThan(0);

            allRecords.All(x => x.TenantId.Equals(knownTenantId)).ShouldBeTrue();
        }
    }

}