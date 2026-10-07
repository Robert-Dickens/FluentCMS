using FluentCMS.Api;
using FluentCMS.Settings;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCMS.Web.Api.Controllers;

public class SettingsController(ISettingsService settingsService) : BaseGlobalController
{
    public const string AREA = "Settings Management";
    public const string READ = "Read";
    public const string UPDATE = $"Update/{READ}";

    [HttpPut]
    [Policy(AREA, UPDATE)]
    public async Task<IApiResult<SettingsDetailResponse>> Update([FromBody] SettingsUpdateRequest request, CancellationToken cancellationToken = default)
    {
        var settings = await settingsService.Update(request.Id, request.Settings, cancellationToken);
        var response = new SettingsDetailResponse
        {
            Id = settings.Id,
            Values = settings.Values
        };

        return Ok(response);
    }
}
