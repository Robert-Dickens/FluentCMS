using FluentCMS.Domain.Content;
using FluentCMS.Providers.MessageBusProviders;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCMS.Services.Content;

[Authorize]
public class LayoutAppService : FluentCMSCrudAppService<LayoutModel, FluentCMS.Content.Layout, Guid, GetLayoutRequest, LayoutCreateRequest, LayoutUpdateRequest>, ILayoutAppService
{
    private readonly IMessagePublisher _messagePublisher;

    public LayoutAppService(ILayoutModelRepository repository, IMessagePublisher messagePublisher) : base(repository)
    {
        _messagePublisher = messagePublisher;
    }

    public override async Task<FluentCMS.Content.Layout> OnAfterCreateAsync(FluentCMS.Content.Layout input, CancellationToken cancellationToken = default)
    {
        await _messagePublisher.Publish(new Message<FluentCMS.Content.Layout>(ActionNames.LayoutCreated, input), cancellationToken);
        return await base.OnAfterCreateAsync(input, cancellationToken);
    }

    public override async Task<FluentCMS.Content.Layout> OnAfterUpdateAsync(FluentCMS.Content.Layout input, CancellationToken cancellationToken = default)
    {
        await _messagePublisher.Publish(new Message<FluentCMS.Content.Layout>(ActionNames.LayoutUpdated, input), cancellationToken);
        return await base.OnAfterUpdateAsync(input, cancellationToken);
    }

    public override async Task OnAfterDeleteAsync(FluentCMS.Content.Layout model, CancellationToken cancellationToken = default)
    {
        await _messagePublisher.Publish(new Message<FluentCMS.Content.Layout>(ActionNames.LayoutDeleted, model), cancellationToken);
    }
}
