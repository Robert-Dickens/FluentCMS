using FluentCMS.Domain.Settings;
using FluentCMS.Permissions;
using FluentCMS.Settings;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCMS.Services.Settings;

[Authorize(FluentCMSPermissions.Settings.GlobalSettings.Default)]
public class GlobalSettingsAppService : FluentCMSCrudAppService<GlobalSettingsModel, GlobalSettings, Guid, GetGlobalSettingsRequest, GlobalSettingsCreateRequest, GlobalSettingsUpdateRequest>, IGlobalSettingsAppService
{
    public GlobalSettingsAppService(IGlobalSettingsModelRepository repository) : base(repository)
    {
        CreatePolicyName = FluentCMSPermissions.Settings.GlobalSettings.Create;
        UpdatePolicyName = FluentCMSPermissions.Settings.GlobalSettings.Edit;
        DeletePolicyName = FluentCMSPermissions.Settings.GlobalSettings.Delete;
    }

    public async Task<GlobalSettings?> Get(CancellationToken cancellationToken = default)
    {
        var model = await ((IGlobalSettingsModelRepository)Repository).Get(cancellationToken);
        return model is null ? null : ObjectMapper.Map<GlobalSettingsModel, GlobalSettings>(model);
    }
}
