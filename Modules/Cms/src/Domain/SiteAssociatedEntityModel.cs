using ByteLabs.Foundations.Auditing;
using ByteLabs.Foundations.Data;
using ByteLabs.Foundations.Domain.Entities;
using ByteLabs.Foundations.Domain.Entities.Auditing;
using ByteLabs.Foundations.MultiTenancy;
using ByteLabs.Foundations.ObjectExtending;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FluentCMS.Domain;

public interface ISiteAssociatedEntityModel : IFullAuditedObject, IEntity<Guid>, IMultiTenant, IHasExtraProperties
{
    Guid SiteId { get; set; }
}

public abstract class SiteAssociatedEntityModel : FullAuditedEntity<Guid>, ISiteAssociatedEntityModel
{
    public Guid SiteId { get; set; }
    public Guid? TenantId { get; set; }

    public virtual ExtraPropertyDictionary ExtraProperties { get; protected set; }

    protected SiteAssociatedEntityModel():this(Guid.NewGuid())
    {
    }

    protected SiteAssociatedEntityModel(Guid id) : base(id)
    {
        ExtraProperties = new ExtraPropertyDictionary();
        this.SetDefaultsForExtraProperties();
    }



    public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        return ExtensibleObjectValidator.GetValidationErrors(this, validationContext);
    }

}
