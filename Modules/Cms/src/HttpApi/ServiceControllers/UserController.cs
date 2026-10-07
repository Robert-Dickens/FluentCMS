using FluentCMS.Api;
using FluentCMS.Membership;
using FluentCMS.Settings;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCMS.Web.Api.Controllers;

public class UserController(IUserService userService, IGlobalSettingsService globalSettingsService) : BaseGlobalController
{
    public const string AREA = "User Management";
    public const string READ = "Read";
    public const string UPDATE = $"Update/{READ}";
    public const string CREATE = "Create";
    public const string DELETE = $"Delete/{READ}";

    [HttpGet]
    [Policy(AREA, READ)]
    public async Task<IApiPagingResult<UserDetailResponse>> GetAll(CancellationToken cancellationToken = default)
    {
        var users = (await userService.GetAll(cancellationToken)).ToList();
        var usersResponse = ObjectMapper.Map<IEnumerable<User>, List<UserDetailResponse>>(users);

        // setting super admin flag
        var globalSettings = await globalSettingsService.Get(cancellationToken);
        if (globalSettings != null)
            usersResponse.ForEach(user => user.IsSuperAdmin = globalSettings.SuperAdmins.Contains(user.Username));

        return OkPaged(usersResponse);
    }

    [HttpGet("{userId}")]
    [Policy(AREA, READ)]
    public async Task<IApiResult<UserDetailResponse>> Get([FromRoute] Guid userId, CancellationToken cancellationToken = default)
    {
        var user = await userService.GetById(userId, cancellationToken);
        var userResponse = ObjectMapper.Map<User,UserDetailResponse>(user);

        return Ok(userResponse);
    }

    [HttpPut]
    [Policy(AREA, UPDATE)]
    public async Task<IApiResult<UserDetailResponse>> Update([FromBody] UserUpdateRequest request, CancellationToken cancellationToken = default)
    {
        var user = ObjectMapper.Map<UserUpdateRequest, User>(request);
        var updated = await userService.Update(user, cancellationToken);
        var userResponse = ObjectMapper.Map<User,UserDetailResponse>(updated);

        return Ok(userResponse);
    }

    [HttpPut]
    [Policy(AREA, UPDATE)]
    public async Task<IApiResult<bool>> SetPassword([FromBody] UserSetPasswordRequest request, CancellationToken cancellationToken = default)
    {
        var user = await userService.GetById(request.UserId, cancellationToken);
        await userService.ChangePassword(user, request.Password, cancellationToken);
        return Ok(true);
    }

    [HttpPost]
    [Policy(AREA, CREATE)]
    public async Task<IApiResult<UserDetailResponse>> Create([FromBody] UserCreateRequest request, CancellationToken cancellationToken = default)
    {
        var user = ObjectMapper.Map<UserCreateRequest,User>(request);
        var created = await userService.Create(user, request.Password, cancellationToken);
        var userResponse = ObjectMapper.Map<User, UserDetailResponse>(created);

        return Ok(userResponse);
    }

    [HttpDelete("{id}")]
    [Policy(AREA, DELETE)]
    public async Task<IApiResult<bool>> Delete([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        await userService.Delete(id, cancellationToken);
        return Ok(true);
    }
}
