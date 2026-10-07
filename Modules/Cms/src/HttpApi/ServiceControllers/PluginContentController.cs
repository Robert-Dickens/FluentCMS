using FluentCMS.Api;
using FluentCMS.Plugins;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCMS.Web.Api.Controllers;

[Route("api/[controller]/{pluginContentTypeName}/[action]")]
public class PluginContentController(IPluginContentService pluginContentService) : BaseGlobalController
{
    public const string AREA = "Plugin Content Management";
    public const string READ = "Read";
    public const string CREATE = "Create";
    public const string UPDATE = $"Update/{READ}";
    public const string DELETE = $"Delete/{READ}";

    [HttpGet("{pluginId}")]
    [Policy(AREA, READ)]
    public async Task<IApiPagingResult<PluginContentDetailResponse>> GetAll([FromRoute] string pluginContentTypeName, [FromRoute, Required] Guid pluginId, CancellationToken cancellationToken = default)
    {
        var contents = await pluginContentService.GetByPluginId(pluginId, cancellationToken);
        var response = ObjectMapper.Map<IEnumerable<PluginContent>, List<PluginContentDetailResponse>>(contents.ToList());
        return OkPaged(response);
    }

    [HttpGet("{pluginId}/{id}")]
    [Policy(AREA, READ)]
    public async Task<IApiResult<PluginContentDetailResponse>> GetById([FromRoute] string pluginContentTypeName, [FromRoute, Required] Guid pluginId, [FromRoute, Required] Guid id, CancellationToken cancellationToken = default)
    {
        var pluginContent = await pluginContentService.GetById(pluginContentTypeName, id, cancellationToken);
        var response = ObjectMapper.Map<PluginContent, PluginContentDetailResponse>(pluginContent);
        return Ok(response);
    }

    [HttpPost("{pluginId}")]
    [Policy(AREA, CREATE)]
    public async Task<IApiResult<PluginContentDetailResponse>> Create([FromRoute] string pluginContentTypeName, [FromRoute, Required] Guid pluginId, [FromBody] Dictionary<string, object?> request, CancellationToken cancellationToken = default)
    {
        var pluginContent = new PluginContent
        {
            PluginId = pluginId,
            Type = pluginContentTypeName,
            Data = request
        };

        await pluginContentService.Create(pluginContent, cancellationToken);

        var response = ObjectMapper.Map<PluginContent, PluginContentDetailResponse>(pluginContent);

        return Ok(response);
    }

    [HttpPut("{pluginId}/{id}")]
    [Policy(AREA, UPDATE)]
    public async Task<IApiResult<PluginContentDetailResponse>> Update([FromRoute] string pluginContentTypeName, [FromRoute, Required] Guid pluginId, [FromRoute, Required] Guid id, [FromBody] Dictionary<string, object?> request, CancellationToken cancellationToken = default)
    {
        var pluginContent = new PluginContent
        {
            Id = id,
            PluginId = pluginId,
            Type = pluginContentTypeName,
            Data = request
        };

        await pluginContentService.Update(pluginContent, cancellationToken);

        var response = ObjectMapper.Map<PluginContent, PluginContentDetailResponse>(pluginContent);

        return Ok(response);
    }

    [HttpDelete("{pluginId}/{id}")]
    [Policy(AREA, DELETE)]
    public async Task<IApiResult<bool>> Delete([FromRoute] string pluginContentTypeName, [FromRoute, Required] Guid pluginId, [FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        await pluginContentService.Delete(pluginContentTypeName, id, cancellationToken);
        return Ok(true);
    }

}
