using ByteLabs.Foundations.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;

namespace FluentCMS.Domain.ApiTokens;

public class ApiTokenModel:FullAuditedEntity<Guid>
{
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public string Key { get; set; } = default!;
    public string Secret { get; set; } = default!;
    public DateTime? ExpireAt { get; set; }
    public bool Enabled { get; set; } = true;
    public ICollection<ApiTokenPolicyModel> Policies { get; set; } = [];

    public ApiTokenModel() : base(Guid.NewGuid())
    {

    }


}
