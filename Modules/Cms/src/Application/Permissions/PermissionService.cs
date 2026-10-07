using ByteLabs.Foundations.DependencyInjection;
using FluentCMS.Content;
using FluentCMS.Domain.Permissions;
using FluentCMS.Permissions;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCMS.Services.Permissions;

public class PermissionService(IPermissionModelRepository permissionRepository) : FluentCMSServiceAppService, IPermissionService, ITransientDependency
{
    public async Task<IEnumerable<Permission>> Set(Guid siteId, SitePermissionAction action, IEnumerable<Guid> roleIds, CancellationToken cancellationToken = default)
    {
        var results = await permissionRepository.Set(siteId, siteId, nameof(Site), action.ToString(), roleIds, cancellationToken);
        return ObjectMapper.Map<IEnumerable<PermissionModel>, IEnumerable<Permission>>(results);
    }

    public async Task<IEnumerable<Permission>> Set(Guid siteId, Guid pageId, PagePermissionAction action, IEnumerable<Guid> roleIds, CancellationToken cancellationToken = default)
    {
        var results = await permissionRepository.Set(siteId, pageId, nameof(Page), action.ToString(), roleIds, cancellationToken);
        return ObjectMapper.Map<IEnumerable<PermissionModel>, IEnumerable<Permission>>(results);
    }

    public async Task<IEnumerable<Permission>> Get(Guid siteId, SitePermissionAction action, CancellationToken cancellationToken = default)
    {
        var results = await permissionRepository.Get(siteId, siteId, nameof(Site), action.ToString(), cancellationToken);
        return ObjectMapper.Map<IEnumerable<PermissionModel>, IEnumerable<Permission>>(results);
    }

    public async Task<IEnumerable<Permission>> Get(Guid siteId, Guid pageId, PagePermissionAction action, CancellationToken cancellationToken = default)
    {
        var results = await permissionRepository.Get(siteId, pageId, nameof(Page), action.ToString(), cancellationToken);
        return ObjectMapper.Map<IEnumerable<PermissionModel>, IEnumerable<Permission>>(results);
    }
}
