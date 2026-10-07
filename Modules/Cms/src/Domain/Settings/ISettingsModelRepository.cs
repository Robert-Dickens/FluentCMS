using ByteLabs.Foundations.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCMS.Domain.Settings;

public interface ISettingsModelRepository : IRepository<SettingsModel, Guid>
{
    Task<ICollection<SettingsModel>> GetByIds(IEnumerable<Guid> entityIds, CancellationToken cancellationToken = default);
    Task<SettingsModel> Update(Guid entityId, Dictionary<string, string> values, CancellationToken cancellationToken = default);
}