using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCMS.Settings;

public interface ISettingsAppService : ICrudAppService<Settings, Guid, GetSettingsRequest, SettingsCreateRequest, SettingsUpdateRequest>
{
}

public interface ISettingsService : IApplicationService
{
    Task<IEnumerable<FluentCMS.Settings.Settings>> GetByIds(IEnumerable<Guid> entityIds, CancellationToken cancellationToken = default);
    Task<FluentCMS.Settings.Settings> GetById(Guid entityId, CancellationToken cancellationToken = default);
    Task<FluentCMS.Settings.Settings> Update(Guid entityId, Dictionary<string, string> values, CancellationToken cancellationToken = default);
}

public class Settings : FullAuditedEntityDto<Guid>
{
    public Dictionary<string, string> Values { get; set; } = default!;
}

public class SettingsDetailResponse: Settings
{
    public Dictionary<string, string> Values { get; set; } = default!;
}


public class GetSettingsRequest : FilteredAndPagedAndSortedResultRequestDto
{
}

public class SettingsCreateRequest : ExtensibleEntityDto
{
}

public class SettingsUpdateRequest : ExtensibleEntityDto<Guid>
{
    [Required]
    public Dictionary<string, string> Settings { get; set; } = [];

}
