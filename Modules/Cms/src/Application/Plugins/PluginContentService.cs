using ByteLabs.Foundations.DependencyInjection;
using FluentCMS.Domain.Plugins;
using FluentCMS.Plugins;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCMS.Services.Plugins;

public class PluginContentService(IPluginContentModelRepository repository) : FluentCMSServiceAppService, IPluginContentService, ITransientDependency
{
    public async Task<PluginContent> Create(PluginContent pluginContent, CancellationToken cancellationToken = default)
    {
        var results = await repository.InsertAsync(ObjectMapper.Map<PluginContent, PluginContentModel>(pluginContent), cancellationToken: cancellationToken) ??
            throw new AppException(ExceptionCodes.PluginContentUnableToCreate);
        return ObjectMapper.Map<PluginContentModel, PluginContent>(results);
    }

    public async Task<PluginContent> Delete(string pluginContentTypeName, Guid id, CancellationToken cancellationToken = default)
    {
        var results = await repository.DeleteWithResultAsync(id, cancellationToken: cancellationToken) ??
            throw new AppException(ExceptionCodes.PluginContentUnableToDelete);
        return ObjectMapper.Map<PluginContentModel, PluginContent>(results);
    }

    public async Task<PluginContent> GetById(string pluginContentTypeName, Guid id, CancellationToken cancellationToken = default)
    {
        var results = await repository.FindAsync(id, cancellationToken: cancellationToken) ??
            throw new AppException(ExceptionCodes.PluginContentNotFound);
        return ObjectMapper.Map<PluginContentModel, PluginContent>(results);
    }

    public async Task<IEnumerable<PluginContent>> GetByPluginId(Guid pluginId, CancellationToken cancellationToken = default)
    {
        var results = await repository.GetByPluginId(pluginId, cancellationToken);
        return ObjectMapper.Map<IEnumerable<PluginContentModel>, List<PluginContent>>(results);
    }

    public async Task<PluginContent> Update(PluginContent pluginContent, CancellationToken cancellationToken)
    {
        var results = await repository.UpdateAsync(ObjectMapper.Map<PluginContent, PluginContentModel>(pluginContent), cancellationToken: cancellationToken) ??
            throw new AppException(ExceptionCodes.PluginContentUnableToUpdate);
        return ObjectMapper.Map<PluginContentModel, PluginContent>(results);
    }
}
