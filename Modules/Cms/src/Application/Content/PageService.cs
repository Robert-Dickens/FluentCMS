using ByteLabs.Foundations.Caching;
using ByteLabs.Foundations.DependencyInjection;
using ByteLabs.Foundations.ObjectMapping;
using FluentCMS.Content;
using FluentCMS.Domain.Content;
using FluentCMS.Permissions;
using FluentCMS.Providers.MessageBusProviders;
using FluentCMS.Services.Permissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using PageEntity = FluentCMS.Domain.Content.PageModel;
using PageModel = FluentCMS.Content.PageModel;

namespace FluentCMS.Services.Content;

public class PageService(IPageModelRepository pageRepository, IPageInternalService internalService, IMessagePublisher messagePublisher, IPermissionManager permissionManager) : FluentCMSServiceAppService, IPageService, ITransientDependency
{
    public async Task<PageModel> Create(Page page, CancellationToken cancellationToken = default)
    {
        if (!await permissionManager.HasAccess(page.SiteId, SitePermissionAction.SiteContributor, cancellationToken))
            throw new AppException(ExceptionCodes.PermissionDenied);

        ValidateAndNormalize(page);

        await ValidateParentPage(page, cancellationToken);

        await ValidateDuplicatePath(page, cancellationToken);

        var pageEntity = ObjectMapper.Map<Page, PageEntity>(page);

        var newPage = await pageRepository.InsertAsync(pageEntity, cancellationToken: cancellationToken) ??
            throw new AppException(ExceptionCodes.PageUnableToCreate);

        // Invalidate cache for the site after creating a page
        internalService.Invalidate(page.SiteId);

        var pageAdded = ObjectMapper.Map<PageEntity, Page>(newPage);

        await messagePublisher.Publish(new Message<Page>(ActionNames.PageCreated, pageAdded), cancellationToken);

        return await internalService.GetById(page.SiteId, page.Id, cancellationToken) ??
            throw new AppException(ExceptionCodes.PageUnableToCreate);
    }

    public async Task<IEnumerable<PageModel>> GetHierarchyBySiteId(Guid siteId, CancellationToken cancellationToken = default)
    {
        var pages = await internalService.GetHierarchyBySiteId(siteId, cancellationToken) ?? [];

        //if (!await permissionManager.HasAccess(page, PermissionActionNames.PageView, cancellationToken))
        //    throw new AppException(ExceptionCodes.PermissionDenied);

        return pages;
    }


    public async Task<PageModel> Update(Page page, CancellationToken cancellationToken = default)
    {
        //fetch original page from db
        var originalPage = await pageRepository.FindAsync(page.Id, cancellationToken: cancellationToken) ??
            throw new AppException(ExceptionCodes.PageNotFound);

        // site id cannot be changed
        page.SiteId = originalPage.SiteId;

        if (!await permissionManager.HasAccess(page.SiteId, SitePermissionAction.SiteContributor, cancellationToken))
            throw new AppException(ExceptionCodes.PermissionDenied);

        ValidateAndNormalize(page);

        await ValidateParentPage(page, cancellationToken);

        // Only validate url if user wants to change path
        if (page.Path != originalPage.Path)
            await ValidateDuplicatePath(page, cancellationToken);

        if (originalPage.Path == "/" && page.Path != "/")
            throw new AppException(ExceptionCodes.PageUnableToUpdateHome);

        var pageEntity = ObjectMapper.Map<Page, PageEntity>(page);

        var updatedPage = await pageRepository.UpdateAsync(pageEntity, cancellationToken: cancellationToken)
             ?? throw new AppException(ExceptionCodes.PageUnableToUpdate);

        // Invalidate cache for the site after updating a page
        internalService.Invalidate(page.SiteId);

        var pageResult = ObjectMapper.Map<PageEntity, Page>(updatedPage);

        await messagePublisher.Publish(new Message<Page>(ActionNames.PageUpdated, pageResult), cancellationToken);

        return await internalService.GetById(page.SiteId, page.Id, cancellationToken) ??
            throw new AppException(ExceptionCodes.PageUnableToUpdate);
    }

    public async Task<Page> Delete(Guid id, CancellationToken cancellationToken = default)
    {
        var originalPage = await pageRepository.FindAsync(id, cancellationToken: cancellationToken) ??
            throw new AppException(ExceptionCodes.PageNotFound);

        //if (!await permissionManager.HasAccess(originalPage.SiteId, originalPage.Id, PagePermissionAction.PageAdmin, cancellationToken))
        //    throw new AppException(ExceptionCodes.PermissionDenied);

        var pageModel = await internalService.GetById(originalPage.SiteId, id, cancellationToken) ??
            throw new AppException(ExceptionCodes.PageNotFound);

        // check that it does not have any children
        if (pageModel.Children.Count != 0)
            throw new AppException(ExceptionCodes.PageHasChildren);

        // check that if page is home page
        if (pageModel.FullPath == "/")
            throw new AppException(ExceptionCodes.PageCannotDeleteHome);

        var deletedPage = await pageRepository.DeleteWithResultAsync(id, cancellationToken: cancellationToken) ??
             throw new AppException(ExceptionCodes.PageUnableToDelete);

        // Invalidate cache for the site after deleting a page
        internalService.Invalidate(originalPage.SiteId);
        var pageResult = ObjectMapper.Map<PageEntity, Page>(deletedPage);

        await messagePublisher.Publish(new Message<Page>(ActionNames.PageDeleted, pageResult), cancellationToken);

        return pageResult;
    }

    public async Task<Page> GetById(Guid id, CancellationToken cancellationToken = default)
    {
        //fetch page from db
        var page = await pageRepository.FindAsync(id, cancellationToken: cancellationToken) ??
            throw new AppException(ExceptionCodes.PageNotFound);

        //if (!await permissionManager.HasAccess(page, PermissionActionNames.PageView, cancellationToken))
        //    throw new AppException(ExceptionCodes.PermissionDenied);

        return ObjectMapper.Map<PageEntity, Page>(page);
    }

    public async Task<IEnumerable<PageModel>> GetBySiteId(Guid siteId, CancellationToken cancellationToken = default)
    {
        var pagesDict = await internalService.GetAllBySiteId(siteId, cancellationToken);
        return pagesDict.Values.ToList();
    }

    public async Task<PageModel> GetByFullPath(Guid siteId, string fullPath, CancellationToken cancellationToken = default)
    {
        var page = await internalService.GetByFullPath(siteId, fullPath, cancellationToken) ??
            throw new AppException(ExceptionCodes.PageNotFound);

        return page;
    }

    #region Private Methods

    // Private helper method to normalize the path
    private static string NormalizeFullPath(string path)
    {
        // Normalize the page path
        path = path.ToLowerInvariant().Trim();

        // If the path has at the end forward slash (/), remove it
        path = path.TrimEnd('/');

        // check if path does not start with forward slash (/), add it
        if (!path.StartsWith("/"))
            path = "/" + path;

        return path;
    }

    // Helper to validate site and permission
    private static void ValidateAndNormalize(Page page)
    {
        page.Path = NormalizeFullPath(page.Path);

        var _reservedNames = new List<string> {
            "/setup",
            "/api",
            "/files"
        };

        // Check if path is valid 
        // Only one segment without forward slash (/) is allowed
        // that should starts with forward slash (/)
        // Alphanumeric characters (only lowercase) : a-z, 0-9
        // Special characters: -, _, ., ~
        if (!Regex.IsMatch(page.Path, @"^\/[a-z0-9-_~]+$") && page.Path != "/")
            throw new AppException(ExceptionCodes.PagePathInvalidCharacter);

        if (page.ParentId is null && _reservedNames.Contains(page.Path))
            throw new AppException(ExceptionCodes.PagePathReservedName);
    }

    private async Task ValidateParentPage(Page page, CancellationToken cancellationToken = default)
    {
        // If Parent Id is assigned
        if (page.ParentId != null)
        {
            // Fetch pages beforehand to avoid multiple db calls
            var parentPage = await internalService.GetById(page.SiteId, page.ParentId.Value, cancellationToken) ??
                throw new AppException(ExceptionCodes.PageParentPageNotFound);

            // If parent id is not on the same site
            if (parentPage.SiteId != page.SiteId)
                throw new AppException(ExceptionCodes.PageParentMustBeOnTheSameSite);

            // If parent page's Path is /
            if (parentPage.Path == "/")
                throw new AppException(ExceptionCodes.PageParentCannotBeHome);

            if (page.Path == "/")
                throw new AppException(ExceptionCodes.PageHomeCannotHaveParent);
        }
    }

    private async Task ValidateDuplicatePath(Page page, CancellationToken cancellationToken = default)
    {
        if (page.ParentId.HasValue)
        {
            // Check if the page with the same path already exists
            var parentPage = await internalService.GetById(page.SiteId, page.ParentId.Value, cancellationToken) ??
                throw new AppException(ExceptionCodes.PageParentPageNotFound);

            if (parentPage.Children.Count != 0)
            {
                // check if any child has the same path throw exception
                if (parentPage.Children.Any(x => x.Path == page.Path))
                    throw new AppException(ExceptionCodes.PagePathMustBeUnique);
            }
        }
        else
        {
            var rootPages = await internalService.GetHierarchyBySiteId(page.SiteId, cancellationToken);
            // check if any root page has the same path throw exception
            if (rootPages.Any(x => x.Path == page.Path))
                throw new AppException(ExceptionCodes.PagePathMustBeUnique);
        }
    }

    #endregion
}

public class PageInternalService(IPageModelRepository pageRepository, ICacheServices cacheProvider) : FluentCMSServiceAppService, IPageInternalService
{
    public async Task<Dictionary<Guid, PageModel>> GetAllBySiteId(Guid siteId, CancellationToken cancellationToken = default)
    {
        var cacheKey = GetAllPagesDictionaryCacheKey(siteId);

        // Try to get from cache first
        if (cacheProvider.TryGetValue(cacheKey, out Dictionary<Guid, PageModel>? cachedPages))
        {
            return cachedPages!;
        }

        // If not cached, get from repository
        var allPages = await pageRepository.GetAllForSite(siteId, cancellationToken);

        // Build dictionary model
        var pageModels = BuildPagesDictionary(allPages);

        // Cache the result
        cacheProvider.Set(cacheKey, pageModels);

        return pageModels;
    }

    public async Task<PageModel?> GetByFullPath(Guid siteId, string fullPath, CancellationToken cancellationToken = default)
    {
        var cacheKey = GetFullPathCacheKey(siteId);

        var pagesDictionary = await GetAllBySiteId(siteId, cancellationToken);

        // Try to get from cache first
        if (!cacheProvider.TryGetValue(cacheKey, out Dictionary<string, Guid>? cachedFullPath))
        {
            cachedFullPath = pagesDictionary.ToDictionary(x => x.Value.FullPath, x => x.Value.Id);

            // Cache the result
            cacheProvider.Set(cacheKey, cachedFullPath);
        }

        if (cachedFullPath!.TryGetValue(fullPath, out Guid pageId))
            return pagesDictionary[pageId];
        else
            return null;
    }

    public async Task<IEnumerable<PageModel>> GetHierarchyBySiteId(Guid siteId, CancellationToken cancellationToken = default)
    {
        var cacheKey = GetAllPagesCacheKey(siteId);

        // Try to get from cache first
        if (cacheProvider.TryGetValue(cacheKey, out IEnumerable<PageModel>? cachedPages))
        {
            return cachedPages!;
        }

        // If not cached, get from internal cached dictionary
        var pageDictionary = await GetAllBySiteId(siteId, cancellationToken);

        // only return the root pages
        var pageModels = pageDictionary.Values.OrderBy(x => x.Order).Where(p => !p.ParentId.HasValue);

        // Cache the result
        cacheProvider.Set(cacheKey, pageModels);

        return pageModels;

    }

    public async Task<PageModel?> GetById(Guid siteId, Guid pageId, CancellationToken cancellationToken = default)
    {
        var pages = await GetAllBySiteId(siteId, cancellationToken);

        if (pages.TryGetValue(pageId, out PageModel? page))
            return page;
        else
            return null;
    }

    public void Invalidate(Guid siteId)
    {
        cacheProvider.Remove(GetAllPagesCacheKey(siteId));
        cacheProvider.Remove(GetAllPagesDictionaryCacheKey(siteId));
        cacheProvider.Remove(GetFullPathCacheKey(siteId));
    }

    #region Private

    private static string GetAllPagesCacheKey(Guid siteId) => $"site_all_pages_{siteId}";
    private static string GetAllPagesDictionaryCacheKey(Guid siteId) => $"site_all_pages_dictionary_{siteId}";
    private static string GetFullPathCacheKey(Guid siteId) => $"site_full-path-guid_dictionary_{siteId}";

    // Helper method to build hierarchical page model
    private Dictionary<Guid, PageModel> BuildPagesDictionary(IEnumerable<PageEntity> pages)
    {
        // Similar to the original implementation to construct the page hierarchy
        var pageDictionary = pages.ToDictionary(p => p.Id, p =>
        {
            var model = ObjectMapper.Map<PageEntity, PageModel>(p);
            model.FullPath = p.Path;
            return model;
        });

        foreach (var page in pages)
        {
            if (page.ParentId.HasValue)
            {
                var parentPage = pageDictionary[page.ParentId.Value];
                var childPage = pageDictionary[page.Id];

                childPage.FullPath = $"{parentPage.FullPath}{childPage.FullPath}";
                parentPage.Children.Add(ObjectMapper.Map<PageModel, PageDetailResponse>(childPage));
            }
        }

        // apply order to children
        foreach (var page in pageDictionary.Values)
        {
            page.Children = [.. page.Children.OrderBy(x => x.Order)];
        }

        return pageDictionary;
    }

    #endregion
}
