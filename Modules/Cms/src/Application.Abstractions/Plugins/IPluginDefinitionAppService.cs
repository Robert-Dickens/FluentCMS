using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCMS.Plugins;

public interface IPluginDefinitionAppService : ICrudAppService<PluginDefinition, Guid, GetPluginDefinitionRequest, PluginDefinitionCreateRequest, PluginDefinitionUpdateRequest>
{
}

public interface IPluginDefinitionService : IApplicationService
{
    Task<IEnumerable<PluginDefinition>> GetAll(CancellationToken cancellationToken = default);
    Task<PluginDefinition> GetById(Guid id, CancellationToken cancellationToken = default);
    Task<PluginDefinition> Create(PluginDefinition pluginDefinition, CancellationToken cancellationToken = default);
    Task<PluginDefinition> Update(PluginDefinition pluginDefinition, CancellationToken cancellationToken = default);
    Task<PluginDefinition> Delete(Guid id, CancellationToken cancellationToken = default);
}


public class PluginDefinition : FullAuditedEntityDto<Guid>
{
    public string Name { get; set; } = default!;
    public string Category { get; set; } = default!;
    public string Assembly { get; set; } = default!;
    public string? Icon { get; set; } = default!;
    public string? Description { get; set; }
    public List<PluginDefinitionType> Types { get; set; } = [];
    public List<string> Stylesheets { get; set; } = [];
    public bool Locked { get; set; } = false;

}


public class PluginDefinitionDetailResponse : PluginDefinition
{
}


public class PluginDefinitionType
{
    public string Name { get; set; } = default!;
    public string Type { get; set; } = default!;
    public bool IsDefault { get; set; } = false;
}

public class GetPluginDefinitionRequest : FilteredAndPagedAndSortedResultRequestDto
{
}

public class PluginDefinitionCreateRequest : EntityDto
{
    public string Name { get; set; } = default!;
    public string Category { get; set; } = default!;
    public string? Description { get; set; }
    public string Type { get; set; } = default!;
    public List<string> Stylesheets { get; set; } = [];
}

public class PluginDefinitionUpdateRequest : EntityDto
{
    public string Name { get; set; } = default!;
    public string Category { get; set; } = default!;
    public string Assembly { get; set; } = default!;
    public string? Icon { get; set; } = default!;
    public string? Description { get; set; }
    public List<PluginDefinitionType> Types { get; set; } = [];
    public List<string> Stylesheets { get; set; } = [];
    public bool Locked { get; set; } = false;
}
