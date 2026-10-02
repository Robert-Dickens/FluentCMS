namespace FluentCMS.DataSeeders;

public class KnownTenants
{
    public static Guid?[] TenantIds = [null, Guid.NewGuid()];


    public static Guid TenantId
    {
        get
        {
            return TenantIds.First(x => x.HasValue).Value;
        }
    }
}
