using ByteLabs.Foundations.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCMS.Domain.Membership;

public interface IUserMembershipRepository : IRepository<UserModel, Guid>
{
    Task<IList<UserModel>> GetUsersForClaim(Claim claim, CancellationToken cancellationToken = default);
    Task<UserModel?> FindByEmail(string normalizedEmail, CancellationToken cancellationToken = default);
    Task<UserModel?> FindByLogin(string loginProvider, string providerKey, CancellationToken cancellationToken = default);
    Task<UserModel?> FindByName(string normalizedUserName, CancellationToken cancellationToken = default);

}
