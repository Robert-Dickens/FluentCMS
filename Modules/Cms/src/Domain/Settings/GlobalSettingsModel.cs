using ByteLabs.Foundations.Domain.Entities.Auditing;
using System;

namespace FluentCMS.Domain.Settings;

public class GlobalSettingsModel : FullAuditedEntity<Guid>
{
    public string SuperAdmins { get; set; } = default!; // comma separated user names

    public GlobalSettingsModel() : base(Guid.NewGuid())
    {

    }

}
