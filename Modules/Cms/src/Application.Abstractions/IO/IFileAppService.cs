using System;

namespace FluentCMS.IO;

public interface IFileAppService : ICrudAppService<File, Guid, GetFileRequest, FileCreateRequest, FileUpdateRequest>
{
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
