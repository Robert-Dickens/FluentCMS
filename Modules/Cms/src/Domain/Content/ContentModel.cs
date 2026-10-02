using System;

namespace FluentCMS.Domain.Content;

public class ContentModel : SiteAssociatedEntityModel
{
    public Guid TypeId { get; set; }
    public string Data { get; set; } = default!;

    public ContentModel() : base(Guid.NewGuid())
    {

    }

}
