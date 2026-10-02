using System;

namespace FluentCMS.Domain.Content;

public class LayoutModel : SiteAssociatedEntityModel
{
    public string Name { get; set; } = default!;
    public string Body { get; set; } = default!;
    public string Head { get; set; } = default!;

    public LayoutModel() : base(Guid.NewGuid())
    {

    }

}
