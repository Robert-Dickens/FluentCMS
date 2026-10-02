using ByteLabs.Foundations;
using ByteLabs.Foundations.Application.Dtos;
using FluentCMS.IO;
using FluentCMS.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace FluentCMS.Api.Controllers.IO;

[Controller]
[RemoteService(Name = FluentCMSRemoteServiceConsts.RemoteServiceName)]
[Area(FluentCMSRemoteServiceConsts.ModuleAreaName)]
[Route($"api/{FluentCMSRemoteServiceConsts.RoutePath}/io/folders")]
[Authorize(FluentCMSPermissions.IO.Folders.Default)]
public class FolderController : FluentCMSBaseController, IFolderAppService
{
    private readonly IFolderAppService _folderAppService;

    public FolderController(IFolderAppService folderAppService) => _folderAppService = folderAppService;

    [HttpPost]
    [Authorize(FluentCMSPermissions.IO.Folders.Create)]
    public Task<Folder> CreateAsync(FolderCreateRequest input) => _folderAppService.CreateAsync(input);

    [HttpDelete("{id}")]
    [Authorize(FluentCMSPermissions.IO.Folders.Delete)]
    public Task DeleteAsync(Guid id) => _folderAppService.DeleteAsync(id);

    [HttpGet("{id}")]
    public Task<Folder> GetAsync(Guid id) => _folderAppService.GetAsync(id);

    [HttpGet]
    public Task<PagedResultDto<Folder>> GetListAsync(GetFolderRequest input) => _folderAppService.GetListAsync(input);

    [HttpPut("{id}")]
    [Authorize(FluentCMSPermissions.IO.Folders.Edit)]
    public Task<Folder> UpdateAsync(Guid id, FolderUpdateRequest input) => _folderAppService.UpdateAsync(id, input);
}
