using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCMS.Plugins;

public interface IPluginContentAppService : ICrudAppService<PluginContent, Guid, GetPluginContentRequest, PluginContentCreateRequest, PluginContentUpdateRequest>
{
}

public interface IPluginContentService : IApplicationService
{
    Task<PluginContent> Create(PluginContent pluginContent, CancellationToken cancellationToken = default);
    Task<PluginContent> Delete(string pluginContentTypeName, Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<PluginContent>> GetByPluginId(Guid pluginId, CancellationToken cancellationToken = default);
    Task<PluginContent> GetById(string pluginContentTypeName, Guid id, CancellationToken cancellationToken = default);
    Task<PluginContent> Update(PluginContent pluginContent, CancellationToken cancellationToken);
}


public class PluginContent : ExtensibleFullAuditedEntityDto<Guid>
{
    public Guid SiteId { get; set; }
    public Guid PluginId { get; set; }
    public string Type { get; set; } = string.Empty;
    public Dictionary<string, object?> Data { get; set; } = [];
}

public class PluginContentDetailResponse : PluginContent
{
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
