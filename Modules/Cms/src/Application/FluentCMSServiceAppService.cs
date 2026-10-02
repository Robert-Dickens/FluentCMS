using ByteLabs.Foundations.Application.Services;
using FluentCMS.Localization;

namespace FluentCMS.Services;

public abstract class FluentCMSServiceAppService : ApplicationService
{
    protected FluentCMSServiceAppService()
    {
        LocalizationResource = typeof(FluentCMSResource);
        ObjectMapperContext = typeof(FluentCMSApplicationModule);
    }
}
