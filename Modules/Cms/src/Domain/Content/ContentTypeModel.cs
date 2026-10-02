using System;
using System.Collections.Generic;

namespace FluentCMS.Domain.Content;

public class ContentTypeModel : SiteAssociatedEntityModel
{
    public string Slug { get; set; } = default!;
    public string Title { get; set; } = default!;
    public string? Description { get; set; }
    public ICollection<ContentTypeFieldModel> Fields { get; set; } = [];

    public ContentTypeModel() : base(Guid.NewGuid())
    {

    }

}
