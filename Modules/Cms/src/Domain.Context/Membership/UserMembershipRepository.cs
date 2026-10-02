using ByteLabs.Foundations.EntityFrameworkCore;
using ByteLabs.PlatformServices.Users.Domain;

namespace FluentCMS.Domain.Membership;

public class UserMembershipRepository : EfCoreUserRepositoryBase<IFluentCMSDbContext, UserModel>
{
    public UserMembershipRepository(IDbContextProvider<IFluentCMSDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }
}
