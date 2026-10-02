using ByteLabs.Foundations.Domain.Entities;
using System;

namespace FluentCMS.Domain.Content;

public class ContentTypeFieldModel : Entity<Guid>
{
    public Guid ContentTypeId { get; set; } = default!; // Foreign key
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string Type { get; set; } = default!;
    public bool Required { get; set; }
    public bool Unique { get; set; }
    public string Label { get; set; } = default!;
    public string Settings { get; set; } = default!; // JSON string for settings
    public ContentTypeModel ContentType { get; set; } = default!; // Navigation property


    public ContentTypeFieldModel() : base(Guid.NewGuid())
    {

    }

}
