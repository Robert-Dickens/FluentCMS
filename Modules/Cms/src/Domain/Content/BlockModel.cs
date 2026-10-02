using System;

namespace FluentCMS.Domain.Content;

public class BlockModel : SiteAssociatedEntityModel
{
    public string Name { get; set; } = default!;
    public string Category { get; set; } = default!;
    public string? Description { get; set; }
    public string Content { get; set; } = default!;

    public BlockModel() : base(Guid.NewGuid())
    {

    }


    public BlockModel(Guid id, Guid siteId) : base(id)
    {
        SiteId = siteId;
    }

    public BlockModel(Guid id, Guid siteId, string name, string category):base(id)
    {
        SiteId = siteId;
        Name = name;
        Category = category;
    }
}
