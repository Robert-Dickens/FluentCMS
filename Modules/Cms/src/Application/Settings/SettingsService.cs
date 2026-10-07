using ByteLabs.Foundations.DependencyInjection;
using FluentCMS.Domain.Settings;
using FluentCMS.Providers.MessageBusProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCMS.Services.Settings;

public class SettingsService(ISettingsModelRepository settingsRepository, IMessagePublisher messagePublisher) : FluentCMSServiceAppService, FluentCMS.Settings.ISettingsService, ITransientDependency
{
    public async Task<FluentCMS.Settings.Settings> GetById(Guid entityId, CancellationToken cancellationToken = default)
    {
        var entity = await settingsRepository.FindAsync(entityId, cancellationToken: cancellationToken);

        if (entity is null) return new FluentCMS.Settings.Settings { Id = entityId, Values = [] };
        return ObjectMapper.Map<SettingsModel, FluentCMS.Settings.Settings>(entity);
    }

    public async Task<IEnumerable<FluentCMS.Settings.Settings>> GetByIds(IEnumerable<Guid> entityIds, CancellationToken cancellationToken = default)
    {
        var entites = await settingsRepository.GetByIds(entityIds, cancellationToken);

        var result = ObjectMapper.Map<ICollection<SettingsModel>, List<FluentCMS.Settings.Settings>>(entites);
        // check if some ids are missing, then add them with empty values
        var missingIds = entityIds.Except(result.Select(x => x.Id));
        foreach (var id in missingIds)
            result.AddIfNotContains(new FluentCMS.Settings.Settings { Id = id, Values = [] });
        return result;
    }

    public async Task<FluentCMS.Settings.Settings> Update(Guid entityId, Dictionary<string, string> values, CancellationToken cancellationToken = default)
    {
        var settings = await settingsRepository.Update(entityId, values, cancellationToken) ??
            throw new AppException(ExceptionCodes.SettingsUnableToUpdate);

        var results = ObjectMapper.Map<SettingsModel, FluentCMS.Settings.Settings>(settings);

        await messagePublisher.Publish(new Message<FluentCMS.Settings.Settings>(ActionNames.SettingsUpdated, results), cancellationToken);

        return results;
    }
}
