using ByteLabs.Foundations.Application.Services;
using System;

namespace FluentCMS.IO;

public interface IFolderAppService : ICrudAppService<Folder, Guid, GetFolderRequest, FolderCreateRequest, FolderUpdateRequest>
{
}

public class Folder : ExtensibleFullAuditedEntityDto<Guid>
{
    public Guid SiteId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string NormalizedName { get; set; } = string.Empty;
    public Guid? ParentId { get; set; }
}

public class GetFolderRequest : FilteredAndPagedAndSortedResultRequestDto
{
}

public class FolderCreateRequest : ExtensibleEntityDto
{
    public Guid SiteId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string NormalizedName { get; set; } = string.Empty;
    public Guid? ParentId { get; set; }
}

public class FolderUpdateRequest : ExtensibleEntityDto
{
    public Guid SiteId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string NormalizedName { get; set; } = string.Empty;
    public Guid? ParentId { get; set; }
}
