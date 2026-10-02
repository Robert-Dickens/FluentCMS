using ByteLabs.Foundations;
using ByteLabs.Foundations.Application.Dtos;
using FluentCMS.ApiTokens;
using FluentCMS.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace FluentCMS.Api.Controllers.ApiTokens;

[Controller]
[RemoteService(Name = FluentCMSRemoteServiceConsts.RemoteServiceName)]
[Area(FluentCMSRemoteServiceConsts.ModuleAreaName)]
[Route($"api/{FluentCMSRemoteServiceConsts.RoutePath}/api-tokens")]
[Authorize(FluentCMSPermissions.ApiTokens.ApiToken.Default)]
public class ApiTokenController : FluentCMSBaseController, IApiTokenAppService
{
    private readonly IApiTokenAppService _apiTokenAppService;

    public ApiTokenController(IApiTokenAppService apiTokenAppService) => _apiTokenAppService = apiTokenAppService;

    [HttpPost]
    [Authorize(FluentCMSPermissions.ApiTokens.ApiToken.Create)]
    public Task<ApiToken> CreateAsync(ApiTokenCreateRequest input) => _apiTokenAppService.CreateAsync(input);

    [HttpDelete("{id}")]
    [Authorize(FluentCMSPermissions.ApiTokens.ApiToken.Delete)]
    public Task DeleteAsync(Guid id) => _apiTokenAppService.DeleteAsync(id);

    [HttpGet("{id}")]
    public Task<ApiToken> GetAsync(Guid id) => _apiTokenAppService.GetAsync(id);

    [HttpGet]
    public Task<PagedResultDto<ApiToken>> GetListAsync(GetApiTokenRequest input) => _apiTokenAppService.GetListAsync(input);

    [HttpPut("{id}")]
    [Authorize(FluentCMSPermissions.ApiTokens.ApiToken.Edit)]
    public Task<ApiToken> UpdateAsync(Guid id, ApiTokenUpdateRequest input) => _apiTokenAppService.UpdateAsync(id, input);
}
