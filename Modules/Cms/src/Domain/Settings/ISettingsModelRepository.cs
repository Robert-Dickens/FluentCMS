using ByteLabs.Foundations.Domain.Repositories;
using System;

namespace FluentCMS.Domain.Settings;

public interface ISettingsModelRepository : IRepository<SettingsModel, Guid>
{

}