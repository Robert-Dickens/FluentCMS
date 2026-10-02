using ByteLabs.Foundations.Domain.Repositories;
using System;

namespace FluentCMS.Domain.ApiTokens;

public interface IApiTokenModelRepository : IRepository<ApiTokenModel, Guid>
{

}