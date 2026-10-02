using ByteLabs.Foundations.Domain.Repositories;
using System;

namespace FluentCMS.Domain.Content;

public interface IContentTypeModelRepository : IRepository<ContentTypeModel, Guid>
{

}