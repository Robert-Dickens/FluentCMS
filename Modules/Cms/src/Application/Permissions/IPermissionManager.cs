using FluentCMS.Content;
using FluentCMS.Domain.Content;
using FluentCMS.Permissions;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCMS.Services.Permissions;

public interface IPermissionManager
{
    Task<bool> HasAccess(GlobalPermissionAction action, CancellationToken cancellationToken = default);
    Task<bool> HasAccess(Guid siteId, SitePermissionAction action, CancellationToken cancellationToken = default);
    Task<IEnumerable<Site>> GetAccessible(IEnumerable<Site> sites, SitePermissionAction action, CancellationToken cancellationToken = default);
    Task<IEnumerable<Site>> GetAccessible(IEnumerable<SiteModel> sites, SitePermissionAction action, CancellationToken cancellationToken = default);
}


