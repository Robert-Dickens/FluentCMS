using System;

namespace FluentCMS.ApiTokens;

public interface IApiTokenAppService : ICrudAppService<ApiToken, Guid, GetApiTokenRequest, ApiTokenCreateRequest, ApiTokenUpdateRequest>
{
}

public class ApiToken : FullAuditedEntityDto<Guid>
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime? ExpireAt { get; set; }
    public bool Enabled { get; set; }
}

public class GetApiTokenRequest : FilteredAndPagedAndSortedResultRequestDto
{
}

public class ApiTokenCreateRequest : ExtensibleEntityDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Key { get; set; } = string.Empty;
    public string Secret { get; set; } = string.Empty;
    public DateTime? ExpireAt { get; set; }
    public bool Enabled { get; set; } = true;
}

public class ApiTokenUpdateRequest : ExtensibleEntityDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime? ExpireAt { get; set; }
    public bool Enabled { get; set; } = true;
}
