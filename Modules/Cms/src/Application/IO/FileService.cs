using ByteLabs.Foundations.DependencyInjection;
using FluentCMS.Domain.IO;
using FluentCMS.IO;
using FluentCMS.Providers.FileStorageProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCMS.Services.IO;

public class FileService(IFileModelRepository fileRepository, IFolderModelRepository folderRepository, IFolderService folderService, IFileStorageProvider fileStorageProvider) : FluentCMSServiceAppService, IFileService, ITransientDependency
{
    public async Task<File> Create(File file, System.IO.Stream fileContent, CancellationToken cancellationToken = default)
    {
        var folder = await folderRepository.FindAsync(file.FolderId, cancellationToken: cancellationToken) ??
            throw new AppException(ExceptionCodes.FolderNotFound);

        if (folder.SiteId != file.SiteId)
            throw new AppException(ExceptionCodes.FolderNotFound);

        file.NormalizedName = GetNormalizedFileName(file.Name);

        // check if file with the same name already exists
        var existingFile = await fileRepository.GetByName(folder.SiteId, folder.Id, file.NormalizedName, cancellationToken);
        if (existingFile != null)
        {
            // add a suffix to the new file's name to avoid conflicts with preiously uploaded files
            var fileNameWithoutExtension = System.IO.Path.GetFileNameWithoutExtension(file.Name);
            var fileExtension = System.IO.Path.GetExtension(file.Name);
            var suffix = 1;
            do
            {
                file.Name = $"{fileNameWithoutExtension} ({suffix}){fileExtension}";
                file.NormalizedName = GetNormalizedFileName(file.Name);
                existingFile = await fileRepository.GetByName(folder.SiteId, folder.Id, file.NormalizedName, cancellationToken);
                suffix++;
            } while (existingFile != null);
        }

        var fileToAdd = ObjectMapper.Map<File, FileModel>(file);

        var fileCreated = await fileRepository.InsertAsync(fileToAdd, cancellationToken: cancellationToken);

        await fileStorageProvider.Upload(fileCreated.Id.ToString(), fileContent, cancellationToken);

        return ObjectMapper.Map<FileModel, File>(fileCreated);
    }

    public async Task<File> Delete(Guid id, CancellationToken cancellationToken = default)
    {
        var results = await fileRepository.DeleteWithResultAsync(id, cancellationToken: cancellationToken) ??
            throw new AppException(ExceptionCodes.FileUnableToDelete);
        return ObjectMapper.Map<FileModel, File>(results);
    }

    public async Task<IEnumerable<File>> GetAll(Guid siteId, CancellationToken cancellationToken = default)
    {
        var results = await fileRepository.GetAllForSite(siteId, cancellationToken);
        return ObjectMapper.Map<IEnumerable<FileModel>, List<File>>(results);
    }

    public async Task<File> GetById(Guid id, CancellationToken cancellationToken = default)
    {
        var file = await fileRepository.FindAsync(id, cancellationToken: cancellationToken) ??
            throw new AppException(ExceptionCodes.FileNotFound);

        return ObjectMapper.Map<FileModel, File>(file);
    }

    public async Task<File> GetByName(Guid folderId, string fileName, CancellationToken cancellationToken = default)
    {
        var folder = await folderRepository.FindAsync(folderId, cancellationToken: cancellationToken) ??
            throw new AppException(ExceptionCodes.FolderNotFound);

        var normalizedFileName = GetNormalizedFileName(fileName);

        var file = await fileRepository.GetByName(folder.SiteId, folder.Id, normalizedFileName, cancellationToken) ??
            throw new AppException(ExceptionCodes.FileNotFound);

        return ObjectMapper.Map<FileModel, File>(file);
    }

    public async Task<string> GetFilePath(File file, CancellationToken cancellationToken = default)
    {
        var folders = await folderService.GetParentFolders(file.FolderId, cancellationToken) ??
            throw new AppException(ExceptionCodes.FolderNotFound);

        return string.Join("/", folders.Select(x => x.Name)) + "/" + file.Name;
    }

    public async Task<System.IO.Stream> GetStream(Guid id, CancellationToken cancellationToken = default)
    {
        return await fileStorageProvider.Download(id.ToString(), cancellationToken) ??
            throw new AppException(ExceptionCodes.FileNotFound);
    }

    public async Task<File> Rename(Guid id, string name, CancellationToken cancellationToken = default)
    {
        var normalizedFileName = GetNormalizedFileName(name);

        var file = await fileRepository.FindAsync(id, cancellationToken: cancellationToken) ??
            throw new AppException(ExceptionCodes.FileNotFound);

        if (!IsValidFileName(normalizedFileName))
            throw new AppException(ExceptionCodes.FileInvalidName);

        // check if file with the same name already exists
        var existingFile = await fileRepository.GetByName(file.SiteId, file.FolderId, normalizedFileName, cancellationToken);
        if (existingFile != null)
            throw new AppException(ExceptionCodes.FileAlreadyExists);

        file.Name = name;
        file.NormalizedName = normalizedFileName;

        var updated = await fileRepository.UpdateAsync(file, cancellationToken: cancellationToken) ??
            throw new AppException(ExceptionCodes.FileUnableToUpdate);

        return ObjectMapper.Map<FileModel, File>(updated);
    }

    public async Task<File> Move(Guid id, Guid folderId, CancellationToken cancellationToken = default)
    {
        var folder = await folderRepository.FindAsync(folderId, cancellationToken: cancellationToken) ??
            throw new AppException(ExceptionCodes.FolderNotFound);

        var file = await fileRepository.FindAsync(id, cancellationToken: cancellationToken) ??
            throw new AppException(ExceptionCodes.FileNotFound);

        if (file.SiteId != folder.SiteId)
            throw new AppException(ExceptionCodes.FolderNotFound);

        // check if file with the same name already exists
        var exisitingFile = await fileRepository.GetByName(folder.SiteId, folder.Id, file.NormalizedName, cancellationToken);
        if (exisitingFile != null)
            throw new AppException(ExceptionCodes.FileAlreadyExists);

        file.FolderId = folderId;

        var updated = await fileRepository.UpdateAsync(file, cancellationToken: cancellationToken) ??
            throw new AppException(ExceptionCodes.FileUnableToUpdate);

        return ObjectMapper.Map<FileModel, File>(updated);
    }

    private static readonly System.Text.RegularExpressions.Regex _fileNameRegex = new(@"[a-zA-Z0-9_\-\.\(\)\s]+(\.[a-zA-Z0-9]{2,6})?");

    private static bool IsValidFileName(string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
            return false; // Folder name should not be empty or whitespace

        return _fileNameRegex.IsMatch(fileName);
    }

    private static string GetNormalizedFileName(string fileName)
    {
        var normalized = fileName.Trim().ToLower();
        return normalized;
    }
}
