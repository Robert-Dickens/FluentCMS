using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCMS.IO;

public interface IFolderAppService : ICrudAppService<Folder, Guid, GetFolderRequest, FolderCreateRequest, FolderUpdateRequest>
{
}

public interface IFolderService : IApplicationService
{
    Task<Folder> Create(Folder folder, CancellationToken cancellationToken = default);
    Task<Folder> CreateRoot(Guid siteId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Folder>> GetAll(Guid siteId, CancellationToken cancellationToken = default);
    Task<Folder> GetById(Guid id, CancellationToken cancellationToken = default);
    Task<Folder> GetByPath(Guid siteId, string folderPath, CancellationToken cancellationToken = default);
    Task<List<Folder>> GetParentFolders(Guid folderId, CancellationToken cancellationToken = default);
    Task<Folder> Rename(Guid folderId, string name, CancellationToken cancellationToken = default);
    Task<Folder> Move(Guid folderId, Guid parentFolderId, CancellationToken cancellationToken = default);
    Task<Folder> Delete(Guid id, CancellationToken cancellationToken = default);
}

public class Folder : ExtensibleFullAuditedEntityDto<Guid>
{
    public Guid SiteId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string NormalizedName { get; set; } = string.Empty;
    public Guid? ParentId { get; set; }

    public long Size { get; set; }
    public List<FileDetailResponse> Files { get; set; } = [];
    public List<Folder> Folders { get; set; } = [];

}

public class FolderDetailResponse:Folder
{

}

public class GetFolderRequest : FilteredAndPagedAndSortedResultRequestDto
{
}

public class FolderCreateRequest : ExtensibleEntityDto
{
    [Required]
    public Guid SiteId { get; set; } = default!;

    [Required]
    public string Name { get; set; } = default!;

    [Required]
    public Guid ParentId { get; set; }
}

public class FolderUpdateRequest : ExtensibleEntityDto
{
    public Guid SiteId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string NormalizedName { get; set; } = string.Empty;
    public Guid? ParentId { get; set; }
}

public class FolderRenameRequest
{
    [Required]
    public Guid Id { get; set; }

    [Required]
    public string Name { get; set; } = default!;
}

public class FolderMoveRequest
{
    [Required]
    public Guid Id { get; set; }

    [Required]
    public Guid ParentId { get; set; }
}
