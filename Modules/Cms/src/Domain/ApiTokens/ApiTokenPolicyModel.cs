using ByteLabs.Foundations.Domain.Entities;
using System;

namespace FluentCMS.Domain.ApiTokens;

public class ApiTokenPolicyModel: Entity<Guid>
{
    public Guid ApiTokenId { get; set; } // foreign key
    public string Area { get; set; } = default!;
    public string Actions { get; set; } = default!; // comma separated list of actions
    public ApiTokenModel ApiToken { get; set; } = default!; // navigation property

    public ApiTokenPolicyModel() : base(Guid.NewGuid())
    {

    }

}