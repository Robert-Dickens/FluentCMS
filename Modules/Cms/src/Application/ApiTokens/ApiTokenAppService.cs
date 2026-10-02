using FluentCMS.ApiTokens;
using FluentCMS.Domain.ApiTokens;
using FluentCMS.Permissions;
using Microsoft.AspNetCore.Authorization;
using System;

namespace FluentCMS.Services.ApiTokens;

[Authorize(FluentCMSPermissions.ApiTokens.ApiToken.Default)]
public class ApiTokenAppService : FluentCMSCrudAppService<ApiTokenModel, ApiToken, Guid, GetApiTokenRequest, ApiTokenCreateRequest, ApiTokenUpdateRequest>, IApiTokenAppService
{
    public ApiTokenAppService(IApiTokenModelRepository repository) : base(repository)
    {
        CreatePolicyName = FluentCMSPermissions.ApiTokens.ApiToken.Create;
        UpdatePolicyName = FluentCMSPermissions.ApiTokens.ApiToken.Edit;
        DeletePolicyName = FluentCMSPermissions.ApiTokens.ApiToken.Delete;
    }
}
