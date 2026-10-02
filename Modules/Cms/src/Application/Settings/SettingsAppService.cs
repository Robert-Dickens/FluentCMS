using FluentCMS.Domain.Settings;
using FluentCMS.Permissions;
using FluentCMS.Settings;
using Microsoft.AspNetCore.Authorization;
using System;
using SettingsDto = FluentCMS.Settings.Settings;

namespace FluentCMS.Services.Settings;

[Authorize(FluentCMSPermissions.Settings.TenantSettings.Default)]
public class SettingsAppService : FluentCMSCrudAppService<SettingsModel, SettingsDto, Guid, GetSettingsRequest, SettingsCreateRequest, SettingsUpdateRequest>, ISettingsAppService
{
    public SettingsAppService(ISettingsModelRepository repository) : base(repository)
    {
        CreatePolicyName = FluentCMSPermissions.Settings.TenantSettings.Create;
        UpdatePolicyName = FluentCMSPermissions.Settings.TenantSettings.Edit;
        DeletePolicyName = FluentCMSPermissions.Settings.TenantSettings.Delete;
    }
}
