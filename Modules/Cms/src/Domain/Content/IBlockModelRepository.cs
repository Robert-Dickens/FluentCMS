using ByteLabs.Foundations.Domain.Repositories;
using System;

namespace FluentCMS.Domain.Content;

public interface IBlockModelRepository : IRepository<BlockModel, Guid>
{

}