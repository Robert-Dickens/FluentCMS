using FluentCMS.Domain.IO;
using FluentCMS.IO;
using FluentCMS.Permissions;
using Microsoft.AspNetCore.Authorization;
using System;

namespace FluentCMS.Services.IO;

[Authorize(FluentCMSPermissions.IO.Files.Default)]
public class FileAppService : FluentCMSCrudAppService<FileModel, File, Guid, GetFileRequest, FileCreateRequest, FileUpdateRequest>, IFileAppService
{
    public FileAppService(IFileModelRepository repository) : base(repository)
    {
        CreatePolicyName = FluentCMSPermissions.IO.Files.Create;
        UpdatePolicyName = FluentCMSPermissions.IO.Files.Edit;
        DeletePolicyName = FluentCMSPermissions.IO.Files.Delete;
    }
}
