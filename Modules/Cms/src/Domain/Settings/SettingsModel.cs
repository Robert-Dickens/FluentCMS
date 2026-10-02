using ByteLabs.Foundations.Domain.Entities.Auditing;
using ByteLabs.Foundations.MultiTenancy;
using System;
using System.Collections.Generic;

namespace FluentCMS.Domain.Settings;

public class SettingsModel : FullAuditedEntity<Guid>, IMultiTenant
{
    public Guid? TenantId { get; set; }
    public ICollection<SettingValuesModel> Values { get; set; } = []; // Navigation property

    public SettingsModel() : base(Guid.NewGuid())
    {

    }

}
