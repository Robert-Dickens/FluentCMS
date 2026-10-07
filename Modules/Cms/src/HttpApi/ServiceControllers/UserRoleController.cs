using FluentCMS.Api;
using FluentCMS.Membership;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCMS.Web.Api.Controllers;

public class UserRoleController(IUserRoleService userRoleService, IRoleService roleService) : BaseGlobalController
{
    public const string AREA = "User Role Management";
    public const string READ = "Read";
    public const string UPDATE = $"Update/{READ}";

    [HttpGet]
    [Policy(AREA, READ)]
    public async Task<IApiPagingResult<RoleDetailResponse>> GetUserRoles([FromQuery] Guid userId, Guid siteId, CancellationToken cancellationToken = default)
    {
        var userRoleIds = await userRoleService.GetUserRoleIds(userId, siteId, cancellationToken);
        var siteRoles = await roleService.GetAllForSite(siteId, cancellationToken);
        var userRoles = siteRoles.Where(x => userRoleIds.Contains(x.Id));
        var roleResponses = ObjectMapper.Map<IEnumerable<Role> ,IEnumerable <RoleDetailResponse>>(userRoles);
        return OkPaged(roleResponses);
    }

    [HttpPut]
    [Policy(AREA, UPDATE)]
    public async Task<IApiResult<bool>> Update([FromBody] UserRoleUpdateRequest request, CancellationToken cancellationToken = default)
    {
        await userRoleService.Update(request.UserId, request.SiteId, request.RoleIds, cancellationToken);

        return Ok(true);
    }
}
