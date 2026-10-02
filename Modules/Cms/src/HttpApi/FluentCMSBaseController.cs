using ByteLabs.Foundations.AspNetCore.Mvc;
using FluentCMS.Localization;

namespace FluentCMS.Api;

public abstract class FluentCMSBaseController : AspNetCoreController
{
    protected FluentCMSBaseController()
    {
        LocalizationResource = typeof(FluentCMSResource);
    }
}
