using Microsoft.AspNetCore.Identity;

namespace FluentCMS.Identity;

public partial class UserStore(IUserMembershipRepository repository) : IProtectedUserStore<UserModel>
{

}
