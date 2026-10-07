using ByteLabs.Foundations.DependencyInjection;
using FluentCMS.Content;
using FluentCMS.Domain.Content;
using FluentCMS.Permissions;
using FluentCMS.Providers.MessageBusProviders;
using FluentCMS.Services.Permissions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCMS.Services.Content;

public class SetupService(IMessagePublisher messagePublisher, ISiteModelRepository setupRepository, IPermissionManager permissionManager) : FluentCMSServiceAppService, ISetupService, ITransientDependency
{
    public Task<IEnumerable<string>> GetTemplates(CancellationToken cancellationToken = default)
    {
        var templateFolders = Path.Combine(ServiceConstants.SetupTemplatesFolder);
        return Task.FromResult(Directory.GetDirectories(templateFolders).Select(x => new DirectoryInfo(x).Name));
    }

    public Task<bool> IsInitialized(CancellationToken cancellationToken = default)
    {
        return setupRepository.Initialized(cancellationToken);
    }

    public async Task<bool> Start(SetupTemplate setupTemplate, CancellationToken cancellationToken = default)
    {
        if (await setupRepository.Initialized(cancellationToken))
            throw new AppException(ExceptionCodes.SetupAlreadyInitialized);

        // initialize db if it's not initialized
        await setupRepository.InitializeDb(cancellationToken);

        if (!await permissionManager.HasAccess(GlobalPermissionAction.SuperAdmin, cancellationToken))
            throw new AppException(ExceptionCodes.PermissionDenied);

        var manifestFilePath = Path.Combine(ServiceConstants.SetupTemplatesFolder, setupTemplate.Template, ServiceConstants.SetupManifestFile);

        if (!System.IO.File.Exists(manifestFilePath))
            throw new AppException($"{ServiceConstants.SetupManifestFile} doesn't exist!");

        var jsonSerializerOptions = new JsonSerializerOptions();
        jsonSerializerOptions.Converters.Add(new DictionaryJsonConverter());

        var jsonSetupTemplate = await JsonSerializer.DeserializeAsync<SetupTemplate>(System.IO.File.OpenRead(manifestFilePath), jsonSerializerOptions) ??
               throw new AppException($"Failed to read/deserialize {ServiceConstants.SetupManifestFile}");

        setupTemplate.PluginDefinitions = jsonSetupTemplate.PluginDefinitions;

        setupTemplate.Site = new SiteTemplate
        {
            Url = setupTemplate.Url,
            Template = setupTemplate.Template
        };

        // since we need ids for the child object relations,
        // we should set ids for all entities manually
        SetIds(setupTemplate);

        await messagePublisher.Publish(new Message<SetupTemplate>(ActionNames.SetupStarted, setupTemplate), cancellationToken);
        await messagePublisher.Publish(new Message<SiteTemplate>(ActionNames.SetupInitializeSite, setupTemplate.Site), cancellationToken);
        await messagePublisher.Publish(new Message<SetupTemplate>(ActionNames.SetupCompleted, setupTemplate), cancellationToken);

        return true;
    }

    private static void SetIds(SetupTemplate setupTemplate)
    {
        foreach (var pluginDefinition in setupTemplate.PluginDefinitions)
            pluginDefinition.Id = Guid.NewGuid();
    }
}
