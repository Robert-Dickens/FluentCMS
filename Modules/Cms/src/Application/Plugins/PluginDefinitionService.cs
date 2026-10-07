using ByteLabs.Foundations.DependencyInjection;
using FluentCMS.Domain.Plugins;
using FluentCMS.Plugins;
using FluentCMS.Providers.MessageBusProviders;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCMS.Services.Plugins;

public class PluginDefinitionService(IPluginDefinitionModelRepository pluginDefinitionRepository, IMessagePublisher messagePublisher) : FluentCMSServiceAppService, IPluginDefinitionService, ITransientDependency
{
    public async Task<PluginDefinition> Create(PluginDefinition pluginDefinition, CancellationToken cancellationToken = default)
    {
        var created = await pluginDefinitionRepository.InsertAsync(ObjectMapper.Map<PluginDefinition, PluginDefinitionModel>(pluginDefinition), cancellationToken: cancellationToken) ??
            throw new AppException(ExceptionCodes.PluginDefinitionUnableToCreate);

        var results = ObjectMapper.Map<PluginDefinitionModel, PluginDefinition>(created);

        await messagePublisher.Publish(new Message<PluginDefinition>(ActionNames.PluginDefinitionCreated, results), cancellationToken);

        return results;
    }

    public async Task<PluginDefinition> Delete(Guid id, CancellationToken cancellationToken = default)
    {
        var deleted = await pluginDefinitionRepository.DeleteWithResultAsync(id, cancellationToken: cancellationToken) ??
            throw new AppException(ExceptionCodes.PluginDefinitionUnableToDelete);

        var results = ObjectMapper.Map<PluginDefinitionModel, PluginDefinition>(deleted);
        await messagePublisher.Publish(new Message<PluginDefinition>(ActionNames.PluginDefinitionDeleted, results), cancellationToken);

        return results;
    }

    public async Task<PluginDefinition> GetById(Guid id, CancellationToken cancellationToken = default)
    {
        var results = await pluginDefinitionRepository.FindAsync(id, cancellationToken: cancellationToken) ??
            throw new AppException(ExceptionCodes.PluginDefinitionNotFound);
        return ObjectMapper.Map<PluginDefinitionModel, PluginDefinition>(results);
    }

    public async Task<IEnumerable<PluginDefinition>> GetAll(CancellationToken cancellationToken = default)
    {
        var results = await pluginDefinitionRepository.GetListAsync(cancellationToken: cancellationToken);
        return ObjectMapper.Map<List<PluginDefinitionModel>, List<PluginDefinition>>(results);
    }

    public async Task<PluginDefinition> Update(PluginDefinition pluginDefinition, CancellationToken cancellationToken = default)
    {
        var updated = await pluginDefinitionRepository.UpdateAsync(ObjectMapper.Map<PluginDefinition, PluginDefinitionModel>(pluginDefinition), cancellationToken: cancellationToken) ??
            throw new AppException(ExceptionCodes.PluginDefinitionUnableToUpdate);

        var results = ObjectMapper.Map<PluginDefinitionModel, PluginDefinition>(updated);
        await messagePublisher.Publish(new Message<PluginDefinition>(ActionNames.PluginDefinitionUpdated, results), cancellationToken);

        return results;
    }
}
