using ByteLabs.Foundations.DependencyInjection;
using FluentCMS.Content;
using FluentCMS.Domain.Content;
using FluentCMS.Providers.MessageBusProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCMS.Services.Content;

public class ContentTypeService(IContentTypeModelRepository contentTypeRepository, IMessagePublisher messagePublisher) : FluentCMSServiceAppService, IContentTypeService, ITransientDependency
{
    public async Task<IEnumerable<ContentType>> GetAll(Guid siteId, CancellationToken cancellationToken = default)
    {
        var results = await contentTypeRepository.GetAllForSite(siteId, cancellationToken);
        return ObjectMapper.Map<IEnumerable<ContentTypeModel>, List<ContentType>>(results);
    }

    public async Task<ContentType> GetBySlug(Guid siteId, string contentTypeSlug, CancellationToken cancellationToken = default)
    {
        var result = await contentTypeRepository.GetBySlug(siteId, contentTypeSlug, cancellationToken) ??
            throw new AppException(ExceptionCodes.ContentTypeNotFound);
        return ObjectMapper.Map<ContentTypeModel, ContentType>(result);
    }

    public async Task<ContentType> Create(ContentType contentType, CancellationToken cancellationToken = default)
    {
        await CheckDuplicateSlug(contentType.SiteId, contentType);

        var contentEntity = ObjectMapper.Map<ContentType, ContentTypeModel>(contentType);

        var created = await contentTypeRepository.InsertAsync(contentEntity, cancellationToken: cancellationToken) ??
            throw new AppException(ExceptionCodes.ContentTypeUnableToCreate);

        var results = ObjectMapper.Map<ContentTypeModel, ContentType>(created);

        await messagePublisher.Publish(new Message<ContentType>(ActionNames.ContentTypeCreated, results), cancellationToken);

        return results;
    }

    public async Task<ContentType> Delete(Guid contentTypeId, CancellationToken cancellationToken = default)
    {
        var deleted = await contentTypeRepository.DeleteWithResultAsync(contentTypeId, cancellationToken: cancellationToken) ??
            throw new AppException(ExceptionCodes.ContentTypeUnableToDelete);

        var results = ObjectMapper.Map<ContentTypeModel, ContentType>(deleted);

        await messagePublisher.Publish(new Message<ContentType>(ActionNames.ContentTypeDeleted, results), cancellationToken);

        return results;
    }

    public async Task<ContentType> Update(ContentType contentType, CancellationToken cancellationToken = default)
    {
        // only allow name and description to be updated
        var original = await contentTypeRepository.FindAsync(contentType.Id, cancellationToken: cancellationToken) ??
            throw new AppException(ExceptionCodes.ContentTypeNotFound);

        await CheckDuplicateSlug(original.SiteId, contentType);

        original.Title = contentType.Title;
        original.Description = contentType.Description;

        var updated = await contentTypeRepository.UpdateAsync(original, cancellationToken: cancellationToken) ??
            throw new AppException(ExceptionCodes.ContentTypeUnableToUpdate);

        var results = ObjectMapper.Map<ContentTypeModel, ContentType>(updated);

        await messagePublisher.Publish(new Message<ContentType>(ActionNames.ContentTypeUpdated, results), cancellationToken);

        return results;
    }

    private async Task CheckDuplicateSlug(Guid siteId, ContentType contentType)
    {
        var originalBySlug = await contentTypeRepository.GetBySlug(siteId, contentType.Slug);
        if (originalBySlug != null && originalBySlug.Id != contentType.Id) throw new AppException(ExceptionCodes.ContentTypeDuplicateSlug);
    }

    public async Task<ContentType> SetField(Guid contentTypeId, ContentTypeField field, CancellationToken cancellationToken = default)
    {
        // load the content type
        var contentType = await contentTypeRepository.FindAsync(contentTypeId, cancellationToken: cancellationToken) ??
            throw new AppException(ExceptionCodes.ContentTypeNotFound);

        // check the field exists
        var originalField = contentType.Fields.FirstOrDefault(f => f.Name == field.Name);

        if (originalField == null)
            contentType.Fields.Add(ObjectMapper.Map<ContentTypeField, ContentTypeFieldModel>(field));
        else
        {
            originalField.Type = field.Type;
            originalField.Settings = field.Settings;
            originalField.Name = field.Name;
            originalField.Required = field.Required;
            originalField.Unique = field.Unique;
            originalField.Label = field.Label;
        }

        var updated = await contentTypeRepository.UpdateAsync(contentType, cancellationToken: cancellationToken) ??
            throw new AppException(ExceptionCodes.ContentTypeUnableToUpdate);



        var results = ObjectMapper.Map<ContentTypeModel, ContentType>(updated);

        await messagePublisher.Publish(new Message<ContentType>(ActionNames.ContentTypeUpdated, results), cancellationToken);

        return results;
    }

    public async Task<ContentType> DeleteField(Guid contentTypeId, string name, CancellationToken cancellationToken = default)
    {
        // load the content type
        var contentType = await contentTypeRepository.FindAsync(contentTypeId, cancellationToken: cancellationToken) ??
            throw new AppException(ExceptionCodes.ContentTypeNotFound);

        // check the field exists
        var original = contentType.Fields.FirstOrDefault(f => f.Name == name) ??
            throw new AppException(ExceptionCodes.ContentTypeFieldNotFound);

        // remove the field
        contentType.Fields.Remove(original);

        //apply changes
        var updated = await contentTypeRepository.UpdateAsync(contentType, cancellationToken: cancellationToken) ??
            throw new AppException(ExceptionCodes.ContentTypeUnableToUpdate);

        var results = ObjectMapper.Map<ContentTypeModel, ContentType>(updated);

        await messagePublisher.Publish(new Message<ContentType>(ActionNames.ContentTypeUpdated, results), cancellationToken);

        return results;
    }

    public async Task<ContentType> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await contentTypeRepository.FindAsync(id, cancellationToken: cancellationToken) ??
               throw new AppException(ExceptionCodes.ContentTypeNotFound);
        return ObjectMapper.Map<ContentTypeModel, ContentType>(result);
    }

}
