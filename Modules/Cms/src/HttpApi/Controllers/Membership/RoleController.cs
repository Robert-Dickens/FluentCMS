using ByteLabs.Foundations;
using ByteLabs.Foundations.Application.Dtos;
using FluentCMS.Membership;
using FluentCMS.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace FluentCMS.Api.Controllers.Membership;

[Controller]
[RemoteService(Name = FluentCMSRemoteServiceConsts.RemoteServiceName)]
[Area(FluentCMSRemoteServiceConsts.ModuleAreaName)]
[Route($"api/{FluentCMSRemoteServiceConsts.RoutePath}/membership/roles")]
[Authorize(FluentCMSPermissions.MemberShips.Roles.Default)]
public class RoleController : FluentCMSBaseController, IRoleAppService
{
    private readonly IRoleAppService _roleAppService;

    public RoleController(IRoleAppService roleAppService) => _roleAppService = roleAppService;

    [HttpPost]
    [Authorize(FluentCMSPermissions.MemberShips.Roles.Create)]
    public Task<Role> CreateAsync(RoleCreateRequest input) => _roleAppService.CreateAsync(input);

    [HttpDelete("{id}")]
    [Authorize(FluentCMSPermissions.MemberShips.Roles.Delete)]
    public Task DeleteAsync(Guid id) => _roleAppService.DeleteAsync(id);

    [HttpGet("{id}")]
    public Task<Role> GetAsync(Guid id) => _roleAppService.GetAsync(id);

    [HttpGet]
    public Task<PagedResultDto<Role>> GetListAsync(GetRoleRequest input) => _roleAppService.GetListAsync(input);

    [HttpPut("{id}")]
    [Authorize(FluentCMSPermissions.MemberShips.Roles.Edit)]
    public Task<Role> UpdateAsync(Guid id, RoleUpdateRequest input) => _roleAppService.UpdateAsync(id, input);
}
