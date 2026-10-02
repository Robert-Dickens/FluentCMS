using ByteLabs.Foundations.Application.Services;
using System;

namespace FluentCMS.Plugins;

public interface IPluginContentAppService : ICrudAppService<PluginContent, Guid, GetPluginContentRequest, PluginContentCreateRequest, PluginContentUpdateRequest>
{
}

public class PluginContent : ExtensibleFullAuditedEntityDto<Guid>
{
    public Guid SiteId { get; set; }
    public Guid PluginId { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Data { get; set; } = string.Empty;
}

public class GetPluginContentRequest : FilteredAndPagedAndSortedResultRequestDto
{
}

public class PluginContentCreateRequest : ExtensibleEntityDto
{
    public Guid SiteId { get; set; }
    public Guid PluginId { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Data { get; set; } = string.Empty;
}

public class PluginContentUpdateRequest : ExtensibleEntityDto
{
    public Guid SiteId { get; set; }
    public Guid PluginId { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Data { get; set; } = string.Empty;
}
