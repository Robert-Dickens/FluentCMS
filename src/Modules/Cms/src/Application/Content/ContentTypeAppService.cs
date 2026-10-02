using FluentCMS.Domain.Content;
using FluentCMS.Providers.MessageBusProviders;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCMS.Services.Content;

[Authorize]
public class ContentTypeAppService : FluentCMSCrudAppService<ContentTypeModel, FluentCMS.Content.ContentType, Guid, GetContentTypeRequest, ContentTypeCreateRequest, ContentTypeUpdateRequest>, IContentTypeAppService
{
    private readonly IMessagePublisher _messagePublisher;

    public ContentTypeAppService(IContentTypeModelRepository repository, IMessagePublisher messagePublisher) : base(repository)
    {
        _messagePublisher = messagePublisher;
    }

    public override async Task<FluentCMS.Content.ContentType> OnAfterCreateAsync(FluentCMS.Content.ContentType input, CancellationToken cancellationToken = default)
    {
        await _messagePublisher.Publish(new Message<FluentCMS.Content.ContentType>(ActionNames.ContentTypeCreated, input), cancellationToken);
        return await base.OnAfterCreateAsync(input, cancellationToken);
    }

    public override async Task<FluentCMS.Content.ContentType> OnAfterUpdateAsync(FluentCMS.Content.ContentType input, CancellationToken cancellationToken = default)
    {
        await _messagePublisher.Publish(new Message<FluentCMS.Content.ContentType>(ActionNames.ContentTypeUpdated, input), cancellationToken);
        return await base.OnAfterUpdateAsync(input, cancellationToken);
    }

    public override async Task OnAfterDeleteAsync(FluentCMS.Content.ContentType model, CancellationToken cancellationToken = default)
    {
        await _messagePublisher.Publish(new Message<FluentCMS.Content.ContentType>(ActionNames.ContentTypeDeleted, model), cancellationToken);
    }
}
