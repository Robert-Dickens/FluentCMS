using FluentCMS.Domain.IO;
using FluentCMS.IO;
using FluentCMS.Permissions;
using Microsoft.AspNetCore.Authorization;
using System;

namespace FluentCMS.Services.IO;

[Authorize(FluentCMSPermissions.IO.Folders.Default)]
public class FolderAppService : FluentCMSCrudAppService<FolderModel, Folder, Guid, GetFolderRequest, FolderCreateRequest, FolderUpdateRequest>, IFolderAppService
{
    public FolderAppService(IFolderModelRepository repository) : base(repository)
    {
        CreatePolicyName = FluentCMSPermissions.IO.Folders.Create;
        UpdatePolicyName = FluentCMSPermissions.IO.Folders.Edit;
        DeletePolicyName = FluentCMSPermissions.IO.Folders.Delete;
    }
}
