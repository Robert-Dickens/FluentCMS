using ByteLabs.Foundations.Application.Services;
using System;

namespace FluentCMS.Plugins;

public interface IPluginDefinitionAppService : ICrudAppService<PluginDefinition, Guid, GetPluginDefinitionRequest, PluginDefinitionCreateRequest, PluginDefinitionUpdateRequest>
{
}

public class PluginDefinition : FullAuditedEntityDto<Guid>
{
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Assembly { get; set; } = string.Empty;
    public string? Icon { get; set; }
    public string? Description { get; set; }
    public bool Locked { get; set; }
    public string Stylesheets { get; set; } = string.Empty;
}

public class GetPluginDefinitionRequest : FilteredAndPagedAndSortedResultRequestDto
{
}

public class PluginDefinitionCreateRequest : ExtensibleEntityDto
{
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Assembly { get; set; } = string.Empty;
    public string? Icon { get; set; }
    public string? Description { get; set; }
    public bool Locked { get; set; }
    public string Stylesheets { get; set; } = string.Empty;
}

public class PluginDefinitionUpdateRequest : ExtensibleEntityDto
{
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Assembly { get; set; } = string.Empty;
    public string? Icon { get; set; }
    public string? Description { get; set; }
    public bool Locked { get; set; }
    public string Stylesheets { get; set; } = string.Empty;
}
