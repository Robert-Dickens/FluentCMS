using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCMS.IO;

public interface IFileAppService : ICrudAppService<File, Guid, GetFileRequest, FileCreateRequest, FileUpdateRequest>
{
}

public interface IFileService : IApplicationService
{
    Task<File> Create(File file, System.IO.Stream fileContent, CancellationToken cancellationToken = default);
    Task<File> GetById(Guid id, CancellationToken cancellationToken = default);
    Task<File> GetByName(Guid folderId, string fileName, CancellationToken cancellationToken = default);
    Task<File> Rename(Guid id, string name, CancellationToken cancellationToken = default);
    Task<File> Delete(Guid id, CancellationToken cancellationToken = default);
    Task<File> Move(Guid id, Guid folderId, CancellationToken cancellationToken = default);
    Task<string> GetFilePath(File file, CancellationToken cancellationToken = default);
    Task<System.IO.Stream> GetStream(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<File>> GetAll(Guid siteId, CancellationToken cancellationToken = default);
}

public class File : ExtensibleFullAuditedEntityDto<Guid>
{
    public Guid SiteId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string NormalizedName { get; set; } = string.Empty;
    public Guid FolderId { get; set; }
    public string Extension { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public long Size { get; set; }
    public string Path { get; set; } = default!;
}

public class FileDetailResponse : File
{

}

public class GetFileRequest : FilteredAndPagedAndSortedResultRequestDto
{
}

public class FileCreateRequest : ExtensibleEntityDto
{
    public Guid SiteId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string NormalizedName { get; set; } = string.Empty;
    public Guid FolderId { get; set; }
    public string Extension { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public long Size { get; set; }
}

public class FileUpdateRequest : ExtensibleEntityDto
{
    public Guid SiteId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string NormalizedName { get; set; } = string.Empty;
    public Guid FolderId { get; set; }
    public string Extension { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public long Size { get; set; }
}

public class FileRenameRequest
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
}

public class FileMoveRequest
{
    public Guid Id { get; set; }
    public Guid FolderId { get; set; }
}
