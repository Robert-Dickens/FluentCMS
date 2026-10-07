using ByteLabs.Foundations.AspNetCore.Mvc;
using FluentCMS.Localization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace FluentCMS.Api;

public abstract class FluentCMSBaseController : AspNetCoreController
{
    protected FluentCMSBaseController()
    {
        LocalizationResource = typeof(FluentCMSResource);
    }
}

[ApiController]
[Produces("application/json")]
public abstract class BaseController : FluentCMSBaseController
{
    public static ApiResult<T> Ok<T>(T item)
    {
        return new ApiResult<T>(item);
    }

    public static ApiPagingResult<T> OkPaged<T>(IEnumerable<T> items)
    {
        return new ApiPagingResult<T>(items);
    }
}

[Route("api/[controller]/[action]")]
public abstract class BaseGlobalController : BaseController
{
}
