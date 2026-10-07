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

public class RoleService(IRoleModelRepository roleRepository, IMessagePublisher messagePublisher, IPermissionManager permissionManager) : FluentCMSServiceAppService, IRoleService, ITransientDependency
{
    public async Task<IEnumerable<Role>> GetAllForSite(Guid siteId, CancellationToken cancellationToken = default)
    {
        // no need to check for permission 
        var resules = await roleRepository.GetAllForSite(siteId, cancellationToken);
        return ObjectMapper.Map<IEnumerable<RoleModel>, List<Role>>(resules);
    }

    public async Task<Role> Create(Role role, CancellationToken cancellationToken)
    {
        if (!await permissionManager.HasAccess(role.SiteId, SitePermissionAction.SiteAdmin, cancellationToken))
            throw new AppException(ExceptionCodes.PermissionDenied);

        // check for duplicated role name
        var allRoles = await roleRepository.GetAllForSite(role.SiteId, cancellationToken);
        if (allRoles.Where(r => r.Name.Equals(role.Name, StringComparison.CurrentCultureIgnoreCase)).Any())
            throw new AppException(ExceptionCodes.RoleNameShouldBeUnique);

        var roleEntity = ObjectMapper.Map<Role, RoleModel>(role);

        var newRole = await roleRepository.InsertAsync(roleEntity, cancellationToken: cancellationToken) ??
           throw new AppException(ExceptionCodes.RoleUnableToCreate);

        var roleCreated = ObjectMapper.Map<RoleModel, Role>(newRole);
        await messagePublisher.Publish(new Message<Role>(ActionNames.RoleCreated, roleCreated), cancellationToken);

        return roleCreated;
    }

    public async Task<Role> Update(Role role, CancellationToken cancellationToken)
    {
        var existingRole = await roleRepository.FindAsync(role.Id, cancellationToken: cancellationToken) ??
            throw new AppException(ExceptionCodes.RoleNotFound);

        // role type can't be changed after creation
        role.Type = existingRole.Type;

        var siteId = existingRole.SiteId;

        if (!await permissionManager.HasAccess(siteId, SitePermissionAction.SiteAdmin, cancellationToken))
            throw new AppException(ExceptionCodes.PermissionDenied);

        // Check if role name is changed
        if (!existingRole.Name.Equals(role.Name, StringComparison.CurrentCultureIgnoreCase))
        {
            // Check for duplicated role name 
            var allRoles = await roleRepository.GetAllForSite(siteId, cancellationToken);
            if (allRoles.Where(r => r.Name.Equals(role.Name, StringComparison.CurrentCultureIgnoreCase)).Any())
                throw new AppException(ExceptionCodes.RoleNameShouldBeUnique);
        }
        var roleEntity = ObjectMapper.Map<Role, RoleModel>(role);

        var updatedRole = await roleRepository.UpdateAsync(roleEntity, cancellationToken: cancellationToken) ??
            throw new AppException(ExceptionCodes.RoleUnableToUpdate);

        var roleUpdated = ObjectMapper.Map<RoleModel, Role>(updatedRole);

        await messagePublisher.Publish(new Message<Role>(ActionNames.RoleUpdated, roleUpdated), cancellationToken);

        return roleUpdated;
    }

    public async Task<Role> Delete(Guid roleId, CancellationToken cancellationToken = default)
    {
        var existingRole = await roleRepository.FindAsync(roleId, cancellationToken: cancellationToken) ??
            throw new AppException(ExceptionCodes.RoleNotFound);

        if (!await permissionManager.HasAccess(existingRole.SiteId, SitePermissionAction.SiteAdmin, cancellationToken))
            throw new AppException(ExceptionCodes.PermissionDenied);

        // Only user defined roles can be deleted
        if (existingRole.Type != RoleTypes.UserDefined)
            throw new AppException(ExceptionCodes.RoleCanNotBeDeleted);

        var deletedRole = await roleRepository.DeleteWithResultAsync(roleId, cancellationToken: cancellationToken) ??
            throw new AppException(ExceptionCodes.RoleUnableToDelete);

        var roleDeleted = ObjectMapper.Map<RoleModel, Role>(deletedRole);
        await messagePublisher.Publish(new Message<Role>(ActionNames.RoleDeleted, roleDeleted), cancellationToken);

        return roleDeleted;
    }

    public async Task<Role> GetById(Guid roleId, CancellationToken cancellationToken = default)
    {
        // no need to check for permission
        var role = await roleRepository.FindAsync(roleId, cancellationToken: cancellationToken) ??
            throw new AppException(ExceptionCodes.RoleNotFound);
        return ObjectMapper.Map<RoleModel, Role>(role);
    }
}
