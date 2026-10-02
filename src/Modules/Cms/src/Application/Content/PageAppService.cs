using FluentCMS.Domain.Content;
using FluentCMS.Providers.MessageBusProviders;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCMS.Services.Content;

[Authorize]
public class PageAppService : FluentCMSCrudAppService<PageModel, FluentCMS.Content.Page, Guid, GetPageRequest, PageCreateRequest, PageUpdateRequest>, IPageAppService
{
    private readonly IMessagePublisher _messagePublisher;

    public PageAppService(IPageModelRepository repository, IMessagePublisher messagePublisher) : base(repository)
    {
        _messagePublisher = messagePublisher;
    }

    public override async Task<FluentCMS.Content.Page> OnAfterCreateAsync(FluentCMS.Content.Page input, CancellationToken cancellationToken = default)
    {
        await _messagePublisher.Publish(new Message<FluentCMS.Content.Page>(ActionNames.PageCreated, input), cancellationToken);
        return await base.OnAfterCreateAsync(input, cancellationToken);
    }

    public override async Task<FluentCMS.Content.Page> OnAfterUpdateAsync(FluentCMS.Content.Page input, CancellationToken cancellationToken = default)
    {
        await _messagePublisher.Publish(new Message<FluentCMS.Content.Page>(ActionNames.PageUpdated, input), cancellationToken);
        return await base.OnAfterUpdateAsync(input, cancellationToken);
    }

    public override async Task OnAfterDeleteAsync(FluentCMS.Content.Page model, CancellationToken cancellationToken = default)
    {
        await _messagePublisher.Publish(new Message<FluentCMS.Content.Page>(ActionNames.PageDeleted, model), cancellationToken);
    }
}
