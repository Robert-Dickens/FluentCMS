using ByteLabs.Foundations.Application;
using ByteLabs.Foundations.Authorization;
using ByteLabs.Foundations.Modularity;

namespace FluentCMS;

[DependsOn(
    typeof(FluentCMSDomainAbstractionsModule),
    typeof(PlatformApplicationAbstractionsModule),
    typeof(PlatformAuthorizationModule)
    )]
public class FluentCMSApplicationAbstractionsModule : PlatformModule
{

}
