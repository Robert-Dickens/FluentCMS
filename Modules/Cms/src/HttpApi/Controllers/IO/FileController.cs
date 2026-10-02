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
[Route($"api/{FluentCMSRemoteServiceConsts.RoutePath}/io/files")]
[Authorize(FluentCMSPermissions.IO.Files.Default)]
public class FileController : FluentCMSBaseController, IFileAppService
{
    private readonly IFileAppService _fileAppService;

    public FileController(IFileAppService fileAppService) => _fileAppService = fileAppService;

    [HttpPost]
    [Authorize(FluentCMSPermissions.IO.Files.Create)]
    public Task<File> CreateAsync(FileCreateRequest input) => _fileAppService.CreateAsync(input);

    [HttpDelete("{id}")]
    [Authorize(FluentCMSPermissions.IO.Files.Delete)]
    public Task DeleteAsync(Guid id) => _fileAppService.DeleteAsync(id);

    [HttpGet("{id}")]
    public Task<File> GetAsync(Guid id) => _fileAppService.GetAsync(id);

    [HttpGet]
    public Task<PagedResultDto<File>> GetListAsync(GetFileRequest input) => _fileAppService.GetListAsync(input);

    [HttpPut("{id}")]
    [Authorize(FluentCMSPermissions.IO.Files.Edit)]
    public Task<File> UpdateAsync(Guid id, FileUpdateRequest input) => _fileAppService.UpdateAsync(id, input);
}
