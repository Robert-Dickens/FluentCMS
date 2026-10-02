using ByteLabs.Foundations.Domain.Entities;
using ByteLabs.Foundations.MultiTenancy;
using System;

namespace FluentCMS.Domain.Settings;

public class SettingValuesModel : Entity<Guid>, IMultiTenant
{
    public Guid? TenantId { get; set; }
    public Guid SettingId { get; set; } // Foreign key
    public string Key { get; set; } = default!;
    public string Value { get; set; } = default!;

    public SettingsModel Setting { get; set; } = default!; // Navigation property

    public SettingValuesModel() : base(Guid.NewGuid())
    {

    }

}
