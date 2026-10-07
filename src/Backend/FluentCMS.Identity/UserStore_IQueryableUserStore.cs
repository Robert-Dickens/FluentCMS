using ByteLabs.Foundations.Threading;
using Microsoft.AspNetCore.Identity;

namespace FluentCMS.Identity;

public partial class UserStore : IQueryableUserStore<UserModel>
{
    public IQueryable<UserModel> Users => AsyncHelper.RunSync(()=> repository.GetQueryableAsync());
}
