using FluentCMS.Content;
using FluentCMS.Providers.MessageBusProviders;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.StaticFiles;
using FluentCMS.IO;
using ByteLabs.Foundations.ObjectMapping;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FluentCMS.Services.MessageHandlers;

public class FolderMessageHandler(IMessagePublisher messagePublisher, IObjectMapper<FluentCMSApplicationModule> mapper, IFolderService folderService, IFileService fileService) : IMessageHandler<SiteTemplate>
{
    public async Task Handle(Message<SiteTemplate> notification, CancellationToken cancellationToken)
    {
        switch (notification.Action)
        {
            case ActionNames.SiteCreated:
                await CreateFolders(notification.Payload, cancellationToken);
                await messagePublisher.Publish(new Message<SiteTemplate>(ActionNames.SiteFilesCreated, notification.Payload), cancellationToken);

                break;

            default:
                break;
        }
    }

    private async Task CreateFolders(SiteTemplate siteTemplate, CancellationToken cancellationToken)
    {
        var rootFolder = await folderService.CreateRoot(siteTemplate.Id, cancellationToken);
        siteTemplate.Folders.Add(mapper.Map<Folder, FolderTemplate>(rootFolder));

        // read from assets folder and create Folders object model for the site
        var assetsPath = System.IO.Path.Combine(ServiceConstants.SetupTemplatesFolder, siteTemplate.Template, ServiceConstants.SetupFilesFolder);

        await CreateChildFolders(siteTemplate, rootFolder, assetsPath, cancellationToken);
    }

    private async Task CreateChildFolders(SiteTemplate siteTemplate, Folder parentFolder, string path, CancellationToken cancellationToken)
    {
        if (System.IO.Directory.Exists(path))
        {
            foreach (var folderName in System.IO.Directory.GetDirectories(path))
            {
                var folder = new Folder
                {
                    Id = Guid.NewGuid(),
                    Name = new System.IO.DirectoryInfo(folderName).Name,
                    SiteId = siteTemplate.Id,
                    ParentId = parentFolder.Id
                };
                await folderService.Create(folder, cancellationToken);
                siteTemplate.Folders.Add(mapper.Map<Folder, FolderTemplate>(folder));

                await CreateChildFolders(siteTemplate, folder, folderName, cancellationToken);
            }

            foreach (var fileName in System.IO.Directory.GetFiles(path))
            {
                var file = new File
                {
                    Id = Guid.NewGuid(),
                    Name = new System.IO.FileInfo(fileName).Name,
                    SiteId = siteTemplate.Id,
                    FolderId = parentFolder.Id,
                    Size = new System.IO.FileInfo(fileName).Length,
                    ContentType = GetContentType(fileName),
                    Extension = new System.IO.FileInfo(fileName).Extension
                };
                using var contentStream = new System.IO.FileStream(fileName, System.IO.FileMode.Open);
                await fileService.Create(file, contentStream, cancellationToken);

                var fileTemplate = mapper.Map<File, FileTemplate>(file);
                fileTemplate.Path = GetFilePath(siteTemplate.Folders, parentFolder, file.Name);
                siteTemplate.Files.Add(fileTemplate);
            }
        }
    }

    private string GetFilePath(List<FolderTemplate> folders, Folder parentFolder, string fileName)
    {
        var folderHierarchy = new List<string>();
        var currentFolder = mapper.Map<Folder, FolderTemplate>(parentFolder);

        while (currentFolder != null)
        {
            folderHierarchy.Insert(0, currentFolder.Name); // Add folder name to the beginning
            currentFolder = folders.FirstOrDefault(f => f.Id == currentFolder.ParentId);
        }

        return "/" + string.Join("/", folderHierarchy) + "/" + fileName;
    }

    private static string GetContentType(string fileName)
    {
        var provider = new FileExtensionContentTypeProvider();
        if (!provider.TryGetContentType(fileName, out var contentType))
        {
            // If the file extension is not recognized, default to a generic content type
            contentType = "application/octet-stream";
        }
        return contentType;
    }

}
