using FluentCMS.Api;
using FluentCMS.Settings;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCMS.Web.Api.Controllers;

public class GlobalSettingsController(IGlobalSettingsService service) : BaseGlobalController
{
    public const string AREA = "Settings Management";
    public const string READ = "Read";
    public const string UPDATE = "Update";

    [HttpGet]
    [Policy(AREA, READ)]
    public async Task<IApiResult<GlobalSettingsResponse>> Get(CancellationToken cancellationToken = default)
    {
        var settings = await service.Get(cancellationToken);
        var settingsResponse = ObjectMapper.Map<GlobalSettings, GlobalSettingsResponse>(settings);

        return Ok(settingsResponse);
    }

    [HttpPost]
    [Policy(AREA, UPDATE)]
    public async Task<IApiResult<GlobalSettingsResponse>> Update(GlobalSettingsUpdateRequest request, CancellationToken cancellationToken = default)
    {
        var settings = ObjectMapper.Map<GlobalSettingsUpdateRequest, GlobalSettings>(request);
        var updated = await service.Update(settings, cancellationToken);
        var settingsResponse = ObjectMapper.Map<GlobalSettings, GlobalSettingsResponse>(updated);

        return Ok(settingsResponse);
    }
}
