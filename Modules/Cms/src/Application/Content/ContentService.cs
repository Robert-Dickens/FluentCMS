using ByteLabs.Foundations.DependencyInjection;
using FluentCMS.Content;
using FluentCMS.Domain.Content;
using FluentCMS.Providers.MessageBusProviders;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCMS.Services.Content;

public class ContentService(IContentModelRepository contentRepository, IMessagePublisher messagePublisher) : FluentCMSServiceAppService, IContentService, ITransientDependency
{
    public async Task<IEnumerable<FluentCMS.Content.Content>> GetAllByContentType(Guid contentTypeId, CancellationToken cancellationToken = default)
    {
        var results = await contentRepository.GetAllByContentType(contentTypeId, cancellationToken);
        return ObjectMapper.Map<IEnumerable<ContentModel>, List<FluentCMS.Content.Content>>(results);
    }

    public async Task<FluentCMS.Content.Content> Create(FluentCMS.Content.Content content, CancellationToken cancellationToken = default)
    {
        var contentEntity = ObjectMapper.Map<FluentCMS.Content.Content, ContentModel>(content);
        var newContent = await contentRepository.InsertAsync(contentEntity, cancellationToken: cancellationToken) ??
            throw new AppException(ExceptionCodes.ContentUnableToCreate);

        var results = ObjectMapper.Map<ContentModel, FluentCMS.Content.Content>(newContent);

        await messagePublisher.Publish(new Message<FluentCMS.Content.Content>(ActionNames.ContentCreated, results), cancellationToken);

        return results;
    }

    public async Task<FluentCMS.Content.Content> Delete(Guid id, CancellationToken cancellationToken = default)
    {
        var deleted = await contentRepository.DeleteWithResultAsync(id, cancellationToken: cancellationToken) ??
            throw new AppException(ExceptionCodes.ContentUnableToDelete);

        var results = ObjectMapper.Map<ContentModel, FluentCMS.Content.Content>(deleted);

        await messagePublisher.Publish(new Message<FluentCMS.Content.Content>(ActionNames.ContentDeleted, results), cancellationToken);

        return results;
    }

    public async Task<FluentCMS.Content.Content> Update(FluentCMS.Content.Content content, CancellationToken cancellationToken = default)
    {
        var contentEntity = ObjectMapper.Map<FluentCMS.Content.Content, ContentModel>(content);
        var updated = await contentRepository.UpdateAsync(contentEntity, cancellationToken: cancellationToken) ??
            throw new AppException(ExceptionCodes.ContentUnableToUpdate);

        var results = ObjectMapper.Map<ContentModel, FluentCMS.Content.Content>(updated);

        await messagePublisher.Publish(new Message<FluentCMS.Content.Content>(ActionNames.ContentUpdated, results), cancellationToken);

        return results;
    }

    public async Task<FluentCMS.Content.Content> GetById(Guid id, CancellationToken cancellationToken = default)
    {
        var results = await contentRepository.FindAsync(id, cancellationToken: cancellationToken) ??
            throw new AppException(ExceptionCodes.ContentNotFound);
        return ObjectMapper.Map<ContentModel, FluentCMS.Content.Content>(results);
    }
}
