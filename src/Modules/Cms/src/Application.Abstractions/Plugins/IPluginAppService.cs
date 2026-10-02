using ByteLabs.Foundations.Application.Services;
using System;

namespace FluentCMS.Plugins;

public interface IPluginAppService : ICrudAppService<Plugin, Guid, GetPluginRequest, PluginCreateRequest, PluginUpdateRequest>
{
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

public class PluginUpdateRequest : ExtensibleEntityDto
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
