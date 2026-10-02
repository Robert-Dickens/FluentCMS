using FluentCMS.Content;
using FluentCMS.Domain.Content;
using FluentCMS.Permissions;
using FluentCMS.Providers.MessageBusProviders;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCMS.Services.Content;

[Authorize(FluentCMSPermissions.SiteContent.Content.Default)]
public class ContentAppService : FluentCMSCrudAppService<ContentModel, FluentCMS.Content.Content, Guid, GetContentRequest, ContentCreateRequest, ContentUpdateRequest>, IContentAppService
{
    private readonly IMessagePublisher _messagePublisher;
    public ContentAppService(IContentModelRepository repository, IMessagePublisher messagePublisher) : base(repository)
    {

        CreatePolicyName = FluentCMSPermissions.SiteContent.Content.Create;
        UpdatePolicyName = FluentCMSPermissions.SiteContent.Content.Edit;
        DeletePolicyName = FluentCMSPermissions.SiteContent.Content.Delete;

        _messagePublisher = messagePublisher;
    }

    public async IAsyncEnumerable<FluentCMS.Content.Content> GetAllByContentType(Guid contentTypeId, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var records = await ((IContentModelRepository)Repository).GetAllByContentType(contentTypeId, cancellationToken);

        await foreach (var record in records.ToAsyncEnumerable())
        {
            yield return ObjectMapper.Map<ContentModel, FluentCMS.Content.Content>(record);
        }
    }

    public override async Task<FluentCMS.Content.Content> OnAfterCreateAsync(FluentCMS.Content.Content input, CancellationToken cancellationToken = default)
    {
        await _messagePublisher.Publish(new Message<FluentCMS.Content.Content>(ActionNames.ContentCreated, input), cancellationToken);
        return await base.OnAfterCreateAsync(input, cancellationToken);
    }

    public override async Task<FluentCMS.Content.Content> OnAfterUpdateAsync(FluentCMS.Content.Content input, CancellationToken cancellationToken = default)
    {
        await _messagePublisher.Publish(new Message<FluentCMS.Content.Content>(ActionNames.ContentUpdated, input), cancellationToken);
        return await base.OnAfterUpdateAsync(input, cancellationToken);
    }

    public override async Task OnAfterDeleteAsync(FluentCMS.Content.Content model, CancellationToken cancellationToken = default)
    {
        await _messagePublisher.Publish(new Message<FluentCMS.Content.Content>(ActionNames.ContentDeleted, model), cancellationToken);
    }

}
