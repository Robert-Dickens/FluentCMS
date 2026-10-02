using ByteLabs.Foundations.Data;
using ByteLabs.Foundations.DependencyInjection;
using ByteLabs.Foundations.TestFactory.RandomData;
using FluentCMS.Domain.Content;
using System.Text.Json;

namespace FluentCMS.DataSeeders;

public class SiteContentDataSeeder : IDataSeedContributor, ITransientDependency
{
    private readonly IRandomData<BlockModel> _randomData;
    private readonly IRandomData<SiteModel> _siteRandomData;
    private readonly IRandomData<ContentTypeModel> _contentTypeRandomData;
    private readonly IRandomData<ContentModel> _contentRandomData;
    private readonly IBlockModelRepository _blocksRepository;
    private readonly FluentCMSTestData _testData;
    private readonly ISiteModelRepository _siteRepository;
    private readonly IContentTypeModelRepository _contentTypeRepository;
    private readonly IContentModelRepository _contentRepository;

    public SiteContentDataSeeder(
        IRandomData<BlockModel> randomData,
        IRandomData<SiteModel> siteRandomData,
        IRandomData<ContentTypeModel> contentTypeRandomData,
        IRandomData<ContentModel> contentRandomData,
        IBlockModelRepository blocksRepository,
        FluentCMSTestData testData,
        ISiteModelRepository siteRepository,
        IContentTypeModelRepository contentTypeRepository,
        IContentModelRepository contentRepository)
    {
        _randomData = randomData;
        _siteRandomData = siteRandomData;
        _contentTypeRandomData = contentTypeRandomData;
        _contentRandomData = contentRandomData;
        _blocksRepository = blocksRepository;
        _testData = testData;
        _siteRepository = siteRepository;
        _contentTypeRepository = contentTypeRepository;
        _contentRepository = contentRepository;
    }

    public async Task SeedAsync(DataSeedContext context)
    {
        if (await _blocksRepository.GetCountAsync() > 0)
        {
            return;
        }

        var site = _siteRandomData.CreateListOfSize(1).Build()[0];
        site.Name = "CMS test site";
        site.Urls = "https://example.test";
        site.TenantId = context.TenantId;
        site.LayoutId = Guid.Empty;
        site.DetailLayoutId = Guid.Empty;
        site.EditLayoutId = Guid.Empty;
        await _siteRepository.InsertAsync(site);

        var contentType = _contentTypeRandomData.CreateListOfSize(1).Build()[0];
        contentType.SiteId = site.Id;
        contentType.TenantId = site.TenantId;
        contentType.Slug = "cms-test-content";
        contentType.Title = "CMS test content";
        await _contentTypeRepository.InsertAsync(contentType);

        var blocks = _randomData.CreateListOfSize(100).Build();
        for (var i = 0; i < blocks.Count; i++)
        {
            blocks[i].SiteId = site.Id;
            blocks[i].TenantId = site.TenantId;
            blocks[i].Name = $"CMS test block {i}";
            blocks[i].Category = "Test";
            blocks[i].Content = $"Content {i}";
        }

        await _blocksRepository.InsertManyAsync(blocks);

        var contents = _contentRandomData.CreateListOfSize(100).Build();
        for (var i = 0; i < contents.Count; i++)
        {
            contents[i].SiteId = site.Id;
            contents[i].TenantId = site.TenantId;
            contents[i].TypeId = contentType.Id;
            contents[i].Data = JsonSerializer.Serialize(new { value = $"content-{i}", sequence = i });
        }

        await _contentRepository.InsertManyAsync(contents);
        _testData.SetCmsSeedIds(site.Id, site.TenantId, contentType.Id, blocks.Select(x => x.Id).ToArray(), contents.Select(x => x.Id).ToArray());
    }
}