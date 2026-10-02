using ByteLabs.Foundations;
using ByteLabs.Foundations.Application.Dtos;
using FluentCMS.Membership;
using FluentCMS.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCMS.Api.Controllers.Membership;

[Controller]
[RemoteService(Name = FluentCMSRemoteServiceConsts.RemoteServiceName)]
[Area(FluentCMSRemoteServiceConsts.ModuleAreaName)]
[Route($"api/{FluentCMSRemoteServiceConsts.RoutePath}/membership/users")]
[Authorize(FluentCMSPermissions.MemberShips.Members.Default)]
public class UserController : FluentCMSBaseController, IUserAppService
{
    private readonly IUserAppService _userAppService;

    public UserController(IUserAppService userAppService) => _userAppService = userAppService;

    [HttpPost]
    [Authorize(FluentCMSPermissions.MemberShips.Members.Create)]
    public Task<User> CreateAsync(UserCreateRequest input) => _userAppService.CreateAsync(input);

    [HttpPost("{id}/change-password")]
    public Task<bool> ChangePassword(Guid id, ChangePasswordRequest request, CancellationToken cancellationToken = default) =>
        _userAppService.ChangePassword(id, request, cancellationToken);

    [HttpDelete("{id}")]
    [Authorize(FluentCMSPermissions.MemberShips.Members.Delete)]
    public Task DeleteAsync(Guid id) => _userAppService.DeleteAsync(id);

    [HttpGet("{id}")]
    public Task<User> GetAsync(Guid id) => _userAppService.GetAsync(id);

    [HttpGet]
    public Task<PagedResultDto<User>> GetListAsync(GetUserRequest input) => _userAppService.GetListAsync(input);

    [HttpPut("{id}")]
    [Authorize(FluentCMSPermissions.MemberShips.Members.Edit)]
    public Task<User> UpdateAsync(Guid id, UserUpdateRequest input) => _userAppService.UpdateAsync(id, input);
}
