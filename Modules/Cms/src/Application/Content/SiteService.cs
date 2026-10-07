using ByteLabs.Foundations.DependencyInjection;
using FluentCMS.Content;
using FluentCMS.Domain.Content;
using FluentCMS.Domain.Plugins;
using FluentCMS.Permissions;
using FluentCMS.Providers.MessageBusProviders;
using FluentCMS.Services.Permissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCMS.Services.Content;

public class SiteService(ISiteModelRepository siteRepository, IPluginDefinitionModelRepository pluginDefinitionRepository, FluentCMS.Settings.ISettingsService settingsService, IPageService pageService, IMessagePublisher messagePublisher, IPermissionManager permissionManager) : FluentCMSServiceAppService, ISiteService, ITransientDependency
{
    public async Task<IEnumerable<Site>> GetAll(CancellationToken cancellationToken = default)
    {
        var sites = await siteRepository.GetListAsync(cancellationToken: cancellationToken);
        return await permissionManager.GetAccessible(sites, SitePermissionAction.SiteAdmin, cancellationToken);
    }

    public async Task<Site> GetById(Guid id, CancellationToken cancellationToken = default)
    {
        if (!await permissionManager.HasAccess(id, SitePermissionAction.SiteAdmin, cancellationToken))
            throw new AppException(ExceptionCodes.PermissionDenied);

        var result = await siteRepository.FindAsync(id, cancellationToken: cancellationToken) ??
            throw new AppException(ExceptionCodes.SiteNotFound);

        return ObjectMapper.Map<SiteModel, Site>(result);
    }

    public async Task<Site> GetByUrl(string url, CancellationToken cancellationToken = default)
    {
        url = ValidateAndFormatUrl(url);
        // no need to check permissions
        var result = await siteRepository.GetByUrl(url, cancellationToken) ??
            throw new AppException(ExceptionCodes.SiteNotFound);

        return ObjectMapper.Map<SiteModel, Site>(result);
    }

    public async Task<Site> Create(SiteTemplate siteTemplate, CancellationToken cancellationToken = default)
    {
        if (!await permissionManager.HasAccess(GlobalPermissionAction.SuperAdmin, cancellationToken))
            throw new AppException(ExceptionCodes.PermissionDenied);

        var layouts = siteTemplate.Layouts;

        var siteFilePath = System.IO.Path.Combine(ServiceConstants.SetupTemplatesFolder, siteTemplate.Template, ServiceConstants.SetupSiteTemplateFile);

        if (!System.IO.File.Exists(siteFilePath))
            throw new AppException($"{ServiceConstants.SetupSiteTemplateFile} doesn't exist!");

        var jsonSerializerOptions = new JsonSerializerOptions();
        jsonSerializerOptions.Converters.Add(new DictionaryJsonConverter());

        var jsonSiteTemplate = await JsonSerializer.DeserializeAsync<SiteTemplate>(System.IO.File.OpenRead(siteFilePath), jsonSerializerOptions, cancellationToken) ??
               throw new AppException($"Failed to read/deserialize {ServiceConstants.SetupSiteTemplateFile}");

        jsonSiteTemplate.Url = siteTemplate.Url;
        jsonSiteTemplate.Template = siteTemplate.Template;

        if (!string.IsNullOrEmpty(siteTemplate.Name))
            jsonSiteTemplate.Name = siteTemplate.Name;

        if (!string.IsNullOrEmpty(siteTemplate.Description))
            jsonSiteTemplate.Description = siteTemplate.Description;

        jsonSiteTemplate.Merge(siteTemplate);

        // loading layout data from files
        foreach (var layout in siteTemplate.Layouts)
        {
            var bodyLayoutFilePath = System.IO.Path.Combine(ServiceConstants.SetupTemplatesFolder, siteTemplate.Template, ServiceConstants.SetupLayoutsFolder, $"{layout.Name}.body.html");
            var headLayoutFilePath = System.IO.Path.Combine(ServiceConstants.SetupTemplatesFolder, siteTemplate.Template, ServiceConstants.SetupLayoutsFolder, $"{layout.Name}.head.html");
            layout.Body = await System.IO.File.ReadAllTextAsync(bodyLayoutFilePath, cancellationToken);
            layout.Head = await System.IO.File.ReadAllTextAsync(headLayoutFilePath, cancellationToken);
        }

        foreach (var block in siteTemplate.Blocks)
        {
            var blockContentFilePath = System.IO.Path.Combine(ServiceConstants.SetupTemplatesFolder, siteTemplate.Template, ServiceConstants.SetupBlocksFolder, block.Category, $"{block.Name}.html");
            block.Content = await System.IO.File.ReadAllTextAsync(blockContentFilePath, cancellationToken);
        }

        var pluginDefinitions = await pluginDefinitionRepository.GetListAsync(cancellationToken: cancellationToken);
        SetIds(siteTemplate, pluginDefinitions);

        var site = ObjectMapper.Map<SiteTemplate, Site>(siteTemplate);

        site.Urls = [ValidateAndFormatUrl(siteTemplate.Url)];
        site.LayoutId = layouts.Where(x => x.Name == siteTemplate.Layout).Single().Id;
        site.EditLayoutId = layouts.Where(x => x.Name == siteTemplate.EditLayout).Single().Id;
        site.DetailLayoutId = layouts.Where(x => x.Name == siteTemplate.DetailLayout).Single().Id;

        ValidateAndFormatUrls(site);

        // check if site url are unique
        if (!await siteRepository.CheckUrlsForAvailability(site.Urls, cancellationToken))
        {
            throw new AppException(ExceptionCodes.SiteUrlMustBeUnique);
        }

        var siteEntity = ObjectMapper.Map<Site, SiteModel>(site);

        var newSite = await siteRepository.InsertAsync(siteEntity, cancellationToken: cancellationToken) ??
            throw new AppException(ExceptionCodes.SiteUnableToCreate);

        await messagePublisher.Publish(new Message<SiteTemplate>(ActionNames.SiteCreated, siteTemplate), cancellationToken);
        await messagePublisher.Publish(new Message<SiteTemplate>(ActionNames.SetupInitializePlugins, siteTemplate), cancellationToken);

        return ObjectMapper.Map<SiteModel, Site>(newSite);
    }

    public async Task<Site> Update(Site site, CancellationToken cancellationToken = default)
    {
        if (!await permissionManager.HasAccess(site.Id, SitePermissionAction.SiteAdmin, cancellationToken))
            throw new AppException(ExceptionCodes.PermissionDenied);

        ValidateAndFormatUrls(site);

        // check if site url is unique
        if (!await siteRepository.CheckUrlsForAvailability(site.Urls, cancellationToken))
        {
            throw new AppException(ExceptionCodes.SiteUrlMustBeUnique);
        }

        var siteEntity = ObjectMapper.Map<Site, SiteModel>(site);

        var updateSite = await siteRepository.UpdateAsync(siteEntity, cancellationToken: cancellationToken) ??
            throw new AppException(ExceptionCodes.SiteUnableToUpdate);

        var siteUpdated = ObjectMapper.Map<SiteModel, Site>(updateSite);

        await messagePublisher.Publish(new Message<Site>(ActionNames.SiteUpdated, siteUpdated), cancellationToken);

        return siteUpdated;
    }

    public async Task<Site> Delete(Guid id, CancellationToken cancellationToken = default)
    {
        if (!await permissionManager.HasAccess(GlobalPermissionAction.SuperAdmin, cancellationToken))
            throw new AppException(ExceptionCodes.PermissionDenied);

        var deletedSite = await siteRepository.DeleteWithResultAsync(id, cancellationToken: cancellationToken) ??
            throw new AppException(ExceptionCodes.SiteUnableToDelete);

        var results = ObjectMapper.Map<SiteModel, Site>(deletedSite);
        await messagePublisher.Publish(new Message<Site>(ActionNames.SiteDeleted, results), cancellationToken);

        return results;
    }

    public async Task<string> GetSitemap(Site site, Uri uri, CancellationToken cancellationToken = default)
    {
        var pages = await pageService.GetBySiteId(site.Id, cancellationToken);
        var settings = await settingsService.GetByIds(pages.Select(x => x.Id), cancellationToken);

        var result = "";

        result += "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n";
        result += "<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\">\n";

        foreach (var page in pages)
        {
            var pageSettings = settings.FirstOrDefault(x => x.Id == page.Id);
            if (pageSettings is null)
                continue;

            pageSettings.Values.TryGetValue("Index", out var index);

            if (index == "true")
            {
                result += "    <url>\n";
                result += $"        <loc>{uri.Scheme}://{uri.Host}{page.FullPath}</loc>\n";
                if (page.LastModificationTime.HasValue)
                    result += $"        <lastmod>{page.LastModificationTime.Value.ToString("yyyy-MM-dd")}</lastmod>\n";
                else
                    result += $"        <lastmod>{page.CreationTime.ToString("yyyy-MM-dd")}</lastmod>\n";
                result += "    </url>\n";
            }
        }

        result += "</urlset>";
        return result;
    }

    private static void ValidateAndFormatUrls(Site site)
    {
        site.Urls = site.Urls.Select(ValidateAndFormatUrl).ToList();
    }

    public const string VALID_DOMAIN_NAME_REGEX = @"^(([a-zA-Z0-9]|[a-zA-Z0-9][a-zA-Z0-9\-]*[a-zA-Z0-9])\.)*([A-Za-z0-9]|[A-Za-z0-9][A-Za-z0-9\-]*[A-Za-z0-9])(:\d{1,5})?$";

    private static string ValidateAndFormatUrl(string url)
    {
        // Trim spaces
        url = url.Trim();

        // Check if URL is empty
        if (string.IsNullOrEmpty(url))
            throw new AppException(ExceptionCodes.SiteUrlIsEmpty);

        // Convert to lowercase
        url = url.ToLowerInvariant();

        // if the url ends with a slash, remove it
        if (url.EndsWith('/'))
            url = url[..^1];

        // Validate URL format using regex
        if (!Regex.IsMatch(url, VALID_DOMAIN_NAME_REGEX))
            throw new AppException(ExceptionCodes.SiteUrlIsInvalid);

        // Return the formatted URL if valid
        return url;
    }

    private static void SetIds(SiteTemplate siteTemplate, IEnumerable<PluginDefinitionModel> pluginDefinitions)
    {
        // set site, page, layout, etc. ids
        siteTemplate.Id = Guid.NewGuid();

        foreach (var layout in siteTemplate.Layouts)
        {
            layout.Id = Guid.NewGuid();
            layout.SiteId = siteTemplate.Id;
        }

        foreach (var role in siteTemplate.Roles)
        {
            role.Id = Guid.NewGuid();
            role.SiteId = siteTemplate.Id;
        }

        foreach (var page in siteTemplate.Pages)
        {
            page.Template = siteTemplate.Template;
            SetIds(siteTemplate, page, pluginDefinitions);
        }
    }

    private static void SetIds(SiteTemplate siteTemplate, PageTemplate page, IEnumerable<PluginDefinitionModel> pluginDefinitions)
    {
        page.Id = Guid.NewGuid();
        page.SiteId = siteTemplate.Id;
        foreach (var plugin in page.Plugins)
        {
            plugin.Id = Guid.NewGuid();
            plugin.DefinitionId = pluginDefinitions.Where(p => p.Name.Equals(plugin.Definition, StringComparison.InvariantCultureIgnoreCase)).Single().Id;
        }
        foreach (var childPage in page.Children)
        {
            childPage.Template = siteTemplate.Template;
            SetIds(siteTemplate, childPage, pluginDefinitions);
        }
    }

}
