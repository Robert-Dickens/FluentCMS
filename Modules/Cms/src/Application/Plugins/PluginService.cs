using ByteLabs.Foundations.DependencyInjection;
using FluentCMS.Domain.Content;
using FluentCMS.Domain.Plugins;
using FluentCMS.Plugins;
using FluentCMS.Providers.MessageBusProviders;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCMS.Services.Plugins;

public class PluginService(IPluginModelRepository pluginRepository, IPageModelRepository pageRepository, IMessagePublisher messagePublisher) : FluentCMSServiceAppService, IPluginService, ITransientDependency
{
    public async Task<Plugin> Create(Plugin plugin, CancellationToken cancellationToken = default)
    {
        //if (!await permissionManager.HasAccess(page, PermissionActionNames.PageContributor, cancellationToken))
        //    throw new AppException(ExceptionCodes.PermissionDenied);

        var created = await pluginRepository.InsertAsync(ObjectMapper.Map<Plugin, PluginModel>(plugin), cancellationToken: cancellationToken) ??
            throw new AppException(ExceptionCodes.PluginUnableToCreate);

        var results = ObjectMapper.Map<PluginModel, Plugin>(created);
        await messagePublisher.Publish(new Message<Plugin>(ActionNames.PluginCreated, results), cancellationToken);

        return results;
    }

    public async Task<Plugin> InitialCreate(Guid pageId, Guid pluginDefinitionId, string section, CancellationToken cancellationToken = default)
    {
        var page = await pageRepository.FindAsync(pageId, cancellationToken: cancellationToken) ??
            throw new AppException(ExceptionCodes.PageNotFound);

        var plugin = new Plugin
        {
            PageId = pageId,
            DefinitionId = pluginDefinitionId,
            SiteId = page.SiteId,
            Section = string.IsNullOrWhiteSpace(section) ? "main" : section,
            Cols = 12
        };

        return await Create(plugin, cancellationToken);
    }

    public async Task<Plugin> Delete(Guid id, CancellationToken cancellationToken = default)
    {
        var plugin = await pluginRepository.FindAsync(id, cancellationToken: cancellationToken) ??
            throw new AppException(ExceptionCodes.PluginNotFound);

        //if (!await permissionManager.HasAccess(plugin, PermissionActionNames.PluginContributor, cancellationToken))
        //    throw new AppException(ExceptionCodes.PermissionDenied);

        var deleted = await pluginRepository.DeleteWithResultAsync(id, cancellationToken: cancellationToken) ??
            throw new AppException(ExceptionCodes.PluginUnableToDelete);

        var results = ObjectMapper.Map<PluginModel, Plugin>(deleted);
        await messagePublisher.Publish(new Message<Plugin>(ActionNames.PluginDeleted, results), cancellationToken);

        return results;
    }

    public async Task<Plugin> GetById(Guid id, CancellationToken cancellationToken = default)
    {
        var plugin = await pluginRepository.FindAsync(id, cancellationToken: cancellationToken) ??
            throw new AppException(ExceptionCodes.PluginNotFound);

        //if (!await permissionManager.HasAccess(plugin, PermissionActionNames.PluginView, cancellationToken))
        //    throw new AppException(ExceptionCodes.PermissionDenied);

        return ObjectMapper.Map<PluginModel, Plugin>(plugin);
    }

    public async Task<IEnumerable<Plugin>> GetByPageId(Guid pageId, CancellationToken cancellationToken = default)
    {
        var pagePlugins = await pluginRepository.GetByPageId(pageId, cancellationToken);

        //var plugins = await permissionManager.HasAccess(pagePlugins, PermissionActionNames.PluginView, cancellationToken);

        return ObjectMapper.Map<IEnumerable<PluginModel>, List<Plugin>>(pagePlugins);
    }

    public async Task<Plugin> Update(Plugin plugin, CancellationToken cancellationToken = default)
    {
        //if (!await permissionManager.HasAccess(plugin, PermissionActionNames.PluginContributor, cancellationToken))
        //    throw new AppException(ExceptionCodes.PermissionDenied);

        var updated = await pluginRepository.UpdateAsync(ObjectMapper.Map<Plugin, PluginModel>(plugin), cancellationToken: cancellationToken) ??
            throw new AppException(ExceptionCodes.PluginUnableToUpdate);

        var results = ObjectMapper.Map<PluginModel, Plugin>(updated);
        await messagePublisher.Publish(new Message<Plugin>(ActionNames.PluginUpdated, results), cancellationToken);

        return results;
    }

    public async Task<IEnumerable<Plugin>> UpdateOrders(IEnumerable<PluginOrder> pluginOrders, CancellationToken cancellationToken = default)
    {
        var order = 0;
        var plugins = new List<Plugin>();
        foreach (var pluginOrder in pluginOrders)
        {
            var plugin = await pluginRepository.UpdateOrder(pluginOrder.Id, pluginOrder.Section, order, cancellationToken);
            if (plugin != null)
            {
                plugins.Add(ObjectMapper.Map<PluginModel, Plugin>(plugin));
                order++;
            }
        }
        return plugins;
    }

    public async Task<Plugin> UpdateCols(Guid pluginId, int cols, int colsMd, int colsLg, CancellationToken cancellationToken = default)
    {
        var updated = await pluginRepository.UpdateCols(pluginId, cols, colsMd, colsLg, cancellationToken) ??
            throw new AppException(ExceptionCodes.PluginUnableToUpdateCols);
        return ObjectMapper.Map<PluginModel, Plugin>(updated);
    }

}
