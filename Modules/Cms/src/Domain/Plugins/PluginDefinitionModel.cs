using ByteLabs.Foundations.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;

namespace FluentCMS.Domain.Plugins;

public class PluginDefinitionModel : FullAuditedEntity<Guid>
{
    public string Name { get; set; } = default!;
    public string Category { get; set; } = default!;
    public string Assembly { get; set; } = default!;
    public string? Icon { get; set; } = default!;
    public string? Description { get; set; }
    public bool Locked { get; set; } = false;
    public string Stylesheets { get; set; } = default!;
    public ICollection<PluginDefinitionTypeModel> Types { get; set; } = []; // Navigation property

    public PluginDefinitionModel() : base(Guid.NewGuid())
    {

    }

}
