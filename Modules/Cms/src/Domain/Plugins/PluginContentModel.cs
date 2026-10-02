using System;

namespace FluentCMS.Domain.Plugins;

public class PluginContentModel : SiteAssociatedEntityModel
{
    public Guid PluginId { get; set; }
    public string Type { get; set; } = default!;
    public string Data { get; set; } = default!;

    public PluginContentModel() : base(Guid.NewGuid())
    {

    }

}
