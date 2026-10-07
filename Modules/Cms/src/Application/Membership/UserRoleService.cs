using ByteLabs.Foundations.DependencyInjection;
using FluentCMS.Domain.Membership;
using FluentCMS.Membership;
using FluentCMS.Permissions;
using FluentCMS.Providers.MessageBusProviders;
using FluentCMS.Services.Permissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCMS.Services.Membership;

public class UserRoleService(IUserRoleModelRepository userRoleRepository, IRoleModelRepository roleRepository, IMessagePublisher messagePublisher, IPermissionManager permissionManager) : FluentCMSServiceAppService, IUserRoleService, ITransientDependency
{
    public async Task<IEnumerable<UserRole>> DeleteByRoleId(Guid roleId, CancellationToken cancellationToken = default)
    {
        var userRoles = await userRoleRepository.GetByRoleId(roleId, cancellationToken);

        var deleted = await userRoleRepository.DeleteManyWithResultAsync(userRoles.Select(x => x.Id), cancellationToken);

        // creating tasks for the message publisher and await them all to finish
        var tasks = deleted?.Select(item => messagePublisher.Publish(new Message<UserRole>(ActionNames.UserRoleDeleted, ObjectMapper.Map<UserRoleModel, UserRole>(item)), cancellationToken)).ToList() ?? [];
        await Task.WhenAll(tasks);

        return ObjectMapper.Map<IEnumerable<UserRoleModel>, List<UserRole>>(userRoles);
    }

    public async Task<IEnumerable<UserRole>> DeleteByUserId(Guid userId, CancellationToken cancellationToken = default)
    {
        if (!await permissionManager.HasAccess(GlobalPermissionAction.SuperAdmin, cancellationToken))
            throw new AppException(ExceptionCodes.PermissionDenied);

        var userRoles = await userRoleRepository.GetByUserId(userId, cancellationToken);

        var deleted = await userRoleRepository.DeleteManyWithResultAsync(userRoles.Select(x => x.Id), cancellationToken);

        // creating tasks for the message publisher and await them all to finish
        var tasks = deleted?.Select(item => messagePublisher.Publish(new Message<UserRole>(ActionNames.UserRoleDeleted, ObjectMapper.Map<UserRoleModel, UserRole>(item)), cancellationToken)).ToList() ?? [];

        await Task.WhenAll(tasks);

        return ObjectMapper.Map<IEnumerable<UserRoleModel>, List<UserRole>>(userRoles);
    }

    public async Task<IEnumerable<UserRole>> GetAllForSite(Guid siteId, CancellationToken cancellationToken = default)
    {
        if (!await permissionManager.HasAccess(siteId, SitePermissionAction.SiteAdmin, cancellationToken))
            throw new AppException(ExceptionCodes.PermissionDenied);

        var results = await userRoleRepository.GetAllForSite(siteId, cancellationToken);
        return ObjectMapper.Map<IEnumerable<UserRoleModel>, List<UserRole>>(results);
    }

    public async Task<IEnumerable<Guid>> GetUserRoleIds(Guid userId, Guid siteId, CancellationToken cancellationToken = default)
    {
        // check if the user is the same as the one in the token or site admin
        if (!await permissionManager.HasAccess(siteId, SitePermissionAction.SiteAdmin, cancellationToken) && CurrentUser.Id != userId)
            throw new AppException(ExceptionCodes.PermissionDenied);

        var allRoles = await roleRepository.GetAllForSite(siteId, cancellationToken);
        if (!CurrentUser.IsAuthenticated)
            return allRoles.Where(x => x.Type == RoleTypes.Guest || x.Type == RoleTypes.AllUsers).Select(x => x.Id);

        var userRoles = await userRoleRepository.GetUserRoles(userId, siteId, cancellationToken) ?? [];
        var defaultRoles = allRoles.Where(x => x.Type == RoleTypes.Authenticated || x.Type == RoleTypes.AllUsers) ?? [];

        return userRoles.Select(x => x.RoleId).Concat(defaultRoles.Select(x => x.Id)).ToList();
    }

    public async Task<IEnumerable<UserRole>> Update(Guid userId, Guid siteId, IEnumerable<Guid> roleIds, CancellationToken cancellationToken = default)
    {
        if (!await permissionManager.HasAccess(siteId, SitePermissionAction.SiteAdmin, cancellationToken))
            throw new AppException(ExceptionCodes.PermissionDenied);

        // find and remove default roles, just accept user defined roles.
        var allRoles = await roleRepository.GetAllForSite(siteId, cancellationToken);
        var validRoleIds = roleIds.Intersect(allRoles.Where(x => x.Type == RoleTypes.UserDefined || x.Type == RoleTypes.Administrators).Select(x => x.Id));

        // delete all exist UserRoles. 
        var existUserRoles = await userRoleRepository.GetUserRoles(userId, siteId, cancellationToken);
        await userRoleRepository.DeleteManyWithResultAsync(existUserRoles.Select(x => x.Id), cancellationToken);

        await messagePublisher.Publish(new Message<IEnumerable<UserRole>>(ActionNames.UserRoleDeleted, ObjectMapper.Map<IEnumerable<UserRoleModel>, List<UserRole>>(existUserRoles)), cancellationToken);

        // add all new UserRoles
        var userRoles = validRoleIds.Select(x => new UserRoleModel
        {
            SiteId = siteId,
            RoleId = x,
            UserId = userId,
        });

        var results = new List<UserRole>();

        foreach (var userRole in userRoles)
        {
            var result = await userRoleRepository.InsertAsync(userRole, cancellationToken: cancellationToken);
            results.Add(ObjectMapper.Map<UserRoleModel, UserRole>(result));
        }

        await messagePublisher.Publish(new Message<IEnumerable<UserRole>>(ActionNames.UserRoleCreated, results), cancellationToken);

        return results;
    }
}
