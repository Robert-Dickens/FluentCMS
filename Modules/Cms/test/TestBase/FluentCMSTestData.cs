using ByteLabs.Foundations.DependencyInjection;

namespace FluentCMS;

public class FluentCMSTestData : ISingletonDependency
{
    public Guid SiteId { get; private set; }

    public Guid? TenantId { get; private set; }

    public Guid ContentTypeId { get; private set; }

    public IReadOnlyList<Guid> BlockIds { get; private set; } = [];

    public IReadOnlyList<Guid> ContentIds { get; private set; } = [];

    public void SetCmsSeedIds(Guid siteId,Guid? tenantId,  Guid contentTypeId, IReadOnlyList<Guid> blockIds, IReadOnlyList<Guid> contentIds)
    {
        SiteId = siteId;
        TenantId = tenantId;
        ContentTypeId = contentTypeId;
        BlockIds = blockIds;
        ContentIds = contentIds;
    }

    public Guid TvId { get; } = Guid.NewGuid();

    public string TvName { get; } = "Super New 105\" TV ";

    public float TvPrice { get; } = 45900.90f;

    public Guid BookId { get; } = Guid.NewGuid();

    public string BookName { get; } = "The Hitchhiker's Guide to the Galaxy";

    public float BookPrice { get; } = 12.50f;

    public Guid WatchId { get; } = Guid.NewGuid();

    public string WatchName { get; } = "Swiss Diamond Watch for Men";

    public float WatchPrice { get; } = 249.50f;
}
