using ByteLabs.Foundations.DependencyInjection;
using FluentCMS.Content;
using FluentCMS.Domain.Content;
using FluentCMS.Permissions;
using FluentCMS.Providers.MessageBusProviders;
using FluentCMS.Services.Permissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCMS.Services.Content;

public class LayoutService(ILayoutModelRepository layoutRepository, ISiteModelRepository siteRepository, IMessagePublisher messagePublisher, IPermissionManager permissionManager) : FluentCMSServiceAppService, ILayoutService, ITransientDependency
{
    public async Task<Layout> GetById(Guid id, CancellationToken cancellationToken = default)
    {
        var layout = await layoutRepository.FindAsync(id, cancellationToken: cancellationToken) ?? throw new AppException(ExceptionCodes.LayoutNotFound);

        return ObjectMapper.Map<LayoutModel, Layout>(layout);
    }

    public async Task<IEnumerable<Layout>> GetAll(CancellationToken cancellationToken = default)
    {
        var layouts = await layoutRepository.GetListAsync(cancellationToken: cancellationToken);
        return ObjectMapper.Map<IEnumerable<LayoutModel>, List<Layout>>(layouts);
    }

    public async Task<Layout> Create(Layout layout, CancellationToken cancellationToken = default)
    {
        if (!await permissionManager.HasAccess(layout.SiteId, SitePermissionAction.SiteAdmin, cancellationToken))
            throw new AppException(ExceptionCodes.PermissionDenied);

        var layoutEntity = ObjectMapper.Map<Layout, LayoutModel>(layout);

        var created = await layoutRepository.InsertAsync(layoutEntity, cancellationToken: cancellationToken) ??
            throw new AppException(ExceptionCodes.LayoutUnableToCreate);

        var results = ObjectMapper.Map<LayoutModel, Layout>(created);
        await messagePublisher.Publish(new Message<Layout>(ActionNames.LayoutCreated, results), cancellationToken);

        return results;
    }

    public async Task<Layout> Update(Layout layout, CancellationToken cancellationToken)
    {
        var existing = await layoutRepository.FindAsync(layout.Id, cancellationToken: cancellationToken) ??
            throw new AppException(ExceptionCodes.LayoutNotFound);

        if (!await permissionManager.HasAccess(existing.SiteId, SitePermissionAction.SiteAdmin, cancellationToken))
            throw new AppException(ExceptionCodes.PermissionDenied);

        var layoutEntity = ObjectMapper.Map<Layout, LayoutModel>(layout);
        var updated = await layoutRepository.UpdateAsync(layoutEntity, cancellationToken: cancellationToken) ??
            throw new AppException(ExceptionCodes.LayoutUnableToUpdate);

        var results = ObjectMapper.Map<LayoutModel, Layout>(updated);
        await messagePublisher.Publish(new Message<Layout>(ActionNames.LayoutUpdated, results), cancellationToken);

        return results;
    }

    public async Task<Layout> Delete(Guid id, CancellationToken cancellationToken = default)
    {
        var existing = await layoutRepository.FindAsync(id, cancellationToken: cancellationToken) ??
            throw new AppException(ExceptionCodes.LayoutNotFound);

        var siteId = existing.SiteId;

        if (!await permissionManager.HasAccess(siteId, SitePermissionAction.SiteAdmin, cancellationToken))
            throw new AppException(ExceptionCodes.PermissionDenied);

        // check if the layout is one of site's default layout, if so, throw an exception
        var site = await siteRepository.FindAsync(siteId, cancellationToken: cancellationToken) ??
            throw new AppException(ExceptionCodes.SiteNotFound);

        if (site.LayoutId == id || site.EditLayoutId == id || site.DetailLayoutId == id)
            throw new AppException(ExceptionCodes.LayoutUnableToDeleteDefaultLayout);

        var deleted = await layoutRepository.DeleteWithResultAsync(id, cancellationToken: cancellationToken) ??
            throw new AppException(ExceptionCodes.LayoutUnableToDelete);

        var results = ObjectMapper.Map<LayoutModel, Layout>(deleted);
        await messagePublisher.Publish(new Message<Layout>(ActionNames.LayoutDeleted, results), cancellationToken);

        return results;
    }

    public async Task<IEnumerable<Layout>> GetAllForSite(Guid siteId, CancellationToken cancellationToken = default)
    {
        var results = await layoutRepository.GetAllForSite(siteId, cancellationToken);
        return ObjectMapper.Map<IEnumerable<LayoutModel>, List<Layout>>(results);
    }

    public async Task<IEnumerable<Layout>> DeleteBySiteId(Guid siteId, CancellationToken cancellationToken = default)
    {
        if (!await permissionManager.HasAccess(siteId, SitePermissionAction.SiteAdmin, cancellationToken))
            throw new AppException(ExceptionCodes.PermissionDenied);

        var layouts = await layoutRepository.GetAllForSite(siteId, cancellationToken);

        var deleted = await layoutRepository.DeleteManyWithResultAsync(layouts.Select(x => x.Id), cancellationToken: cancellationToken);

        var results = ObjectMapper.Map<IEnumerable<LayoutModel>, List<Layout>>(deleted);
        // creating tasks for the message publisher and await them all to finish
        var tasks = results?.Select(item => messagePublisher.Publish(new Message<Layout>(ActionNames.LayoutDeleted, item), cancellationToken)).ToList() ?? [];

        await Task.WhenAll(tasks);

        return results;
    }
}
