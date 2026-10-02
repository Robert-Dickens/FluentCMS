using ByteLabs.Foundations.Domain.Entities.Auditing;
using ByteLabs.Foundations.MultiTenancy;
using System;

namespace FluentCMS.Domain.Content;

public class SiteModel : FullAuditedEntity<Guid>, IMultiTenant
{
    public Guid? TenantId { get; set; }
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public string Urls { get; set; } = default!; // comma separated
    public Guid LayoutId { get; set; }
    public Guid DetailLayoutId { get; set; }
    public Guid EditLayoutId { get; set; }


    public SiteModel():base(Guid.NewGuid())
    {
        
    }
}
