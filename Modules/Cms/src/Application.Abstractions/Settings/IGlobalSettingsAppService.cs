using System;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCMS.Settings;

public interface IGlobalSettingsAppService : ICrudAppService<GlobalSettings, Guid, GetGlobalSettingsRequest, GlobalSettingsCreateRequest, GlobalSettingsUpdateRequest>
{
    Task<GlobalSettings?> Get(CancellationToken cancellationToken = default);
}

public class GlobalSettings : FullAuditedEntityDto<Guid>
{
    public string SuperAdmins { get; set; } = string.Empty;
}

public class GetGlobalSettingsRequest : FilteredAndPagedAndSortedResultRequestDto
{
}

public class GlobalSettingsCreateRequest : ExtensibleEntityDto
{
    public string SuperAdmins { get; set; } = string.Empty;
}

public class GlobalSettingsUpdateRequest : ExtensibleEntityDto
{
    public string SuperAdmins { get; set; } = string.Empty;
}
