using ByteLabs.Foundations.DependencyInjection;
using FluentCMS.Domain.IO;
using FluentCMS.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCMS.Services.IO;

public partial class FolderService(IFolderModelRepository folderRepository, IFileModelRepository fileRepository) : FluentCMSServiceAppService, IFolderService, ITransientDependency
{
    public async Task<Folder> CreateRoot(Guid siteId, CancellationToken cancellationToken = default)
    {
        var normalizedName = GetNormalizedFolderName(ServiceConstants.ROOT_FOLDER_PATH);

        if (!IsValidFolderName(normalizedName))
            throw new AppException(ExceptionCodes.FolderInvalidName);

        if (await folderRepository.GetByName(siteId, null, normalizedName, cancellationToken) != null)
            throw new AppException(ExceptionCodes.FolderAlreadyExists);

        var rootFolder = new Folder
        {
            Name = ServiceConstants.ROOT_FOLDER_PATH,
            NormalizedName = normalizedName,
            SiteId = siteId
        };

        var folderEntity = ObjectMapper.Map<Folder, FolderModel>(rootFolder);

        var results = await folderRepository.InsertAsync(folderEntity, cancellationToken: cancellationToken) ??
            throw new AppException(ExceptionCodes.FolderUnableToCreate);
        return ObjectMapper.Map<FolderModel, Folder>(results);
    }

    public async Task<Folder> Create(Folder folder, CancellationToken cancellationToken = default)
    {
        folder.NormalizedName = GetNormalizedFolderName(folder.Name);

        if (!IsValidFolderName(folder.NormalizedName))
            throw new AppException(ExceptionCodes.FolderInvalidName);

        // check if parent folder exists
        if (folder.ParentId == null)
            throw new AppException(ExceptionCodes.FolderParentNotFound);

        _ = await folderRepository.FindAsync(folder.ParentId.Value, cancellationToken: cancellationToken) ??
                throw new AppException(ExceptionCodes.FolderParentNotFound);

        // check if folder with the same name already exists
        var existingFolder = await folderRepository.GetByName(folder.SiteId, folder.ParentId, folder.NormalizedName, cancellationToken);
        if (existingFolder != null)
            throw new AppException(ExceptionCodes.FolderAlreadyExists);

        var folderEntity = ObjectMapper.Map<Folder, FolderModel>(folder);

        var results = await folderRepository.InsertAsync(folderEntity, cancellationToken: cancellationToken) ??
            throw new AppException(ExceptionCodes.FolderUnableToCreate);
        return ObjectMapper.Map<FolderModel, Folder>(results);
    }

    public async Task<IEnumerable<Folder>> GetAll(Guid siteId, CancellationToken cancellationToken = default)
    {
        var results = await folderRepository.GetAllForSite(siteId, cancellationToken);
        return ObjectMapper.Map<IEnumerable<FolderModel>, List<Folder>>(results);
    }

    public async Task<Folder> GetById(Guid id, CancellationToken cancellationToken = default)
    {
        var folder = await folderRepository.FindAsync(id, cancellationToken: cancellationToken) ??
            throw new AppException(ExceptionCodes.FolderNotFound);

        return ObjectMapper.Map<FolderModel, Folder>(folder);
    }

    public async Task<List<Folder>> GetParentFolders(Guid folderId, CancellationToken cancellationToken = default)
    {
        var folders = new List<FolderModel>();
        var currentFolder = await folderRepository.FindAsync(folderId, cancellationToken: cancellationToken) ??
            throw new AppException(ExceptionCodes.FolderNotFound);

        folders.Add(currentFolder);

        while (currentFolder != null && currentFolder.ParentId != null)
        {
            currentFolder = await folderRepository.FindAsync(currentFolder.ParentId.Value, cancellationToken: cancellationToken) ??
                throw new AppException(ExceptionCodes.FolderNotFound);
            if (currentFolder != null)
                folders.Add(currentFolder);
        }

        folders.Reverse();

        return ObjectMapper.Map<List<FolderModel>, List<Folder>>(folders);
    }

    public async Task<Folder> Delete(Guid id, CancellationToken cancellationToken = default)
    {
        var existingFolder = await folderRepository.FindAsync(id, cancellationToken: cancellationToken) ??
            throw new AppException(ExceptionCodes.FolderNotFound);

        var allFolders = await GetAll(existingFolder.SiteId, cancellationToken);
        var allFiles = await fileRepository.GetAllForSite(existingFolder.SiteId, cancellationToken);

        return await DeleteSubFolders(id, allFolders, allFiles, cancellationToken);
    }

    public Task<Folder> GetByPath(Guid siteId, string folderPath, CancellationToken cancellationToken = default)
    {
        folderPath = GetNormalizedFolderName(folderPath);

        // split folder path
        string[] folderNames = folderPath.Split('/', StringSplitOptions.RemoveEmptyEntries);

        return FindFolder(siteId, null, folderNames, 0, cancellationToken);
    }

    // find folder recursively by parent folder
    private async Task<Folder> FindFolder(Guid siteId, Guid? parentId, string[] folderNames, int index, CancellationToken cancellationToken)
    {
        if (index == folderNames.Length - 1)
        {
            var byName = await folderRepository.GetByName(siteId, parentId, folderNames[index], cancellationToken) ??
                throw new AppException(ExceptionCodes.FolderNotFound);
            return ObjectMapper.Map<FolderModel, Folder>(byName);
        }
        var folder = await folderRepository.GetByName(siteId, parentId, folderNames[index], cancellationToken) ??
            throw new AppException(ExceptionCodes.FolderNotFound);

        return await FindFolder(siteId, folder.Id, folderNames, index + 1, cancellationToken);
    }

    public async Task<Folder> Rename(Guid folderId, string name, CancellationToken cancellationToken = default)
    {
        var normalizedName = GetNormalizedFolderName(name);

        if (!IsValidFolderName(normalizedName))
            throw new AppException(ExceptionCodes.FolderInvalidName);

        var existingFolder = await folderRepository.FindAsync(folderId, cancellationToken: cancellationToken) ??
            throw new AppException(ExceptionCodes.FolderNotFound);

        // check if folder with the same name already exists
        var existingFolderSameName = await folderRepository.GetByName(existingFolder.SiteId, existingFolder.ParentId, normalizedName, cancellationToken);

        // check if the folder is not root folder
        if (existingFolder.ParentId == null)
            throw new AppException(ExceptionCodes.FolderCannotRenameRootFolder);

        if (existingFolderSameName != null && existingFolderSameName.Id != folderId)
            throw new AppException(ExceptionCodes.FolderAlreadyExists);

        existingFolder.Name = name;
        existingFolder.NormalizedName = normalizedName;

        var results = await folderRepository.UpdateAsync(existingFolder, cancellationToken: cancellationToken) ??
            throw new AppException(ExceptionCodes.FolderUnableToUpdate);
        return ObjectMapper.Map<FolderModel, Folder>(results);
    }

    public async Task<Folder> Move(Guid folderId, Guid parentFolderId, CancellationToken cancellationToken = default)
    {
        // check if folder is not moving to itself
        if (folderId == parentFolderId)
            throw new AppException(ExceptionCodes.FolderCannotMoveToItself);

        // check if folder exists
        var existingFolder = await folderRepository.FindAsync(folderId, cancellationToken: cancellationToken) ??
            throw new AppException(ExceptionCodes.FolderNotFound);

        // check if the folder is not root folder
        if (existingFolder.ParentId == null)
            throw new AppException(ExceptionCodes.FolderCannotMoveRootFolder);

        // check if parent folder exists
        var parentFolder = await folderRepository.FindAsync(parentFolderId, cancellationToken: cancellationToken) ??
                throw new AppException(ExceptionCodes.FolderParentNotFound);

        // check if folder is not moving to its child
        var parentFolders = await GetParentFolders(parentFolderId, cancellationToken);
        if (parentFolders.Any(x => x.Id == folderId))
            throw new AppException(ExceptionCodes.FolderCannotMoveToChild);

        // check parent folder and folder is in the same site
        if (parentFolder.SiteId != existingFolder.SiteId)
            throw new AppException(ExceptionCodes.FolderNotFound);

        existingFolder.ParentId = parentFolderId;

        var results = await folderRepository.UpdateAsync(existingFolder, cancellationToken: cancellationToken) ??
            throw new AppException(ExceptionCodes.FolderUnableToUpdate);
        return ObjectMapper.Map<FolderModel, Folder>(results);
    }

    private static string GetNormalizedFolderName(string folderName)
    {
        return folderName.Trim().ToLower();
    }

    // Regular expression to allow only alphanumeric, spaces, underscores, and certain special characters
    private static readonly Regex _regex = new(@"^[\w\-. ]+$");

    private static bool IsValidFolderName(string folderName)
    {
        if (string.IsNullOrWhiteSpace(folderName))
            return false; // Folder name should not be empty or whitespace

        return _regex.IsMatch(folderName);
    }

    private async Task<Folder> DeleteSubFolders(Guid parentFolderId, IEnumerable<Folder> allFolders, IEnumerable<FileModel> allFiles, CancellationToken cancellationToken = default)
    {
        var results = await DeleteSubFolders(parentFolderId, ObjectMapper.Map<IEnumerable<Folder>, List<FolderModel>>(allFolders), allFiles, cancellationToken);
        return ObjectMapper.Map<FolderModel, Folder>(results);
    }

    private async Task<FolderModel> DeleteSubFolders(Guid parentFolderId, IEnumerable<FolderModel> allFolders, IEnumerable<FileModel> allFiles, CancellationToken cancellationToken = default)
    {
        var fileIds = allFiles.Where(x => x.FolderId == parentFolderId).Select(x => x.Id);
        await fileRepository.DeleteManyAsync(fileIds, cancellationToken: cancellationToken);

        foreach (var childFolder in allFolders.Where(x => x.ParentId == parentFolderId))
            await DeleteSubFolders(childFolder.Id, allFolders, allFiles, cancellationToken);

        return await folderRepository.DeleteWithResultAsync(parentFolderId, cancellationToken: cancellationToken) ??
            throw new AppException(ExceptionCodes.FolderUnableToDelete);
    }

}
