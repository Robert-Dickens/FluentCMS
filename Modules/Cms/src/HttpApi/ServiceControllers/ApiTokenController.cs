using FluentCMS.Api;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;
using System.Reflection;
using FluentCMS.ApiTokens;

namespace FluentCMS.Web.Api.Controllers;

public class ApiTokenController(IApiTokenService apiTokenService, IEnumerable<EndpointDataSource> endpointSources) : BaseGlobalController
{
    public const string AREA = "API Token Management";
    public const string READ = "Read";
    public const string UPDATE = $"Update/{READ}";
    public const string CREATE = "Create";
    public const string DELETE = $"Delete/{READ}";

    [HttpGet]
    [Policy(AREA, READ)]
    public async Task<IApiPagingResult<ApiTokenDetailResponse>> GetAll(CancellationToken cancellationToken = default)
    {
        var apiTokens = await apiTokenService.GetAll(cancellationToken);
        var apiTokensResponse = ObjectMapper.Map<IEnumerable<ApiToken>,List<ApiTokenDetailResponse>>(apiTokens);
        return OkPaged(apiTokensResponse);
    }

    [HttpGet("{id}")]
    [Policy(AREA, READ)]
    public async Task<IApiResult<ApiTokenDetailResponse>> GetById([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        var apiToken = await apiTokenService.GetById(id, cancellationToken);
        var apiTokenResponse = ObjectMapper.Map<ApiToken, ApiTokenDetailResponse>(apiToken);
        return Ok(apiTokenResponse);
    }

    [HttpPost]
    [Policy(AREA, CREATE)]
    public async Task<IApiResult<ApiTokenDetailResponse>> Create([FromBody] ApiTokenCreateRequest request, CancellationToken cancellationToken = default)
    {
        var apiToken = ObjectMapper.Map<ApiTokenCreateRequest, ApiToken>(request);
        await apiTokenService.Create(apiToken, cancellationToken);
        var apiTokenResponse = ObjectMapper.Map<ApiToken, ApiTokenDetailResponse>(apiToken);
        return Ok(apiTokenResponse);
    }

    [HttpPut]
    [Policy(AREA, UPDATE)]
    public async Task<IApiResult<ApiTokenDetailResponse>> Update([FromBody] ApiTokenUpdateRequest request, CancellationToken cancellationToken = default)
    {
        var apiToken = ObjectMapper.Map<ApiTokenUpdateRequest, ApiToken>(request);
        await apiTokenService.Update(apiToken, cancellationToken);
        var apiTokenResponse = ObjectMapper.Map<ApiToken, ApiTokenDetailResponse>(apiToken);
        return Ok(apiTokenResponse);
    }

    [HttpPut]
    [Policy(AREA, UPDATE)]
    public async Task<IApiResult<ApiTokenDetailResponse>> RegenerateSecret(Guid id, CancellationToken cancellationToken = default)
    {
        var apiToken = await apiTokenService.RegenerateSecret(id, cancellationToken);
        var apiTokenResponse = ObjectMapper.Map<ApiToken,ApiTokenDetailResponse>(apiToken);
        return Ok(apiTokenResponse);
    }

    [HttpDelete("{id}")]
    [Policy(AREA, DELETE)]
    public async Task<IApiResult<bool>> Delete([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        await apiTokenService.Delete(id, cancellationToken);
        return Ok(true);
    }

    [HttpGet]
    [Policy(AREA, READ)]
    public async Task<IApiPagingResult<Policy>> GetPolicies(CancellationToken cancellationToken = default)
    {
        var policiesDict = new Dictionary<string, Policy>();

        var endpoints = endpointSources.SelectMany(es => es.Endpoints).OfType<RouteEndpoint>();
        foreach (var endpoint in endpoints)
        {
            var actionDescriptor = endpoint.Metadata.OfType<ControllerActionDescriptor>().FirstOrDefault();
            if (actionDescriptor == null)
                continue;

            var policyAttributes = actionDescriptor.MethodInfo.GetCustomAttributes<PolicyAttribute>(true);

            foreach (var policyAttribute in policyAttributes)
            {
                if (!policiesDict.ContainsKey(policyAttribute.Area))
                    policiesDict.Add(policyAttribute.Area, new Policy { Area = policyAttribute.Area, Actions = [] });

                if (!policiesDict[policyAttribute.Area].Actions.Where(x => x == policyAttribute.Action).Any())
                    policiesDict[policyAttribute.Area].Actions.Add(policyAttribute.Action);
            }
        }
        var policies = policiesDict.Values.ToList();
        policies.Insert(0, new Policy { Area = "Global", Actions = ["All Actions"] });
        return await Task.FromResult(OkPaged(policies));
    }
}
