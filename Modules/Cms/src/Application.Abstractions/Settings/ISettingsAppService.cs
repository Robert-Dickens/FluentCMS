using System;

namespace FluentCMS.Settings;

public interface ISettingsAppService : ICrudAppService<Settings, Guid, GetSettingsRequest, SettingsCreateRequest, SettingsUpdateRequest>
{
}

public class Settings : FullAuditedEntityDto<Guid>
{
}

public class GetSettingsRequest : FilteredAndPagedAndSortedResultRequestDto
{
}

public class SettingsCreateRequest : ExtensibleEntityDto
{
}

public class SettingsUpdateRequest : ExtensibleEntityDto
{
}
