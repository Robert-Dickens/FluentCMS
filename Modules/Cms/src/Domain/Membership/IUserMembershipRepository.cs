using ByteLabs.Foundations.Domain.Repositories;
using System;

namespace FluentCMS.Domain.Membership;

public interface IUserMembershipRepository : IRepository<UserModel, Guid>
{
}
