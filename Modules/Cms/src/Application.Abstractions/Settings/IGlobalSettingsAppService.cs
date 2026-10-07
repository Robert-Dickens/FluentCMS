using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCMS.Settings;

public interface IGlobalSettingsAppService : ICrudAppService<GlobalSettings, Guid, GetGlobalSettingsRequest, GlobalSettingsCreateRequest, GlobalSettingsUpdateRequest>
{
    Task<GlobalSettings?> Get(CancellationToken cancellationToken = default);
}

public interface IGlobalSettingsService : IApplicationService
{
    Task<GlobalSettings> Update(GlobalSettings settings, CancellationToken cancellationToken = default);
    Task<GlobalSettings> Get(CancellationToken cancellationToken = default);
}

public class GlobalSettings : FullAuditedEntityDto<Guid>
{
    public IEnumerable<string> SuperAdmins { get; set; } = [];
    public FileUploadConfig FileUpload { get; set; } = default!;
}

public class GlobalSettingsResponse: GlobalSettings
{

}

public class GetGlobalSettingsRequest : FilteredAndPagedAndSortedResultRequestDto
{
}

public class GlobalSettingsCreateRequest : ExtensibleEntityDto
{
    public IEnumerable<string> SuperAdmins { get; set; } = [];
}

public class GlobalSettingsUpdateRequest : ExtensibleEntityDto
{
    public IEnumerable<string> SuperAdmins { get; set; } = [];
}
