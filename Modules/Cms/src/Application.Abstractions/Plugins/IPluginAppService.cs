using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCMS.Plugins;

public interface IPluginAppService : ICrudAppService<Plugin, Guid, GetPluginRequest, PluginCreateRequest, PluginUpdateRequest>
{
    Task<Plugin> InitialCreateAsync(InitialCreateRequest? body, CancellationToken cancellationToken = default);
    Task<Plugin?> UpdateColsAsync(Guid id, UpdateColsRequest? body, CancellationToken cancellationToken = default);
    Task<PagedResultDto<Plugin?>> UpdateOrdersAsync(UpdateOrdersRequest? body, CancellationToken cancellationToken = default);
}

public interface IPluginService : IApplicationService
{
    Task<IEnumerable<Plugin>> GetByPageId(Guid pageId, CancellationToken cancellationToken = default);
    Task<Plugin> GetById(Guid id, CancellationToken cancellationToken = default);
    Task<Plugin> Create(Plugin plugin, CancellationToken cancellationToken = default);
    Task<Plugin> InitialCreate(Guid pageId, Guid pluginDefinitionId, string section, CancellationToken cancellationToken = default);
    Task<Plugin> Update(Plugin plugin, CancellationToken cancellationToken = default);
    Task<IEnumerable<Plugin>> UpdateOrders(IEnumerable<PluginOrder> pluginOrders, CancellationToken cancellationToken = default);
    Task<Plugin> UpdateCols(Guid pluginId, int cols, int colsMd, int colsLg, CancellationToken cancellationToken = default);
    Task<Plugin> Delete(Guid id, CancellationToken cancellationToken = default);
}

public class Plugin : ExtensibleFullAuditedEntityDto<Guid>
{
    public Guid SiteId { get; set; }
    public Guid DefinitionId { get; set; }
    public Guid PageId { get; set; }
    public int Order { get; set; }
    public int Cols { get; set; }
    public int ColsMd { get; set; }
    public int ColsLg { get; set; }
    public string Section { get; set; } = string.Empty;
    public bool Locked { get; set; }
}

public class PluginDetailResponse : Plugin
{
    public PluginDefinitionDetailResponse Definition { get; set; } = default!;
    public Dictionary<string, string> Settings { get; set; } = [];
}


public class GetPluginRequest : FilteredAndPagedAndSortedResultRequestDto
{
}

public class PluginCreateRequest : ExtensibleEntityDto
{
    public Guid SiteId { get; set; }
    public Guid DefinitionId { get; set; }
    public Guid PageId { get; set; }
    public int Order { get; set; }
    public int Cols { get; set; } = 12;
    public int ColsMd { get; set; }
    public int ColsLg { get; set; }
    public string Section { get; set; } = string.Empty;
    public bool Locked { get; set; }
}

public class PluginUpdateRequest : ExtensibleEntityDto<Guid>
{
    public int Order { get; set; } = 0;
    public string Section { get; set; } = default!;
    public int Cols { get; set; } = 0;
    public int ColsMd { get; set; } = 0;
    public int ColsLg { get; set; } = 0;
    public Dictionary<string, string>? Settings { get; set; } = [];
}

public class InitialCreateRequest : EntityDto
{
    [Required(AllowEmptyStrings = true)]
    public Guid DefinitionId { get; set; } = default!;

    [Required(AllowEmptyStrings = true)]
    public Guid PageId { get; set; } = default!;

    [Required]
    public string Section { get; set; } = default!;

}

public class UpdateColsRequest : EntityDto<Guid>
{
    public int Cols { get; set; } = default!;
    public int ColsMd { get; set; } = default!;
    public int ColsLg { get; set; } = default!;
}

public class UpdateOrdersRequest:EntityDto
{
    [Required]
    public ICollection<PluginOrder> PluginOrders { get; set; } = new Collection<PluginOrder>();

}

public class PluginOrder: EntityDto<Guid>
{
    public string? Section { get; set; } = default!;
}