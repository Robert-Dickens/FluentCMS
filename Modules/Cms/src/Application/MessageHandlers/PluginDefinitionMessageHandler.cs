using FluentCMS.Content;
using FluentCMS.Plugins;
using FluentCMS.Providers.MessageBusProviders;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCMS.Services.MessageHandlers;

public class PluginDefinitionMessageHandler(IPluginDefinitionService pluginDefinitionService) : IMessageHandler<SetupTemplate>
{
    public async Task Handle(Message<SetupTemplate> notification, CancellationToken cancellationToken)
    {
        switch (notification.Action)
        {
            case ActionNames.SetupStarted:
                foreach (var pluginDef in notification.Payload.PluginDefinitions)
                    await pluginDefinitionService.Create(pluginDef, cancellationToken);

                break;

            default:
                break;
        }
    }
}
