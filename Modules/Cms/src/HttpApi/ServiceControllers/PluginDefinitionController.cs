using FluentCMS.Api;
using FluentCMS.Plugins;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCMS.Web.Api.Controllers;

public class PluginDefinitionController(IPluginDefinitionService pluginDefinitionService) : BaseGlobalController
{
    public const string AREA = "Plugin Definition Management";
    public const string READ = "Read";
    public const string CREATE = "Create";

    [HttpPost]
    [Policy(AREA, CREATE)]
    public async Task<IApiResult<PluginDefinitionDetailResponse>> Create([FromBody] PluginDefinitionCreateRequest request, CancellationToken cancellationToken = default)
    {
        var entity = ObjectMapper.Map<PluginDefinitionCreateRequest, PluginDefinition>(request);
        var created = await pluginDefinitionService.Create(entity, cancellationToken);
        var response = ObjectMapper.Map<PluginDefinition, PluginDefinitionDetailResponse>(created);
        return Ok(response);
    }

    [HttpGet]
    [Policy(AREA, READ)]
    public async Task<IApiPagingResult<PluginDefinitionDetailResponse>> GetAll(CancellationToken cancellationToken = default)
    {
        var entities = await pluginDefinitionService.GetAll(cancellationToken);
        var entitiesResponse = ObjectMapper.Map<IEnumerable<PluginDefinition> ,List<PluginDefinitionDetailResponse>>(entities);
        return OkPaged(entitiesResponse);
    }
}
