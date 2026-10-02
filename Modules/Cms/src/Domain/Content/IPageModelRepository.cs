using ByteLabs.Foundations.Domain.Repositories;
using System;

namespace FluentCMS.Domain.Content;

public interface IPageModelRepository : IRepository<PageModel, Guid>
{

}