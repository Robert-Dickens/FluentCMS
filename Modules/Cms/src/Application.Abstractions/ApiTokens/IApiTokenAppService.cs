using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCMS.ApiTokens;

public interface IApiTokenAppService : ICrudAppService<ApiToken, Guid, GetApiTokenRequest, ApiTokenCreateRequest, ApiTokenUpdateRequest>
{
}

public interface IApiTokenService : IApplicationService
{
    Task<ApiToken> Create(ApiToken apiToken, CancellationToken cancellationToken = default);
    Task<ApiToken> Delete(Guid tokenId, CancellationToken cancellationToken = default);
    Task<IEnumerable<ApiToken>> GetAll(CancellationToken cancellationToken = default);
    Task<ApiToken> GetById(Guid tokenId, CancellationToken cancellationToken = default);
    Task<ApiToken> Update(ApiToken apiToken, CancellationToken cancellationToken = default);
    Task<ApiToken> RegenerateSecret(Guid id, CancellationToken cancellationToken = default);
    Task<ApiToken> Validate(string apiKey, string apiSecret, CancellationToken cancellationToken = default);
}

public class ApiToken : FullAuditedEntityDto<Guid>
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime? ExpireAt { get; set; }
    public bool Enabled { get; set; }
    public string Key { get; set; } = default!;
    public string Secret { get; set; } = default!;

    public List<Policy> Policies { get; set; } = [];

}

public class ApiTokenDetailResponse : ApiToken
{

}
public class Policy
{
    public string Area { get; set; } = default!;
    public List<string> Actions { get; set; } = [];
}

public class GetApiTokenRequest : FilteredAndPagedAndSortedResultRequestDto
{
}

public class ApiTokenCreateRequest : ExtensibleEntityDto
{
    [Required]
    public string Name { get; set; } = default!;
    public string? Description { get; set; }

    public DateTime? ExpireAt { get; set; }

    [Required]
    public bool Enabled { get; set; } = true;

    [Required]
    public List<Policy> Policies { get; set; } = [];
}

public class ApiTokenUpdateRequest : ExtensibleEntityDto
{
    [Required]
    public Guid Id { get; set; }

    [Required]
    public string Name { get; set; } = default!;

    [Required]
    public bool Enabled { get; set; } = true;

    [Required]
    public List<Policy> Policies { get; set; } = [];

    public string? Description { get; set; }
    public DateTime? ExpireAt { get; set; }
}
