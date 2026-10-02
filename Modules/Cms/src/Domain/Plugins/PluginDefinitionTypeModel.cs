using ByteLabs.Foundations.Domain.Entities;
using System;

namespace FluentCMS.Domain.Plugins;

public class PluginDefinitionTypeModel : Entity<Guid>
{
    public string Name { get; set; } = default!;
    public string Type { get; set; } = default!;
    public bool IsDefault { get; set; } = false;
    public Guid PluginDefinitionId { get; set; } // foreign key
    public PluginDefinitionModel PluginDefinition { get; set; } = default!; // navigation property

    public PluginDefinitionTypeModel() : base(Guid.NewGuid())
    {

    }

}
